using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardList : MonoBehaviour
{
    [Header("Hero Panel")]
    [SerializeField] MenuHeroCardSO[] heroes;
    [SerializeField] GameObject heroCardPrefab;
    [SerializeField] Transform heroTransform;
    [SerializeField] GameObject heroCardDetailsPrefabs;
    [SerializeField] Transform heroDetailsTransform;

    private void Start()
    {
        HeroPanelUpdate();
    }

    public void HeroPanelUpdate()
    {
        for (int i = 0; i < heroes.Length; i++)
        {
            GameObject cardPrefabs = Instantiate(heroCardPrefab, heroTransform);

            MenuHeroListCard heroScript = cardPrefabs.GetComponent<MenuHeroListCard>();

            heroScript.Config(
                heroes[i].name,
                heroes[i].heroIcon,
                heroes[i].cardType.ToString());

            heroScript.cardIndex = i;

            Button cardButton = heroScript.detailsButton;

            int capturedIndex = i; // Index'i yakala
            cardButton.onClick.AddListener(() => CardDetailsPanel(capturedIndex));
        }
    }

    public void CardDetailsPanel(int index)
    {
        GameObject cardDetails = Instantiate(heroCardDetailsPrefabs, heroDetailsTransform);
        MenuHeroDetails cardScript = cardDetails.GetComponent<MenuHeroDetails>();
        DOTween.Kill(cardDetails.transform);
        cardDetails.transform.localScale = Vector3.zero;
        if (cardDetails != null)
        {
            cardDetails.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }


        // Instantiate edilen objeyi kullanarak konfigürasyon yap
        cardScript.Config(
            heroes[index].name,
            heroes[index].heroIcon,
            heroes[index].cardType.ToString(),
            heroes[index].cardDescription,
            heroes[index].GetCurrentDamage(),
            heroes[index].GetCurrentHealth(),
            heroes[index].range,
            heroes[index].hitSpeed,
            heroes[index].moveSpeed,
            heroes[index].GetUpgradeCost()
        );
    }

}
