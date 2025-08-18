using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SkeletonBulletPool : MonoBehaviour
{
    [SerializeField] private GameObject skeletonBulletPrefab;
    public ObjectPool<GameObject> skeletonBulletPool;

    private void Awake()
    {
        RangeEnemy.OnBulletRequested += SpawnBullet;
    }

    private void OnDestroy()
    {
        RangeEnemy.OnBulletRequested -= SpawnBullet;
    }

    private void Start()
    {
        skeletonBulletPool = new ObjectPool<GameObject>(
            CreateBullet,
            OnGet,
            OnRelease,
            OnDestroy
        );
    }

    private GameObject CreateBullet()
    {
        return Instantiate(skeletonBulletPrefab);
    }

    private void OnGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnRelease(GameObject obj)
    {
        obj.GetComponent<SkeletonBulletController>().ResetBullet();
        obj.transform.SetParent(null);
        obj.transform.position = Vector3.zero;
        obj.SetActive(false);
    }

    private void OnDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    private void SpawnBullet(BulletData data)
    {
        GameObject bulletInstance = skeletonBulletPool.Get();

        if (bulletInstance == null)
        {
            Debug.LogError("Skeleton bullet instance is null.");
            return;
        }

        if (bulletInstance.activeInHierarchy)
        {
            Debug.LogWarning("Skeleton bullet already active! Releasing and retrying.");
            skeletonBulletPool.Release(bulletInstance);
            bulletInstance = skeletonBulletPool.Get();
        }

        bulletInstance.transform.SetParent(data.firePoint);
        bulletInstance.transform.position = data.spawnPosition;

        var controller = bulletInstance.GetComponent<SkeletonBulletController>();
        if (controller == null)
        {
            Debug.LogError("SkeletonBulletController missing from prefab!");
            return;
        }

        controller.target = data.target;
        controller.targetPosition = data.target.transform.position;
        controller.enemySO = data.dataSO as EnemySO; 
        controller.pool = skeletonBulletPool;
    }

}
