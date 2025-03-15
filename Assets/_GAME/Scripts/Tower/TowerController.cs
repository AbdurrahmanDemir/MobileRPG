using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerController : MonoBehaviour
{

    [Header("Settings")]
    public TowerSO towerSO;
    int health;


    [Header("Elements")]
    private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    SpriteRenderer towerSpriteRenderer;
    private Color originalColor;
    private Vector2 originalScale;
    public Vector2 scaleReduction = new Vector3(0.9f, 0.9f, 1f);

    private void Start()
    {
        towerSpriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = towerSpriteRenderer.color;
        originalScale = transform.localScale;

        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.maxValue = towerSO.maxHealth;
        health = towerSO.maxHealth;
        healthSlider.value = health;
        healthText.text=health.ToString();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;
        healthText.text = health.ToString();


        towerSpriteRenderer.DOColor(Color.gray, 0.1f).OnComplete(() =>
        {
            towerSpriteRenderer.DOColor(originalColor, 0.1f).SetDelay(0.1f);
        });
        transform.DOScale(originalScale * scaleReduction, 0.1f).OnComplete(() =>
        {
            transform.DOScale(originalScale, 0.1f);
        });


        if (health <= 0)
        {
            Debug.Log("OYUNU KAYBETT�N");
        }
    }
}
