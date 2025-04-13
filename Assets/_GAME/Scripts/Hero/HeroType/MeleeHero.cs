using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeHero : Hero
{
    protected override void PerformSingleTargetAttack(GameObject target)
    {
        Debug.Log($"{gameObject.name} is attacking {target.name} with single target attack for {heroSO.damage} damage!");
        if (target.CompareTag("Enemy"))
            target.GetComponent<Enemy>().HeroTakeDamage(heroSO.damage);
        else if (target.CompareTag("EnemyTower"))
            target.GetComponent<EnemyTowerController>().TakeDamage(heroSO.damage);
    }

    protected override void PerformAreaAttack()
    {
        // Alan hasarý uygulanmaz
    }
}
