using UnityEngine;

public class HookManager : MonoBehaviour
{
    public static HookManager instance;

    [HideInInspector]
    public int length;

    [HideInInspector]
    public int strength;

    [HideInInspector]
    public int offlineEarnings;

    [HideInInspector]
    public int lengthCost;

    [HideInInspector]
    public int strengthCost;

    [HideInInspector]
    public int offlineEarningsCost;

    [HideInInspector]
    public int wallet;

    [HideInInspector]
    public int totalGain;

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
    void LoadData()
    {
        length = -PlayerPrefs.GetInt("Length", 30);
        strength = PlayerPrefs.GetInt("Strength", 3);
        offlineEarnings = PlayerPrefs.GetInt("Offline", 3);
        lengthCost = costs[-length / 10 - 3];
        strengthCost = costs[strength - 3];
        offlineEarningsCost = costs[offlineEarnings - 3];
        wallet = PlayerPrefs.GetInt("Wallet", 0);
    }

    public void BuyLength()
    {
        length -= 10;
        wallet -= lengthCost;
        lengthCost = costs[-length / 10 - 3];
        PlayerPrefs.SetInt("Length", -length);
        PlayerPrefs.SetInt("Wallet", wallet);
        //ScreensManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void BuyStrength()
    {
        strength++;
        wallet -= strengthCost;
        strengthCost = costs[strength - 3];
        PlayerPrefs.SetInt("Strength", strength);
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
}
