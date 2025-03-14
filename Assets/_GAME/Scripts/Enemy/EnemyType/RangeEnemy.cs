using TMPro;
using UnityEngine;

public class RangeEnemy : Enemy
{
    [Header("Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletTransform;

    private void Start()
    {
        bulletTransform = GetComponentInChildren<Transform>();
    }
    protected override void PerformAreaAttack(GameObject target)
    {
        GameObject bulletInstance = Instantiate(bullet, bulletTransform);
        bulletInstance.transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 2 * Time.deltaTime);
        //bulletInstance.GetComponent<AngelBulletController>().heroSO =
    }

    protected override void PerformSingleTargetAttack(GameObject target)
    {
    }
}
