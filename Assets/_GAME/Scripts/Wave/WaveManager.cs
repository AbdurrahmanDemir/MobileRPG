using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("Elements")]
    [SerializeField] private Wave[] waves;
    private Wave currentWave;
    [SerializeField] private Transform[] creatEnemyPosition;
    [SerializeField] private WaveUIManager waveUI;

    [Header("Settings")]
    [SerializeField] private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;
    private int currentSegmentIndex;
    private int currentEnemyIndex;
    private int currentEnemyCount;
    private float segmentDelay = 5f; // Delay between segments

    [Header("Action")]
    private bool onThrow = false;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Hook.onThrowStarting += OnThrowStartingCallBack;
        Hook.onThrowEnding += OnThrowEndingCallBack;
    }
    private void OnDestroy()
    {
        Hook.onThrowStarting -= OnThrowStartingCallBack;
        Hook.onThrowEnding -= OnThrowEndingCallBack;
    }

    //private void Start()
    //{
    //    //StartWaves(0);
    //    //isTimerOn = true;
    //}

    private void Update()
    {
        if (!isTimerOn)
            return;

        ManageCurrentWave();
    }

    public void StartWaves(int index)
    {
        currentWaveIndex = index;
        currentSegmentIndex = 0;
        currentEnemyIndex = 0;
        currentWave = waves[currentWaveIndex];
        isTimerOn = true;
        SetupNextSegment();
        waveUI.waveSegmentText.text = "Wave " + currentSegmentIndex + " / " + currentWave.segments.Count;
    }

    private void ManageCurrentWave()
    {
        if (currentSegmentIndex >= currentWave.segments.Count)
        {
            isTimerOn = false;
            Debug.Log("Wave Completed");
            return;
        }

        if (onThrow)
            return;


        WaveSegmet currentSegment = currentWave.segments[currentSegmentIndex];

        timer += Time.deltaTime;

        if (timer >= currentSegment.segmetDuration)
        {
            if (SpawnEnemy(currentSegment))
            {
                timer = 0;
            }
            else
            {
                // Move to the next segment after a delay
                currentSegmentIndex++;
                waveUI.waveSegmentText.text = "Wave " + currentSegmentIndex + " / " + currentWave.segments.Count;
                if (currentSegmentIndex < currentWave.segments.Count)
                {
                    Invoke("StartNextSegment", segmentDelay);
                }
                else
                {
                    if (currentWaveIndex + 1 < waves.Length)
                    {
                        StartWaves(currentWaveIndex + 1);
                    }
                }
            }
        }
    }

    private void StartNextSegment()
    {
        isTimerOn = true;
        SetupNextSegment();
    }

    private void SetupNextSegment()
    {
        currentEnemyIndex = 0;
        if (currentSegmentIndex < currentWave.segments.Count)
        {
            currentEnemyCount = currentWave.segments[currentSegmentIndex].segmentEnemys[currentEnemyIndex].enemyCount;
        }
    }

    private bool SpawnEnemy(WaveSegmet segment)
    {
        if (currentEnemyCount <= 0)
        {
            currentEnemyIndex++;
            if (currentEnemyIndex < segment.segmentEnemys.Length)
            {
                currentEnemyCount = segment.segmentEnemys[currentEnemyIndex].enemyCount;
            }
            else
            {
                return false; 
            }
        }

        int randomCreatPos = Random.Range(0, creatEnemyPosition.Length);
        Instantiate(
            segment.segmentEnemys[currentEnemyIndex].enemy[Random.Range(0, segment.segmentEnemys[currentEnemyIndex].enemy.Length)],
            creatEnemyPosition[randomCreatPos].position,
            Quaternion.Euler(0f, 180f, 0f));
        ;
        currentEnemyCount--;
        return true;
    }

    public void OnThrowStartingCallBack()
    {
        onThrow = true;
        Debug.Log("Avtipn çalýþtý" + onThrow);
    }
    public void OnThrowEndingCallBack()
    {
        onThrow = false;
        Debug.Log("Avtipn çalýþtý" + onThrow);

    }
}

[Serializable]
public struct Wave
{
    public string waveName;
    public List<WaveSegmet> segments;
}

[Serializable]
public struct WaveSegmet
{
    public float segmetDuration;
    public WaveSegmentEnemyManage[] segmentEnemys;
}

[Serializable]
public struct WaveSegmentEnemyManage
{
    public GameObject[] enemy;
    public int enemyCount;
}
