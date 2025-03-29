using DG.Tweening;
using UnityEngine;

public class AngelBulletController : MonoBehaviour
{
    public HeroSO heroSO;
    public Vector2 targetPosition;

    private void Start()
    {
        DOTween.Sequence()
            .AppendInterval(4)
            .AppendCallback(() => gameObject.SetActive(false));
    }
    private void Update()
    {
        if(targetPosition!=null)
            MoveTowardsTarget(targetPosition);
        else
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().HeroTakeDamage(heroSO.damage);
            gameObject.SetActive(false);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().HeroTakeDamage(heroSO.damage);
            gameObject.SetActive(false);

        }
    }
    private void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, 1 * Time.deltaTime);
    }
}
