using DG.Tweening;
using System;
using UnityEngine;

public class HookedHero : MonoBehaviour
{
    private HeroType type;

    private CircleCollider2D coll;

    private SpriteRenderer rend;

    private float screenLeft;

    private Tweener tweener;

    public HeroType Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            coll.radius = type.collierRadius;
            rend.sprite = type.sprite;
        }
    }

    void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }

    public void ResetHero()
    {
        if (tweener != null)
            tweener.Kill(false);

        float num = UnityEngine.Random.Range(type.minLenght, type.maxLenght);
        coll.enabled = true;

        Vector3 position = transform.position;
        position.y = num;
        position.x = screenLeft;
        transform.position = position;

        float num2 = 1;
        float y = UnityEngine.Random.Range(num - num2, num + num2);
        Vector2 v = new Vector2(-position.x, y);

        float num3 = 3;
        float delay = UnityEngine.Random.Range(0, 2 * num3);
        tweener = transform.DOMove(v, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        });

    }

    public string GetHeroName()
    {
        return type.name;
    }

    public void Hooked()
    {
        coll.enabled = false;
        tweener.Kill(false);
    }

    [Serializable]
    public class HeroType
    {
        public string name;

        public int price;

        public float heroCount;

        public float minLenght;

        public float maxLenght;

        public float collierRadius;

        public Sprite sprite;
    }
}
