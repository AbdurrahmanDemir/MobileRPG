using UnityEngine;

public class RangeHero : Hero
{
    [Header("Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletTransform;

    protected override void PerformAreaAttack(GameObject target)
    {
       
    }

    protected override void PerformSingleTargetAttack(GameObject target)
    {
        GameObject bulletInstance = Instantiate(bullet, bulletTransform);
        bulletInstance.transform.position = bulletTransform.position;
        bulletInstance.GetComponent<AngelBulletController>().targetPosition=target.transform.position;
        bulletInstance.GetComponent<AngelBulletController>().heroSO = heroSO;
    }
}
