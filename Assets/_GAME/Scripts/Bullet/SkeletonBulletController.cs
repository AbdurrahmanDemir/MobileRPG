using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class SkeletonBulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    public EnemySO enemySO;
    public GameObject target;
    public Vector2 targetPosition;
    public float moveSpeed = 4f;

    [HideInInspector] public ObjectPool<GameObject> pool;

    private bool isReleased = false;
    private Tween autoReleaseTween;

    private void OnEnable()
    {
        StartAutoReleaseTween();
    }

    private void OnDisable()
    {
        autoReleaseTween?.Kill();
    }

    private void Update()
    {
        if (isReleased || target == null || !target.activeInHierarchy)
        {
            SafeRelease();
            return;
        }

        MoveTowardsTarget(targetPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void HandleCollision(GameObject hitObj)
    {
        if (isReleased || hitObj == null) return;

        if (hitObj.CompareTag("Hero"))
        {
            if (hitObj.TryGetComponent<IDamageable>(out var damageable) && damageable.GetTeam() == TeamType.Hero)
            {
                damageable.TakeDamage(enemySO.damage);
            }
            SafeRelease();
        }
        else if (hitObj.CompareTag("Tower"))
        {
            if (hitObj.TryGetComponent<TowerController>(out var tower))
            {
                tower.TakeDamage(enemySO.damage);
            }
            SafeRelease();
        }
    }

    private void MoveTowardsTarget(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void SafeRelease()
    {
        if (isReleased) return;

        isReleased = true;

        autoReleaseTween?.Kill();
        pool?.Release(gameObject);
    }

    private void StartAutoReleaseTween()
    {
        autoReleaseTween?.Kill();
        autoReleaseTween = DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() => SafeRelease());
    }

    public void ResetBullet()
    {
        isReleased = false;
        target = null;
        targetPosition = Vector2.zero;
        transform.position = Vector3.zero;

        StartAutoReleaseTween();
    }
}
