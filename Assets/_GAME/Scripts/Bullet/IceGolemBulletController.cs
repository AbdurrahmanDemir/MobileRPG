using DG.Tweening;
using System.Collections;
using UnityEngine;

public class IceGolemBulletController : MonoBehaviour
{
    public HeroSO heroSO;
    public Vector2 targetPosition;
    public GameObject target;
    bool onDamage;


    BulletParticleManager bulletParticle;
    private bool isReleased = false;

    private void Awake()
    {
        bulletParticle = GameObject.FindGameObjectWithTag("ParticleManager").GetComponent<BulletParticleManager>();
    }
    private void Start()
    {
        DOTween.Sequence()
            .AppendInterval(1)
            .AppendCallback(() => bulletParticle.iceGolemBulletPool.Release(gameObject));
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
            collision.GetComponent<Enemy>().HeroTakeDamage(heroSO.GetCurrentDamage());
            StartCoroutine(ApplySlowAndRelease(collision.gameObject));
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        collision.gameObject.GetComponent<Enemy>().HeroTakeDamage(heroSO.GetCurrentDamage());
    //        StartCoroutine(EnemyAttackSpeed(collision.gameObject));
    //    }
    //    else if (collision.CompareTag("EnemyTower"))
    //    {
    //        collision.GetComponent<EnemyTowerController>().TakeDamage(heroSO.GetCurrentDamage());
    //        StartCoroutine(EnemyAttackSpeed(collision.gameObject));
    //    }
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            collision.gameObject.GetComponent<Enemy>().HeroTakeDamage(heroSO.GetCurrentDamage());
            StartCoroutine(EnemyAttackSpeed(collision.gameObject));

        }
        else if (collision.gameObject.CompareTag("EnemyTower"))
        {
            collision.gameObject.GetComponent<EnemyTowerController>().TakeDamage(heroSO.GetCurrentDamage());
            StartCoroutine(EnemyAttackSpeed(collision.gameObject));
        }
    }
    IEnumerator ApplySlowAndRelease(GameObject enemy)
    {
        if (onDamage) yield break;

        onDamage = true;

        // Mermiyi görünmez ve etkisiz hale getir
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        // Yavaþlatma uygula
        var enemyScript = enemy.GetComponent<Enemy>();
        float originalCooldown = enemyScript.enemySO.cooldown;
        float originalSpeed = enemyScript.enemySO.moveSpeed;

        enemyScript.cooldown = originalCooldown + 2f;
        enemyScript.moveSpeed = originalSpeed * 0.2f; // %80 yavaþlat

        // Burada yavaþlama süresi ayarlanabilir
        yield return new WaitForSeconds(0.1f);

        ReleaseBullet(); // Object pool’a geri gönder

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
        bulletParticle.iceGolemBulletPool.Release(gameObject);
    }
    //public void ResetBullet()
    //{
    //    isReleased = false;
    //    target = null;
    //    targetPosition = Vector2.zero;
    //}
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

    //IEnumerator DamageOn(GameObject enemy)
    //{
    //    onDamage = true;
    //    float cooldown = enemy.GetComponent<Enemy>().enemySO.cooldown;
    //    float moveSpeed = enemy.GetComponent<Enemy>().enemySO.moveSpeed;
    //    enemy.GetComponent<Enemy>().cooldown = cooldown + 2;
    //    enemy.GetComponent<Enemy>().moveSpeed = moveSpeed - (moveSpeed*(100/100));

    //    Debug.Log("Enemy cooldown:" + enemy.GetComponent<Enemy>().cooldown + "move speed: " + enemy.GetComponent<Enemy>().moveSpeed);

    //    yield return new WaitForSeconds(3f);
    //    onDamage = false;
    //    enemy.GetComponent<Enemy>().cooldown = cooldown;
    //    enemy.GetComponent<Enemy>().moveSpeed = moveSpeed;

    //    Debug.Log("Enemy cooldown:" + enemy.GetComponent<Enemy>().cooldown + "move speed: " + enemy.GetComponent<Enemy>().moveSpeed);


    //}
}
