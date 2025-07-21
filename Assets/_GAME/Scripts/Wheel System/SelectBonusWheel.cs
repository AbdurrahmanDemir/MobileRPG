using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectBonusWheel : PointerMover
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UpgradeSelectManager upgradeSelectManager;
    public TextMeshProUGUI[] heroSlotsText;

    private void Start()
    {
        if (stopButton != null)
            stopButton.onClick.AddListener(StopPointer);

        WheelSOConfig();
        BonusAlertImage();
    }
    protected override void Apply(string slotName)
    {
        WheelManager.instance.heroes.Add(slotName);
        WheelManager.instance.wheelStopButtons[0].interactable = false;
        WheelManager.instance.wheelStopButtons[1].interactable = false;
        WheelManager.instance.wheelStopButtons[2].interactable = false;

        WheelManager.instance.attackButton.interactable = true;

        switch (slotName)
        {
            case "+5 Health":
                upgradeSelectManager.HeroHealthItem(5);
                break;
            case "+10 Health":
                upgradeSelectManager.HeroHealthItem(5);
                break;
            case "+2 Damage":
                upgradeSelectManager.HeroDamageItem(2);
                break;
            case "+5 Damage":
                upgradeSelectManager.HeroDamageItem(5);
                break;
            case "+10 Gold":
                DataManager.instance.AddGold(10);
                break;
            case "+30 Gold":
                DataManager.instance.AddGold(30);
                break;
            case "+50 Gold":
                DataManager.instance.AddGold(50);
                break;
            case "+5 Energy":
                DataManager.instance.AddEnergy(5);
                break;
            case "+10 Energy":
                DataManager.instance.AddEnergy(10);
                break;
            case "+1 Count":
                int number = System.Convert.ToInt32(WheelManager.instance.heroes[1]);
                number++;
                WheelManager.instance.heroes[1] = number.ToString();
                break;
            case "+2 Count":
                int number1 = System.Convert.ToInt32(WheelManager.instance.heroes[1]);
                number1+=2;
                WheelManager.instance.heroes[1] = number1.ToString();
                break;
            case "2x Count":
                int number2 = System.Convert.ToInt32(WheelManager.instance.heroes[1]);
                number2 *= 2;
                WheelManager.instance.heroes[1] = number2.ToString();
                break;

        }


    }

    public void BonusAlertImage()
    {
        for (int i = 0; i < heroSlots.Length; i++)
        {
            switch (wheelSO.slotsName[i])
            {
                case "+5 Health":
                    heroSlotsText[i].text = "+5";
                    break;
                case "+10 Health":
                    heroSlotsText[i].text = "+10";
                    break;
                case "+2 Damage":
                    heroSlotsText[i].text = "+2";
                    break;
                case "+5 Damage":
                    heroSlotsText[i].text = "+5";
                    break;
                case "+10 Gold":
                    heroSlotsText[i].text = "+10";
                    break;
                case "+30 Gold":
                    heroSlotsText[i].text = "+30";
                    break;
                case "+50 Gold":
                    heroSlotsText[i].text = "+50";
                    break;
                case "+5 Energy":
                    heroSlotsText[i].text = "+5";
                    break;
                case "+10 Energy":
                    heroSlotsText[i].text = "+10";
                    break;
                case "+1 Count":
                    heroSlotsText[i].text = "+1";
                    break;
                case "+2 Count":
                    heroSlotsText[i].text = "+2";
                    break;
                case "2x Count":
                    heroSlotsText[i].text = "x2";
                    break;
            }
        }

    }
}
