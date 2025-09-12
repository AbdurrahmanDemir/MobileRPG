using NavMeshPlus.Components;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyBaseManager : MonoBehaviour
{
    public static EnemyBaseManager instance;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelMapManager levelMapManager; // LevelMapManager referansý eklendi

    [Header("Addressable Level References")]
    public AssetReference[] levelAssetReferences;

    [Header("Loading UI")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private UnityEngine.UI.Slider loadingSlider;

    [Header("Assign")]
    public Transform levelSpawnRoot;

    private GameObject currentLevelObj;
    private AsyncOperationHandle<GameObject> currentLevelHandle;
    public int aliveCount = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CheckAssetReferences();

        // LevelMapManager referansýný bul
        if (levelMapManager == null)
            levelMapManager = FindObjectOfType<LevelMapManager>();
    }

    private void CheckAssetReferences()
    {
        Debug.Log($"Toplam AssetReference sayýsý: {levelAssetReferences.Length}");

        for (int i = 0; i < levelAssetReferences.Length; i++)
        {
            if (levelAssetReferences[i] == null)
            {
                Debug.LogError($"AssetReference {i} NULL!");
            }
            else if (!levelAssetReferences[i].RuntimeKeyIsValid())
            {
                Debug.LogError($"AssetReference {i} RuntimeKey geçersiz!");
            }
            else
            {
                Debug.Log($"AssetReference {i} OK: {levelAssetReferences[i].AssetGUID}");
            }
        }
    }

    public async void LoadLevel(int index)
    {
        if (index < 0 || index >= levelAssetReferences.Length)
        {
            Debug.LogError($"Geçersiz level index: {index}. Toplam level sayýsý: {levelAssetReferences.Length}");
            return;
        }

        if (levelAssetReferences[index] == null)
        {
            Debug.LogError($"Level AssetReference null: {index}");
            return;
        }

        Debug.Log($"Level yükleniyor: {index}");

        await UnloadCurrentLevel();

        try
        {
            loadingPanel.SetActive(true);
            loadingSlider.value = 0f;

            currentLevelHandle = levelAssetReferences[index].LoadAssetAsync<GameObject>();

            while (!currentLevelHandle.IsDone)
            {
                loadingSlider.value = currentLevelHandle.PercentComplete;
                await System.Threading.Tasks.Task.Yield();
            }

            GameObject levelPrefab = await currentLevelHandle.Task;

            if (currentLevelHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Level prefab baþarýyla yüklendi: {levelPrefab.name}");

                currentLevelObj = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity, levelSpawnRoot);

                if (currentLevelObj == null)
                {
                    Debug.LogError("Level objesi oluþturulamadý!");
                    loadingPanel.SetActive(false);
                    return;
                }

                Debug.Log($"Level objesi oluþturuldu: {currentLevelObj.name}");

                var navMeshSurface = currentLevelObj.GetComponentInChildren<NavMeshSurface>();
                if (navMeshSurface != null)
                {
                    Debug.Log("NavMeshSurface bulundu, build ediliyor...");
                    navMeshSurface.BuildNavMesh();
                }
                else
                {
                    Debug.LogWarning("NavMeshSurface bulunamadý!");
                }

                StartCoroutine(SpawnEnemiesSafely());
            }
            else
            {
                Debug.LogError($"Level yükleme baþarýsýz: {currentLevelHandle.Status}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Level yüklenirken hata: {e.Message}");
        }
        finally
        {
            loadingPanel.SetActive(false);
        }
    }

    private IEnumerator SpawnEnemiesSafely()
    {
        yield return null;

        NavMeshAgent[] agents = currentLevelObj.GetComponentsInChildren<NavMeshAgent>(true);
        Debug.Log($"Bulunan NavMeshAgent sayýsý: {agents.Length}");

        foreach (var agent in agents)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(agent.transform.position, out hit, 1f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
                Debug.Log($"Agent spawn edildi: {agent.name}");
            }
            else
            {
                Debug.LogWarning("NavMeshAgent spawn pozisyonu NavMesh üzerinde deðil: " + agent.name);
            }
        }

        Debug.Log($"Toplam alive count: {aliveCount}");
    }

    public async System.Threading.Tasks.Task UnloadCurrentLevel()
    {
        if (currentLevelObj != null)
        {
            Debug.Log("Mevcut level destroy ediliyor...");
            Destroy(currentLevelObj);
            currentLevelObj = null;
        }

        if (currentLevelHandle.IsValid())
        {
            Addressables.Release(currentLevelHandle);
        }

        await System.Threading.Tasks.Task.Yield();
    }

    public void RegisterObject(string name)
    {
        aliveCount++;
        Debug.Log($"Object registered. Alive count: {aliveCount} - {name}");
    }

    public void UnRegisterObject()
    {
        aliveCount--;
        Debug.Log($"Object unregistered. Alive count: {aliveCount}");

        if (aliveCount <= 0)
        {
            Debug.Log("Tüm düþmanlar öldü, oyun kazanýldý!");

            // Hangi episode ve level'da olduðumuzu al
            int playingEpisode = PlayerPrefs.GetInt("PlayingEpisode", 0);
            int playingLevel = PlayerPrefs.GetInt("PlayingLevel", 0);

            Debug.Log($"Tamamlanan Episode: {playingEpisode}, Level: {playingLevel}");

            // Bu episode için current level'i güncelle
            int newLevel = playingLevel + 2; // Bir sonraki level'i aç (index 0'dan baþladýðý için +2)
            levelMapManager.SetCurrentLevelForEpisode(playingEpisode, newLevel);

            // Episode'daki total level sayýsýný al
            int totalLevelsInEpisode = GetTotalLevelsInEpisode(playingEpisode);

            // Eðer episode'un son level'iyse 
            if (playingLevel >= totalLevelsInEpisode - 1) // Son level index = totalLevel - 1
            {
                Debug.Log($"Episode {playingEpisode} tamamlandý! Yeni episode açýlýyor...");

                // Yeni episode'u aç
                int newEpisodeIndex = playingEpisode + 1;
                PlayerPrefs.SetInt("LevelEpisodeIndex", newEpisodeIndex);

                // Yeni episode'un ilk levelini aç (level 1 = index 0, bu yüzden 1 kaydedeceðiz)
                levelMapManager.SetCurrentLevelForEpisode(newEpisodeIndex, 1);

                PlayerPrefs.Save();

                Debug.Log($"Yeni episode açýldý: {newEpisodeIndex}");
            }

            uiManager.GameWinPanel();
        }
    }

    // Episode'daki toplam level sayýsýný döndürür
    private int GetTotalLevelsInEpisode(int episodeIndex)
    {
        // LevelMapManager'dan episode bilgilerini al
        // Bu bilgiyi public bir fonksiyon ile alabilirsiniz
        if (levelMapManager != null)
        {
            return levelMapManager.GetEpisodeLevelCount(episodeIndex);
        }

        // Fallback: sabit sayý (her episode 6 level varsa)
        return 6;
    }

    private void OnDestroy()
    {
        if (currentLevelHandle.IsValid())
        {
            Addressables.Release(currentLevelHandle);
        }
    }
}