using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuHeroDetails : MonoBehaviour
{
    [Header("Card")]
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Image cardIconImage;
    [SerializeField] private GameObject[] cardTypes;
    [SerializeField] private TextMeshProUGUI cardText;
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [Header("Other")]
    [SerializeField] TextMeshProUGUI upgradePriceText;

    public void Config(string name, Sprite icon, string type, string cardDetails,
                       float damage, float health, float range, float attackSpeed, float moveSpeed,
                       int price)
    {
        cardNameText.text = name;
        cardIconImage.sprite = icon;
        cardText.text = cardDetails;
        damageText.text = damage.ToString();
        healthText.text = health.ToString();
        rangeText.text = range.ToString();
        attackSpeedText.text = attackSpeed.ToString();
        moveSpeedText.text = moveSpeed.ToString();
        upgradePriceText.text = price.ToString();

        switch (type)
        {
            case "Cammon":
                cardTypes[0].SetActive(true);
                cardTypes[1].SetActive(false);
                cardTypes[2].SetActive(false);
                cardTypes[3].SetActive(false);
                break;
            case "Rare":
                cardTypes[0].SetActive(false);
                cardTypes[1].SetActive(true);
                cardTypes[2].SetActive(false);
                cardTypes[3].SetActive(false);
                break;
            case "Epic":
                cardTypes[0].SetActive(false);
                cardTypes[1].SetActive(false);
                cardTypes[2].SetActive(true);
                cardTypes[3].SetActive(false);
                break;
            case "Legendary":
                cardTypes[0].SetActive(false);
                cardTypes[1].SetActive(false);
                cardTypes[2].SetActive(false);
                cardTypes[3].SetActive(true);
                break;
        }

    }
    public void TogglePanel()
    {
        if (gameObject.activeSelf)
        {
            gameObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(true);
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }


}
