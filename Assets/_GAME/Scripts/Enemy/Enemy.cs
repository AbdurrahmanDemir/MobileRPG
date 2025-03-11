using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Settings")]
    public EnemySO enemySO;
    protected float lastAttackTime = 0f;
    public LayerMask targetLayerMask;
    int health;

    [Header("Elements")]
    private Animator animator;
    private Slider healthSlider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.maxValue = enemySO.maxHealth;
        health = enemySO.maxHealth;
        healthSlider.value = health;
    }
    void Update()
    {
        GameObject target = FindClosestTarget();

        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

            if (distanceToTarget <= enemySO.range)
            {
                Attack(target);
            }
            else
            {
                MoveTowardsTarget(target.transform.position);
                animator.Play("run");
            }
        }
    }

    protected virtual void Attack(GameObject target)
    {
        if(Time.time- lastAttackTime>= enemySO.cooldown)
        {
            lastAttackTime = Time.time;

            if (enemySO.isAreaOfEffect)
            {
                PerformAreaAttack();
            }
            else
            {
                PerformSingleTargetAttack(target);
                animator.Play("attack");
            }
        }
    }

    protected abstract void PerformSingleTargetAttack(GameObject target);
    protected abstract void PerformAreaAttack();
    protected GameObject FindClosestTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, 100, targetLayerMask);
        foreach (var target in potentialTargets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.gameObject;
            }
        }
        return closestTarget;
    }
    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemySO.moveSpeed * Time.deltaTime);
    }

    public virtual void HeroTakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        if (health <= 0)
        {
            Debug.Log("enemy öldü");
            Destroy(gameObject);
        }
    }
}
