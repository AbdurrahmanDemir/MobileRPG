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

            MenuHeroListCard heroScript = cardPrefabs.gameObject.GetComponent<MenuHeroListCard>();

            cardPrefabs.GetComponent<MenuHeroListCard>().Config(
                heroes[i].name,
                heroes[i].heroIcon,
                heroes[i].cardType.ToString());

            Button cardButton = heroScript.detailsButton;
            cardButton.onClick.AddListener(() => CardDetailsPanel(i-1));
        }
    }
    public void CardDetailsPanel(int index)
    {
        heroCardDetailsPrefabs.SetActive(true);

        //GameObject cardDetails = Instantiate(heroCardDetailsPrefabs, heroDetailsTransform);
        //MenuHeroDetails cardScript= cardDetails.gameObject.GetComponent<MenuHeroDetails>();

        heroCardDetailsPrefabs.GetComponent<MenuHeroDetails>().Config(
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
