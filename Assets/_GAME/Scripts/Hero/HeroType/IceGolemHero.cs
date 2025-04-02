using System;
using System.Collections;
using UnityEngine;

public class IceGolemHero : Hero
{
    [Header("Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletTransform;

    [Header("Action")]
    public static Action<Vector2, GameObject, HeroSO, Transform> onIceGolemBulletInstante;

    bool onDamage;
    protected override void PerformSingleTargetAttack(GameObject target)
    {        
        onIceGolemBulletInstante?.Invoke(bulletTransform.position, target, heroSO, bulletTransform);

    }

    protected override void PerformAreaAttack()
    {
        // Alan hasarý uygulanmaz
    }
}
