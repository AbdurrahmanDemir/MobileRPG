using DG.Tweening;
using UnityEngine;

public class SkeletonBulletController : MonoBehaviour
{
    public EnemySO enemySO;
    public Vector2 targetPosition;

    BulletParticleManager bulletParticle;

    private void Start()
    {
        DOTween.Sequence()
            .AppendInterval(4)
            .AppendCallback(() => gameObject.SetActive(false));
    }

    private void Update()
    {
        if (targetPosition != null)
            MoveTowardsTarget(targetPosition);
        else
            gameObject.SetActive(false);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hero"))
        {
            collision.GetComponent<Hero>().HeroTakeDamage(enemySO.damage);
            gameObject.SetActive(false);

        }
        else if (collision.CompareTag("Tower"))
        {
            collision.GetComponent<TowerController>().TakeDamage(enemySO.damage);
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hero"))
        {
            collision.gameObject.GetComponent<Hero>().HeroTakeDamage(enemySO.damage);
            gameObject.SetActive(false);

        }
        else if (collision.gameObject.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<TowerController>().TakeDamage(enemySO.damage);
            gameObject.SetActive(false);

        }
    }
    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 1 * Time.deltaTime);
    }
}
