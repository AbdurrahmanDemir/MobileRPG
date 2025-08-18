using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public TeamType GetTeam() => TeamType.Enemy;


    [Header("Settings")]
    public EnemySO enemySO;
    protected float lastAttackTime = 0f;
    public LayerMask targetLayerMask;

    public string enemyName;
    public Sprite enemyImage;
    public string attackType;
    public int damage;
    public float range;
    public float moveSpeed;
    public int health;
    public float cooldown;

    [Header("AI Settings")]
    public float detectionRange = 5f;

    private GameObject currentTarget;

    [Header("Elements")]
    public Animator animator;
    private Slider healthSlider;
    SpriteRenderer characterSpriteRenderer;
    private Color originalColor;
    private Vector2 originalScale;
    public Vector2 scaleReduction = new Vector3(0.9f, 0.9f, 1f);

    [Header("Action")]
    private bool onThrow=false;
    public static Action<Vector2> onDead;
    public static Action OnAnyEnemyHealthChanged;


    private void Awake()
    {

        UpgradeSelectManager.onPowerUpPanelOpened += OnThrowStartingCallBack;
        UpgradeSelectManager.onPowerUpPanelClosed += OnThrowEndingCallBack;

        //TowerController.onGameLose += OnThrowStartingCallBack;
        //EnemyTowerController.onGameWin += OnThrowStartingCallBack;

    }
    private void OnDestroy()
    {
        UpgradeSelectManager.onPowerUpPanelOpened -= OnThrowStartingCallBack;
        UpgradeSelectManager.onPowerUpPanelClosed -= OnThrowEndingCallBack;

        //TowerController.onGameLose -= OnThrowStartingCallBack;
        //EnemyTowerController.onGameWin -= OnThrowStartingCallBack;

    }


    private void Start()
    {
        animator = GetComponent<Animator>();

        characterSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = characterSpriteRenderer.color;
        originalScale = transform.localScale;

        Initialize(enemySO);

        //enemyName = enemySO.enemyName;
        //enemyImage = enemySO.enemyImage;
        //attackType = enemySO.attackType;
        //damage = enemySO.GetEnemyDamage();
        //range = enemySO.range;
        //moveSpeed = enemySO.moveSpeed;
        //cooldown = enemySO.cooldown;


        //healthSlider = GetComponentInChildren<Slider>();
        //healthSlider.maxValue = enemySO.maxHealth;
        //health = enemySO.GetEnemyHealth();
        //healthSlider.value = health;

        //cooldown = enemySO.cooldown;

        if (SceneManager.GetActiveScene().name=="PixelGane")
            EnemyBaseManager.instance.RegisterObject();

    }
    public void Initialize(EnemySO so)
    {
        enemySO = so;

        enemyName = enemySO.enemyName;
        enemyImage = enemySO.enemyImage;
        attackType = enemySO.attackType;
        damage = enemySO.GetEnemyDamage();
        range = enemySO.range;
        moveSpeed = enemySO.moveSpeed;
        cooldown = enemySO.cooldown;

        health = enemySO.GetEnemyHealth();

        if (healthSlider == null)
            healthSlider = GetComponentInChildren<Slider>();

        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    //void Update()
    //{
    //    GameObject target = FindClosestTarget();

    //    if (target != null)
    //    {
    //        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

    //        if (onThrow)
    //            return;

    //        if (distanceToTarget <= range)
    //        {
    //            if (target == null)
    //                Debug.Log("Hero olmustu");
    //            else
    //                Attack(target);
    //        }
    //        else
    //        {
    //            MoveTowardsTarget(target.transform.position);
    //            animator.Play("run");
    //        }
    //    }
    //}
    void Update()
    {
        if (onThrow)
            return;

        if (currentTarget == null)
        {
            currentTarget = FindTargetInDetectionRange();
        }

        if (currentTarget != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, currentTarget.transform.position);

            if (distanceToTarget > detectionRange * 1.5f)  
            {
                currentTarget = null;
                animator.Play("idle");
                return;
            }

            if (distanceToTarget <= range)
            {
                Attack(currentTarget);
            }
            else
            {
                MoveTowardsTarget(currentTarget.transform.position);
                animator.Play("run");
            }
        }
        else
        {
            animator.Play("idle");
        }
    }

    private GameObject FindTargetInDetectionRange()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, detectionRange, targetLayerMask);
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (var target in targets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.gameObject;
            }
        }
        return closestTarget;
    }

    protected virtual void Attack(GameObject target)
    {
        if(Time.time- lastAttackTime>= cooldown)
        {
            lastAttackTime = Time.time;

            if (enemySO.isAreaOfEffect)
            {
                PerformAreaAttack();
            }
            else
            {
                PerformSingleTargetAttack(target);
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
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;
        OnAnyEnemyHealthChanged?.Invoke();
        characterSpriteRenderer.DOColor(Color.gray, 0.1f).OnComplete(() =>
        {
            characterSpriteRenderer.DOColor(originalColor, 0.1f).SetDelay(0.1f);
        });
        transform.DOScale(originalScale * scaleReduction, 0.1f).OnComplete(() =>
        {
            transform.DOScale(originalScale, 0.1f);
        });


        if (health <= 0)
        {
            if (SceneManager.GetActiveScene().name == "PixelGane")
                EnemyBaseManager.instance.UnRegisterObject();

            Debug.Log("enemy öldü");
            onDead?.Invoke(transform.position);
            GameManager.enemyCount++;
            Debug.Log("ENEMY COUNT: " + GameManager.enemyCount);
            Destroy(gameObject);
        }
    }

    public void OnThrowStartingCallBack()
    {
        onThrow = true;
    }
    public void OnThrowEndingCallBack()
    {
        onThrow = false;
    }

    public int GetCurrentHealth()
    {
        return health;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
#endif

}
