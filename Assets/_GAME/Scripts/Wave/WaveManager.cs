using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    //[SerializeField] private EnemyTowerController enemyTowerController;

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
    public int currentEnemyCount;
    private float segmentDelay = 0.1f;

    [Header("Enemy Tracking")]
    private int totalEnemiesSpawned = 0;
    private int totalEnemiesKilled = 0;
    private int aliveEnemiesCount = 0; // Sahne içinde yaþayan düþman sayýsý
    private int totalEnemiesInWave = 0; // Wave'deki toplam düþman sayýsý
    private bool allEnemiesSpawned = false; // Tüm düþmanlar spawn edildi mi?

    [Header("Action")]
    private bool onThrow = false;
    public static Action onGameWin;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Düþman ölüm olayýna abone ol
        Enemy.onDead += OnEnemyDeath;

        TowerController.onGameLose += OnThrowStartingCallBack;
        EnemyTowerController.onGameWin += OnThrowStartingCallBack;
    }

    private void OnDestroy()
    {
        // Düþman ölüm olayýndan aboneliði iptal et
        Enemy.onDead -= OnEnemyDeath;

        TowerController.onGameLose -= OnThrowStartingCallBack;
        EnemyTowerController.onGameWin -= OnThrowStartingCallBack;
    }

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
        totalEnemiesSpawned = 0;
        totalEnemiesKilled = 0;
        aliveEnemiesCount = 0;
        allEnemiesSpawned = false;

        currentWave = waves[currentWaveIndex];

        // Wave'deki toplam düþman sayýsýný hesapla
        CalculateTotalEnemiesInWave();

        Debug.Log("Wave baþlatýldý. Toplam düþman sayýsý: " + totalEnemiesInWave);

        isTimerOn = true;
        SetupNextSegment();
        waveUI.waveSegmentText.text = "Wave " + (currentSegmentIndex + 1) + " / " + currentWave.segments.Count;
    }

    // Wave'deki toplam düþman sayýsýný hesapla
    private void CalculateTotalEnemiesInWave()
    {
        totalEnemiesInWave = 0;
        foreach (var segment in currentWave.segments)
        {
            foreach (var enemyManage in segment.segmentEnemys)
            {
                totalEnemiesInWave += enemyManage.enemyCount * enemyManage.enemy.Length;
            }
        }
    }

    private void ManageCurrentWave()
    {
        if (currentSegmentIndex >= currentWave.segments.Count)
        {
            isTimerOn = false;
            allEnemiesSpawned = true;
            Debug.Log("Tüm segmentler tamamlandý. Tüm düþmanlar spawn edildi. Yaþayan düþman sayýsý: " + aliveEnemiesCount);
            CheckForGameWin();
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
                currentSegmentIndex++;
                Debug.Log("Moving to next segment. Current Index: " + currentSegmentIndex);

                if (currentSegmentIndex >= currentWave.segments.Count)
                {
                    Debug.Log("All segments in the wave completed.");
                    isTimerOn = false;
                    allEnemiesSpawned = true;
                    CheckForGameWin();
                    return;
                }

                waveUI.waveSegmentText.text = "Wave " + (currentSegmentIndex + 1) + " / " + currentWave.segments.Count;

                isTimerOn = false;
                Invoke("StartNextSegment", segmentDelay);
            }
        }
    }

    private void StartNextSegment()
    {
        isTimerOn = true;
        timer = 0;
        Debug.Log("Starting next segment. Current Index: " + currentSegmentIndex);
        SetupNextSegment();
    }

    private void SetupNextSegment()
    {
        currentEnemyIndex = 0;
        currentEnemySubIndex = 0;
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
        if (currentEnemyCount <= 0)
        {
            currentEnemySubIndex++;
            if (currentEnemySubIndex < segment.segmentEnemys[currentEnemyIndex].enemy.Length)
            {
                currentEnemyCount = segment.segmentEnemys[currentEnemyIndex].enemyCount;
            }
            else
            {
                currentEnemySubIndex = 0;
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
        }

        // Dizi sýnýr kontrolü
        if (currentEnemyIndex >= segment.segmentEnemys.Length ||
            currentEnemySubIndex >= segment.segmentEnemys[currentEnemyIndex].enemy.Length)
        {
            Debug.LogError("Index out of range error.");
            return false;
        }

        int randomCreatPos = Random.Range(0, creatEnemyPosition.Length);
        GameObject enemyInstance = Instantiate(
            segment.segmentEnemys[currentEnemyIndex].enemy[currentEnemySubIndex],
            creatEnemyPosition[randomCreatPos].position,
            Quaternion.Euler(0f, 180f, 0f), enemyParent);

        Enemy enemy = enemyInstance.GetComponent<Enemy>();
        enemy.Initialize(segment.segmentEnemys[currentEnemyIndex].enemyLevel);

        // Spawn edilen düþman sayýsýný artýr
        totalEnemiesSpawned++;
        aliveEnemiesCount++; // Yaþayan düþman sayýsýný artýr
        Debug.Log("Enemy spawned. Total spawned: " + totalEnemiesSpawned + " / " + totalEnemiesInWave + " | Alive: " + aliveEnemiesCount);

        currentEnemyCount--;
        return true;
    }

    // Düþman öldüðünde çaðrýlacak method
    private void OnEnemyDeath(Vector2 transform)
    {
        totalEnemiesKilled++;
        aliveEnemiesCount--; // Yaþayan düþman sayýsýný azalt

        Debug.Log("Enemy killed. Total killed: " + totalEnemiesKilled + " / " + totalEnemiesInWave +
                  " | Alive: " + aliveEnemiesCount + " | All spawned: " + allEnemiesSpawned);

        // Kazanma kontrolü yap (her düþman öldüðünde)
        CheckForGameWin();
    }

    // Oyun kazanma kontrolü
    private void CheckForGameWin()
    {
        Debug.Log($"Win Check - All Spawned: {allEnemiesSpawned}, Alive Enemies: {aliveEnemiesCount}, Total Spawned: {totalEnemiesSpawned}, Total in Wave: {totalEnemiesInWave}");

        // Kazanma þartlarý:
        // 1. Tüm düþmanlar spawn edilmiþ olmalý (wave bitmiþ olmalý)
        // 2. Sahne içinde yaþayan düþman kalmamalý
        // 3. En az bir düþman spawn edilmiþ olmalý (boþ wave kontrolü)
        if (allEnemiesSpawned && aliveEnemiesCount <= 0 && totalEnemiesSpawned > 0)
        {
            Debug.Log("All enemies defeated! Game Win!");
            int waveIndex = PlayerPrefs.GetInt("WaveIndex", 0);
            waveIndex++;
            PlayerPrefs.SetInt("WaveIndex", waveIndex);
            onGameWin?.Invoke();
        }
    }

    public void OnThrowStartingCallBack()
    {
        onThrow = true;
        Time.timeScale = 1;
        Debug.Log("Avtipn çalýþtý" + onThrow);
    }

    public void OnThrowEndingCallBack()
    {
        onThrow = false;
        Debug.Log("Avtipn çalýþtý" + onThrow);
    }

    // Debug için yaþayan düþman sayýsýný kontrol etmek isteyebilirsiniz
    public int GetAliveEnemiesCount()
    {
        return aliveEnemiesCount;
    }

    public bool IsWaveCompletelyFinished()
    {
        return allEnemiesSpawned && aliveEnemiesCount <= 0;
    }
}

[Serializable]
public struct Wave
{
    public string waveName;
    public TowerSO waveTower;
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
    public EnemySO enemyLevel;
    public int enemyCount;
}