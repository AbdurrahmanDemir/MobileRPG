using UnityEngine;

public class SkeletonBulletController : MonoBehaviour
{
    public EnemySO enemySO;
    public Vector2 targetPosition;

    private void Update()
    {
        if (targetPosition != null)
            MoveTowardsTarget(targetPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hero"))
        {
            collision.GetComponent<Hero>().HeroTakeDamage(enemySO.damage);
        }
        else if (collision.CompareTag("Tower"))
        {
            collision.GetComponent<TowerController>().TakeDamage(enemySO.damage);
        }
    }
    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 1 * Time.deltaTime);
    }
}
