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
    [SerializeField] private Transform enemyParent;
    [SerializeField] private WaveUIManager waveUI;

    [Header("Settings")]
    [SerializeField] private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;
    private int currentSegmentIndex;
    private int currentEnemySubIndex;
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
                // Bir gecikme sonrasýnda bir sonraki segmente geç
                currentSegmentIndex++;
                Debug.Log("Moving to next segment. Current Index: " + currentSegmentIndex);
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
        Debug.Log("Starting next segment. Current Index: " + currentSegmentIndex);
        SetupNextSegment();
    }

    private void SetupNextSegment()
    {
        currentEnemyIndex = 0;
        currentEnemySubIndex = 0; // Yeni deðiþkeni sýfýrla
        if (currentSegmentIndex < currentWave.segments.Count)
        {
            if (currentWave.segments[currentSegmentIndex].segmentEnemys.Length > 0)
            {
                currentEnemyCount = currentWave.segments[currentSegmentIndex].segmentEnemys[currentEnemyIndex].enemyCount;
                Debug.Log("Setting up next segment. Enemy Count: " + currentEnemyCount);
            }
            else
            {
                Debug.LogError("No enemies defined in the current segment.");
            }
        }
    }

    private bool SpawnEnemy(WaveSegmet segment)
    {
        // Check if we've processed all enemy types in this segment
        if (currentEnemyIndex >= segment.segmentEnemys.Length)
        {
            return false;
        }

        // Check if we've processed all enemy variations for the current enemy type
        if (currentEnemySubIndex >= segment.segmentEnemys[currentEnemyIndex].enemy.Length)
        {
            currentEnemyIndex++;
            currentEnemySubIndex = 0;

            // Check if we've processed all enemy types after incrementing
            if (currentEnemyIndex >= segment.segmentEnemys.Length)
            {
                return false;
            }

            currentEnemyCount = segment.segmentEnemys[currentEnemyIndex].enemyCount;
            return true; // Return true to try again with the new enemy type
        }

        // If we've spawned all enemies of the current variation
        if (currentEnemyCount <= 0)
        {
            currentEnemySubIndex++;

            // If there are more variations of this enemy type
            if (currentEnemySubIndex < segment.segmentEnemys[currentEnemyIndex].enemy.Length)
            {
                currentEnemyCount = segment.segmentEnemys[currentEnemyIndex].enemyCount;
            }
            else
            {
                // Move to next enemy type
                currentEnemyIndex++;
                currentEnemySubIndex = 0;

                // If there are more enemy types
                if (currentEnemyIndex < segment.segmentEnemys.Length)
                {
                    currentEnemyCount = segment.segmentEnemys[currentEnemyIndex].enemyCount;
                }
                else
                {
                    return false;
                }
            }
        }

        // Spawn the enemy
        int randomCreatPos = Random.Range(0, creatEnemyPosition.Length);
        Instantiate(
            segment.segmentEnemys[currentEnemyIndex].enemy[currentEnemySubIndex],
            creatEnemyPosition[randomCreatPos].position,
            Quaternion.Euler(0f, 180f, 0f), enemyParent);

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
