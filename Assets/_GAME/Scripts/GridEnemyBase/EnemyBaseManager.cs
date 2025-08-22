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
        // AssetReference durumlarýný kontrol et
        CheckAssetReferences();
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

        // Bir frame bekle
        await System.Threading.Tasks.Task.Yield();
    }

    public void RegisterObject(string name)
    {
        aliveCount++;
        Debug.Log($"Object registered. Alive count: {aliveCount}" + name);
    }

    public void UnRegisterObject()
    {
        aliveCount--;
        Debug.Log($"Object unregistered. Alive count: {aliveCount}");

        if (aliveCount <= 0)
        {
            Debug.Log("Tüm düþmanlar öldü, oyun kazanýldý!");
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            currentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            uiManager.GameWinPanel();
        }
    }

    private void OnDestroy()
    {
        if (currentLevelHandle.IsValid())
        {
            Addressables.Release(currentLevelHandle);
        }
    }
}