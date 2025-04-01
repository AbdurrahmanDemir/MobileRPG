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

    [Header(" Data ")]
    [SerializeField] private int token;
    [SerializeField] private TextMeshProUGUI tokenText;


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
        //CheckIdles();
        UpdateTexts();
        AddToken(20);
    }
    void LoadData()
    {
        hookLength = -PlayerPrefs.GetInt("Length", 30);
        hookStrength = PlayerPrefs.GetInt("Strength", 3);
        offlineEarnings = PlayerPrefs.GetInt("Offline", 3);
        lengthCost = costs[-hookLength / 10 - 3];
        strengthCost = costs[hookStrength - 3];
        offlineEarningsCost = costs[offlineEarnings - 3];
    }
    public bool TryPurchaseToken(int price)
    {
        if (price <= token)
        {
            token -= price;
            UpdateTokenText();
            return true;
        }
        else
        {
            PopUpController.instance.OpenPopUp("NOT ENOUGH TOKEN");
        }
        return false;
    }

    public void AddToken(int value)
    {
        token += value;
        UpdateTokenText();
    }
    private void UpdateTokenText()
    {
        tokenText.text = token.ToString();
    }


    public void BuyLength()
    {
        if(TryPurchaseToken(costs[-hookLength / 10 - 3]))
        {
            hookLength -= 10;
            lengthCost = costs[-hookLength / 10 - 3];
            //PlayerPrefs.SetInt("Length", -hookLength);
            UpdateTexts();
        }

    }

    public void BuyStrength()
    {
        if (TryPurchaseToken(costs[hookStrength - 3]))
        {
            hookStrength++;
            strengthCost = costs[hookStrength - 3];
            //PlayerPrefs.SetInt("Strength", hookStrength);
            UpdateTexts();
        }

    }

    public void BuyOfflineEarnings()
    {
        if (TryPurchaseToken(costs[offlineEarnings - 3]))
        {
            offlineEarnings++;
            strengthCost = costs[offlineEarnings - 3];
            //PlayerPrefs.SetInt("Offline", offlineEarnings);
            UpdateTexts();
        }

    }

    public void CollectMoney()
    {
        AddToken(totalGain);
    }

    public void CollectDoubleMoney()
    {
        AddToken(totalGain * 2);

    }
    public void UpdateTexts()
    {
        lengthCostText.text = lengthCost.ToString();
        lengthValueText.text = -hookLength + "m";
        strengthCostText.text = strengthCost.ToString();
        strengthValueText.text = hookStrength + " heroes.";
        offlineCostText.text = offlineEarningsCost.ToString();
        offlineValueText.text = "$" + offlineEarnings + "/min";
    }

    //public void CheckIdles()
    //{        
    //    if (token < lengthCost)
    //        lengthButton.interactable = false;
    //    else
    //        lengthButton.interactable = true;

    //    if (token < strengthCost)
    //        strengthButton.interactable = false;
    //    else
    //        strengthButton.interactable = true;

    //    if (token < offlineEarningsCost)
    //        offlineButton.interactable = false;
    //    else
    //        offlineButton.interactable = true;
    //}
}
