using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class BulletParticleManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject skeletonBulletPrefabs;
    [SerializeField] private GameObject angelBulletPrefabs;

    [Header("Pooling")]
    private ObjectPool<GameObject> skeletonBulletPool;
    private ObjectPool<GameObject> angelBulletPool;

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
                                                    ActionOnRelease,
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
        particle.SetActive(true);
    }
    private void ActionOnRelease(GameObject particle)
    {
        particle.SetActive(false);
    }
    private void ActionOnDestroy(GameObject particle)
    {
        Destroy(particle);
    }

    private void EnemyBulletParticleCallBack(Vector2 createPosition, GameObject target, EnemySO enemySO,Transform bulletTransform)
    {
        GameObject bulletInstance = skeletonBulletPool.Get();

        bulletInstance.transform.SetParent(bulletTransform);

        bulletInstance.transform.position = createPosition;

        bulletInstance.GetComponent<SkeletonBulletController>().targetPosition = target.transform.position;
        bulletInstance.GetComponent<SkeletonBulletController>().enemySO = enemySO;

        //DOTween.Sequence()
        //    .AppendInterval(4)
        //    .AppendCallback(() => skeletonBulletPool.Release(bulletInstance));
    }
    private void AngelBulletParticleCallBack(Vector2 createPosition, GameObject target, HeroSO heroSO, Transform bulletTransform)
    {
        GameObject bulletInstance = angelBulletPool.Get();

        bulletInstance.transform.SetParent(bulletTransform);

        bulletInstance.transform.position = createPosition;

        bulletInstance.GetComponent<AngelBulletController>().targetPosition = target.transform.position;
        bulletInstance.GetComponent<AngelBulletController>().heroSO = heroSO;

    }
}
