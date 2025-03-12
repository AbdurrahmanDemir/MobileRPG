using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Wave[] waves;
    private Wave currentWave;
    [SerializeField] private Transform[] creatEnemyPosition;

    [Header("Settings")]
    [SerializeField] private float timer;
    private bool isTimerOn;
    private int currentWaveIndex;
    private int currentSegmentIndex;
    private int currentEnemyIndex;
    private int currentEnemyCount;
    private float segmentDelay = 5f; // Delay between segments

    private void Start()
    {
        StartWaves(0);
        isTimerOn = true;
    }

    private void Update()
    {
        if (!isTimerOn)
            return;

        ManageCurrentWave();
    }

    void StartWaves(int index)
    {
        currentWaveIndex = index;
        currentSegmentIndex = 0;
        currentEnemyIndex = 0;
        currentWave = waves[currentWaveIndex];
        isTimerOn = true;
        SetupNextSegment();
    }

    private void ManageCurrentWave()
    {
        if (currentSegmentIndex >= currentWave.segments.Count)
        {
            isTimerOn = false;
            Debug.Log("Wave Completed");
            return;
        }

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
            Quaternion.identity);
        currentEnemyCount--;
        return true;
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
