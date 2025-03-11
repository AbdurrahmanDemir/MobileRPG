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
    private int currentSegmentIndex;

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
        currentWave= waves[index];
    }

    private void ManageCurrentWave()
    {
        currentSegmentIndex= 0;

        currentWave = waves[currentSegmentIndex];

        for (int i = 0; i < currentWave.segments.Count; i++)
        {
            WaveSegmet segmet = currentWave.segments[i];
            int segmentEnemyIndex = 0;
            int enemyCount = segmet.segmentEnemys[segmentEnemyIndex].enemyCount;

            if (segmentEnemyIndex >= segmet.segmentEnemys.Length)
            {
                currentSegmentIndex++;
                isTimerOn= false;
            }

            for (int t = 0; t < enemyCount; t++)
            {
                timer += Time.deltaTime;

                int randomCreatPos = Random.Range(0, creatEnemyPosition.Length);

                if (timer >= segmet.segmetDuration)
                {
                    Instantiate(segmet.segmentEnemys[t].enemy, creatEnemyPosition[randomCreatPos]);
                    timer = 0;
                }

            }
            segmentEnemyIndex++;



        }
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
    public GameObject enemy;
    public int enemyCount;
}
