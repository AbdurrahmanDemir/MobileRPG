using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class Hook : MonoBehaviour
{
    public Transform hookedTransform;

    private Camera mainCamera;
    private Collider2D coll;

    private int length;
    private int strength;
    private int heroCount;

    private bool canMove=true;

    private List<HookedHero> hookedHero;

    private Tweener cameraTween;

    private void Awake()
    {
        mainCamera = Camera.main;
        coll= GetComponent<Collider2D>();
        hookedHero = new List<HookedHero>();
    }

    private void Update()
    {
        if (canMove && Input.GetMouseButton(0))
        {
            Vector3 camPos= mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 hookPosition = transform.position;
            hookPosition.x = camPos.x;
            transform.position = hookPosition;
        }
    }

    public void StartThrow()
    {
        length = -50;
        strength = 3;
        heroCount = 0;
        float time = (-length) * 0.1f;

        cameraTween = mainCamera.transform.DOMoveY(length, 1 * time * 0.25f, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y <= -11)
            {
                transform.SetParent(mainCamera.transform);
            }
        }).OnComplete(delegate
        {
            coll.enabled = true;
            cameraTween = mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
            {
                if (mainCamera.transform.position.y >= -25f)
                    StopThrow();
            });
        });
        //Screen(GAME)

        coll.enabled = false;
        canMove = true;
        hookedHero.Clear();
    }

    public void StopThrow()
    {
        canMove = false;
        cameraTween.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            coll.enabled = true;
            int num = 0;
            for (int i = 0; i < hookedHero.Count; i++)
            {
                hookedHero[i].transform.SetParent(null);
                hookedHero[i].ResetHero();
                num += hookedHero[i].Type.price;
            }

            //IdleManager.insance.totalGain = num;
            //SceenEnd
            Debug.Log("BITTI");
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HookedHero") && heroCount != strength)
        {
            heroCount++;
            HookedHero component = collision.GetComponent<HookedHero>();
            component.Hooked();
            hookedHero.Add(component);
            collision.transform.SetParent(transform);
            collision.transform.position = hookedTransform.position;
            collision.transform.rotation = hookedTransform.rotation;
            collision.transform.localScale = Vector3.one;

            collision.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                collision.transform.rotation = Quaternion.identity;
            });
            if (heroCount == strength)
                StopThrow();
        }
    }
}
