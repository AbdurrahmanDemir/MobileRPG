using UnityEngine;

public class AngelBulletController : MonoBehaviour
{
    public HeroSO heroSO;
    public Vector2 targetPosition;

    private void Update()
    {
        if(targetPosition!=null)
            MoveTowardsTarget(targetPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().HeroTakeDamage(heroSO.damage);
            Destroy(gameObject);
        }
    }
    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 1 * Time.deltaTime);
    }
}
