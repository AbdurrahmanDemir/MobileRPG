using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HookManager : MonoBehaviour
{
    public static HookManager instance;

    [Header("Settings")]
    public int hookLength;
    public int hookStrength;
    [SerializeField] private int offlineEarnings;
    [SerializeField] private int lengthCost;
    [SerializeField] private int strengthCost;
    [SerializeField] private int offlineEarningsCost;
    [SerializeField] private int wallet;
    [SerializeField] private int totalGain;


    [Header("UI Settings")]
    public Button lengthButton;
    public Button strengthButton;
    public Button offlineButton;

    public TextMeshProUGUI gameScreenMoney;
    public TextMeshProUGUI lengthCostText;
    public TextMeshProUGUI lengthValueText;
    public TextMeshProUGUI strengthCostText;
    public TextMeshProUGUI strengthValueText;
    public TextMeshProUGUI offlineCostText;
    public TextMeshProUGUI offlineValueText;
    public TextMeshProUGUI endScreenMoney;
    public TextMeshProUGUI returnScreenMoney;

    private int[] costs = new int[]
  {
        120,
        151,
        197,
        250,
        324,
        414,
        537,
        687,
        892,
        1145,
        1484,
        1911,
        2479,
        3196,
        4148,
        5359,
        6954,
        9000,
        11687
  };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        LoadData();
    }
    void Start()
    {
        CheckIdles();
        UpdateTexts();
    }
    void LoadData()
    {
        hookLength = -PlayerPrefs.GetInt("Length", 30);
        hookStrength = PlayerPrefs.GetInt("Strength", 3);
        offlineEarnings = PlayerPrefs.GetInt("Offline", 3);
        lengthCost = costs[-hookLength / 10 - 3];
        strengthCost = costs[hookStrength - 3];
        offlineEarningsCost = costs[offlineEarnings - 3];
        wallet = PlayerPrefs.GetInt("Wallet", 50000);
    }

    public void BuyLength()
    {
        hookLength -= 10;
        wallet -= lengthCost;
        lengthCost = costs[-hookLength / 10 - 3];
        PlayerPrefs.SetInt("Length", -hookLength);
        PlayerPrefs.SetInt("Wallet", wallet);
        //ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void BuyStrength()
    {
        hookStrength++;
        wallet -= strengthCost;
        strengthCost = costs[hookStrength - 3];
        PlayerPrefs.SetInt("Strength", hookStrength);
        PlayerPrefs.SetInt("Wallet", wallet);
        //ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void BuyOfflineEarnings()
    {
        offlineEarnings++;
        wallet -= offlineEarningsCost;
        strengthCost = costs[offlineEarnings - 3];
        PlayerPrefs.SetInt("Offline", offlineEarnings);
        PlayerPrefs.SetInt("Wallet", wallet);
        //ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void CollectMoney()
    {
        wallet += totalGain;
        PlayerPrefs.SetInt("Wallet", wallet);
        //ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void CollectDoubleMoney()
    {
        wallet += totalGain * 2;
        PlayerPrefs.SetInt("Wallet", wallet);
        //ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }
    public void UpdateTexts()
    {
        lengthCostText.text = "$" + lengthCost;
        lengthValueText.text = -hookLength + "m";
        strengthCostText.text = "$" + strengthCost;
        strengthValueText.text = hookStrength + " fishes.";
        offlineCostText.text = "$" + offlineEarningsCost;
        offlineValueText.text = "$" + offlineEarnings + "/min";
    }

    public void CheckIdles()
    {        
        if (wallet < lengthCost)
            lengthButton.interactable = false;
        else
            lengthButton.interactable = true;

        if (wallet < strengthCost)
            strengthButton.interactable = false;
        else
            strengthButton.interactable = true;

        if (wallet < offlineEarningsCost)
            offlineButton.interactable = false;
        else
            offlineButton.interactable = true;
    }
}
