using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeAreaHero : Hero
{
    protected override void PerformSingleTargetAttack(GameObject target)
    {
    }

    protected override void PerformAreaAttack()
    {

        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, heroSO.range, targetLayerMask);
        foreach (var targetx in targetsInRange)
        {
            Debug.Log($"{gameObject.name} is attacking {targetx.gameObject.name} with area of effect attack for {heroSO.damage} damage!");
            targetx.GetComponent<Enemy>().HeroTakeDamage(heroSO.damage);
        }
    }
}
