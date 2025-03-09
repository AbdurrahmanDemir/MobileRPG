using UnityEngine;

public class MeleeEnemy : Enemy
{    
    protected override void PerformSingleTargetAttack(GameObject target)
    {
        Debug.Log($"{gameObject.name} is attacking {target.name} with single target attack for {heroSO.damage} damage!");
        target.GetComponent<Hero>().HeroTakeDamage(enemySO.damage);
    }

    protected override void PerformAreaAttack()
    {
        // Alan hasarý uygulanmaz
    }
}
