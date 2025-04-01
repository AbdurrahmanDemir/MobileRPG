using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class BulletParticleManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject skeletonBulletPrefabs;
    [SerializeField] private GameObject angelBulletPrefabs;

    [Header("Pooling")]
    public ObjectPool<GameObject> skeletonBulletPool;
    public ObjectPool<GameObject> angelBulletPool;

    private void Awake()
    {
        RangeEnemy.onEnemyBulletInstante += EnemyBulletParticleCallBack;
        RangeHero.onAngelBulletInstante+= AngelBulletParticleCallBack;
    }
    private void OnDestroy()
    {
        RangeEnemy.onEnemyBulletInstante -= EnemyBulletParticleCallBack;
        RangeHero.onAngelBulletInstante -= AngelBulletParticleCallBack;
    }


    private void Start()
    {
        skeletonBulletPool = new ObjectPool<GameObject>(CreateFunction,
                                                      ActionOnGet,
                                                      ActionOnRelease,
                                                      ActionOnDestroy);

        angelBulletPool= new ObjectPool<GameObject>(CreateAngelBulletFunction,
                                                    ActionOnGet,
                                                    ActionOnAngelRelease,
                                                    ActionOnDestroy);
    }

    private GameObject CreateFunction()
    {
        return Instantiate(skeletonBulletPrefabs) as GameObject;
    }
    private GameObject CreateAngelBulletFunction()
    {
        return Instantiate(angelBulletPrefabs) as GameObject;
    }
    private void ActionOnGet(GameObject particle)
    {
        Debug.Log("Bullet retrieved from pool: " + particle.name);
        particle.SetActive(true);
    }

    private void ActionOnRelease(GameObject particle)
    {
        particle.transform.SetParent(null);  // Ebeveyni sýfýrla
        particle.transform.position = Vector3.zero;  // Konumu sýfýrla
        particle.GetComponent<SkeletonBulletController>().ResetBullet();  // Bullet kontrolcüsünü sýfýrla
        particle.SetActive(false);
    }
    private void ActionOnAngelRelease(GameObject particle)
    {
        particle.transform.SetParent(null);  // Ebeveyni sýfýrla
        particle.transform.position = Vector3.zero;  // Konumu sýfýrla
        particle.GetComponent<AngelBulletController>().ResetBullet();  // Bullet kontrolcüsünü sýfýrla
        particle.SetActive(false);
    }


    private void ActionOnDestroy(GameObject particle)
    {
        Destroy(particle);
    }

    private void EnemyBulletParticleCallBack(Vector2 createPosition, GameObject target, EnemySO enemySO, Transform bulletTransform)
    {
        GameObject bulletInstance = skeletonBulletPool.Get();

        if (bulletInstance == null)
        {
            Debug.Log("Bullet instance is null.");
            return;
        }

        if (bulletInstance.activeInHierarchy)
        {
            Debug.LogWarning("Bullet already active! Releasing and re-getting...");
            skeletonBulletPool.Release(bulletInstance);
            bulletInstance = skeletonBulletPool.Get();
        }

        bulletInstance.transform.SetParent(bulletTransform);
        bulletInstance.transform.position = createPosition;

        var bulletController = bulletInstance.GetComponent<SkeletonBulletController>();
        if (bulletController == null)
        {
            Debug.Log("SkeletonBulletController bulunamadý! Prefab'de eksik olabilir.");
            return;
        }
        bulletController.target = target;
        bulletController.targetPosition = target.transform.position;
        bulletController.enemySO = enemySO;
    }


    private void AngelBulletParticleCallBack(Vector2 createPosition, GameObject target, HeroSO heroSO, Transform bulletTransform)
    {
        GameObject bulletInstance = angelBulletPool.Get();

        if (bulletInstance == null)
        {
            Debug.Log("Bullet instance is null.");
            return;
        }
        if (bulletInstance.activeInHierarchy)
        {
            Debug.LogWarning("Bullet already active! Releasing and re-getting...");
            angelBulletPool.Release(bulletInstance);
            bulletInstance = angelBulletPool.Get();
        }

        bulletInstance.transform.SetParent(bulletTransform);
        bulletInstance.transform.position = createPosition;

        var bulletController = bulletInstance.GetComponent<AngelBulletController>();
        if (bulletController == null)
        {
            Debug.Log("AngelBulletController bulunamadý! Prefab'de eksik olabilir.");
            return;
        }
        bulletController.target = target;
        bulletController.targetPosition = target.transform.position;
        bulletController.heroSO = heroSO;

    }
}
