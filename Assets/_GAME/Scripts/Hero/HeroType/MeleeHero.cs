using UnityEngine;

public class MeleeHero : Hero
{
    protected override void PerformSingleTargetAttack(GameObject target)
    {
        Debug.Log($"{gameObject.name} is attacking {target.name} with single target attack for {heroSO.damage} damage!");
        // Tek hedefe saldýrýyý uygulayýn
    }

    protected override void PerformAreaAttack()
    {
        // Alan hasarý uygulanmaz
    }
}
