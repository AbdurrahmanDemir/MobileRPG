using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class SkeletonBulletController : MonoBehaviour
{
    public EnemySO enemySO;
    public Vector2 targetPosition;
    public GameObject target;


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
        if (collision.CompareTag("Hero"))
        {
            if (collision.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.GetTeam() == TeamType.Hero)
                    damageable.TakeDamage(enemySO.damage);
            }

            ReleaseBullet();

        }
        else if (collision.CompareTag("Tower"))
        {
            collision.GetComponent<TowerController>().TakeDamage(enemySO.damage);
            ReleaseBullet();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                if (damageable.GetTeam() == TeamType.Hero)
                    damageable.TakeDamage(enemySO.damage);
            }

            ReleaseBullet();

        }
        else if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<TowerController>().TakeDamage(enemySO.damage);
            ReleaseBullet();

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
        target = null;
        targetPosition = Vector2.zero;
        transform.position = Vector3.zero;

    }


    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 4 * Time.deltaTime);
    }
}
