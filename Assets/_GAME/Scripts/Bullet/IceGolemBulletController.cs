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
        if (target == null || isReleased) // Eðer serbest býrakýlmýþsa hareket ettirme
        {
            ReleaseBullet();
            return;
        }

        if (targetPosition != null)
            MoveTowardsTarget(targetPosition);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().HeroTakeDamage(0);
            EnemyAttackSpeed(collision.gameObject);

            ReleaseBullet();


        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            collision.gameObject.GetComponent<Enemy>().HeroTakeDamage(0);
            EnemyAttackSpeed(collision.gameObject);
            ReleaseBullet();

        }
    }
    public void EnemyAttackSpeed(GameObject enemy)
    {
        if (!onDamage)
            StartCoroutine(DamageOn(enemy));
    }
    IEnumerator DamageOn(GameObject enemy)
    {
        onDamage = true;
        float cooldown = enemy.GetComponent<Enemy>().enemySO.cooldown;
        enemy.GetComponent<Enemy>().attackSpeed = cooldown + 2;
        yield return new WaitForSeconds(5f);
        onDamage = false;
        enemy.GetComponent<Enemy>().attackSpeed = cooldown;

    }
    private void ReleaseBullet()
    {
        if (isReleased) return;
        isReleased = true;
        Debug.Log("Bullet released: " + gameObject.name);
        bulletParticle.iceGolemBulletPool.Release(gameObject);
    }
    public void ResetBullet()
    {
        isReleased = false;
        target = null;
        targetPosition = Vector2.zero;
    }


    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 4 * Time.deltaTime);
    }
}
