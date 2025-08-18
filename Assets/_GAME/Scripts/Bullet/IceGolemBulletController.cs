using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class IceGolemBulletController : MonoBehaviour
{
    public HeroSO heroSO;
    public Vector2 targetPosition;
    public GameObject target;
    bool onDamage;


    [HideInInspector] public ObjectPool<GameObject> pool;
    private bool isReleased = false;

    private void Start()
    {
        DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() => ReleaseBullet());
    }
    private void Update()
    {
        if (target == null || isReleased)
        {
            ReleaseBullet();
            return;
        }

        if (targetPosition != null)
            MoveTowardsTarget(targetPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReleased) return;

        if (collision.CompareTag("Enemy"))
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.GetTeam() == TeamType.Enemy)
                    damageable.TakeDamage(heroSO.GetCurrentDamage());
            }
            StartCoroutine(ApplySlowAndRelease(collision.gameObject));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.GetTeam() == TeamType.Enemy)
                    damageable.TakeDamage(heroSO.GetCurrentDamage());
            }
            StartCoroutine(EnemyAttackSpeed(collision.gameObject));

        }
        else if (collision.gameObject.CompareTag("EnemyTower"))
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.GetTeam() == TeamType.Enemy)
                    damageable.TakeDamage(heroSO.GetCurrentDamage());
            }
            StartCoroutine(EnemyAttackSpeed(collision.gameObject));
        }
    }
    IEnumerator ApplySlowAndRelease(GameObject enemy)
    {
        if (enemy == null) yield break;

        var enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript == null) yield break;

        if (onDamage) yield break;
        onDamage = true;

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        float originalCooldown = enemyScript.enemySO.cooldown;
        float originalSpeed = enemyScript.enemySO.moveSpeed;

        enemyScript.cooldown = originalCooldown + 2f;
        enemyScript.moveSpeed = originalSpeed * 0.2f;

        yield return new WaitForSeconds(0.1f);

        ReleaseBullet();
        onDamage = false;
    }


    IEnumerator EnemyAttackSpeed(GameObject enemy)
    {
        if (!onDamage)
        {
            onDamage = true;

            float cooldown = enemy.GetComponent<Enemy>().enemySO.cooldown;
            float moveSpeed = enemy.GetComponent<Enemy>().enemySO.moveSpeed;
            enemy.GetComponent<Enemy>().cooldown = cooldown + 2;
            enemy.GetComponent<Enemy>().moveSpeed = moveSpeed - (moveSpeed * (100 / 100));

            yield return new WaitForSeconds(0.1f);

            ReleaseBullet();
            onDamage = false;
        }
    }
    private void ReleaseBullet()
    {
        if (isReleased) return;
        isReleased = true;
        Debug.Log("Bullet released: " + gameObject.name);
        pool?.Release(gameObject);
    }
    public void ResetBullet()
    {
        isReleased = false;
        onDamage = false;
        target = null;
        targetPosition = Vector2.zero;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }


    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 5 * Time.deltaTime);
    }

}
