using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuHeroListCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Image cardIconImage;
    [SerializeField] private GameObject[] cardTypes;
    public Button detailsButton;


    public void Config(string name, Sprite icon, string type)
    {
        cardNameText.text = name;
        cardIconImage.sprite = icon;
        switch (type)
        {
            case "Cammon":
                cardTypes[0].SetActive(true);
                break;
            case "Rare":
                cardTypes[1].SetActive(true);
                break;
            case "Epic":
                cardTypes[2].SetActive(true);
                break;
            case "Legendary":
                cardTypes[3].SetActive(true);
                break;
        }
    }
}
