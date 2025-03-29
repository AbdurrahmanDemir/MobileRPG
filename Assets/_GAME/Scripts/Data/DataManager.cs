using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Header(" Data ")]
    [SerializeField] private int gold;
    [SerializeField] private int xp;
    [SerializeField] private int energy;

    private GameObject popUp;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();

    }


    public bool TryPurchaseGold(int price)
    {
        if (price <= gold)
        {
            gold -= price;
            SaveData();
            UpdateGoldText();
            return true;
        }
        else
        {
            PopUpController.instance.OpenPopUp("NOT ENOUGH GOLD");

        }
        return false;
    }

    public void AddGold(int value)
    {
        gold += value;
        UpdateGoldText();
        SaveData();
    }

    public void AddXP(int value)
    {
        xp += value;
        UpdateXPText();
        SaveData();
    }

    private void UpdateGoldText()
    {
        TextMeshProUGUI coinText = GameObject.FindGameObjectWithTag("CoinText").GetComponent<TextMeshProUGUI>();
        coinText.text = gold.ToString();
    }

    private void UpdateXPText()
    {
            TextMeshProUGUI xpText = GameObject.FindGameObjectWithTag("XpText").GetComponent<TextMeshProUGUI>();
            xpText.text = xp.ToString();

    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetInt("Energy", energy);
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            gold = PlayerPrefs.GetInt("Gold");
        }
        else
        {
            AddGold(100);
            AddEnergy(5);
        }
        xp = PlayerPrefs.GetInt("XP", 0);
        energy = PlayerPrefs.GetInt("Energy", energy);

        Debug.Log("GOLD" + gold + "XP" + xp);

        SaveData();
        UpdateGoldText();
        UpdateXPText();
        UpdateEnergyText();
    }

    // Energy

    public bool TryPurchaseEnergy(int price)
    {
        if (price <= energy)
        {
            energy -= price;
            SaveData();
            UpdateEnergyText();
            return true;
        }
        else
        {
            PopUpController.instance.OpenPopUp("NOT ENOUGH ENERGY");
        }
        return false;
    }

    private void UpdateEnergyText()
    {
        TextMeshProUGUI energyText = GameObject.FindGameObjectWithTag("EnergyText").GetComponent<TextMeshProUGUI>();
        energyText.text = energy.ToString();
    }

    public void AddEnergy(int value)
    {
        energy += value;
        UpdateEnergyText();
        SaveData();
    }
}
