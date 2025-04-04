using UnityEngine;

[CreateAssetMenu(fileName = "MenuHero", menuName = "HeroCard")]

public class MenuHeroCardSO : ScriptableObject
{
    [Header("Hero")]
    public string cardName;
    public Sprite heroIcon;
    public CardType cardType;
    public int baseUpgradeCost = 10;
    public float range;
    public float damage;
    public float health;
    public float hitSpeed;
    public float moveSpeed;
    public float undergroundRange;
    [Header("Other Stat")]
    public string specialStatName;
    public float specialStat;
    [TextArea] public string cardDescription;


    public float GetCurrentHealth()
    {
        return PlayerPrefs.GetFloat($"{cardName}_Health", health);
    }
    public float GetCurrentDamage()
    {
        return PlayerPrefs.GetFloat($"{cardName}_Damage", damage);
    }
    public int GetUpgradeCost()
    {
        return PlayerPrefs.GetInt($"{cardName}_UpgradeCostHero", baseUpgradeCost);
    }
    public void UpgradeDamage()
    {
        float currentDamage = GetCurrentDamage();
        int upgradeCost = GetUpgradeCost();
        float currentHealth = GetCurrentHealth();

        // Yeni deðerleri hesapla
        float newDamage = currentDamage + 1;
        int newUpgradeCost = upgradeCost * 2;
        float newHealth = currentHealth + 1;

        // PlayerPrefs'e kaydet
        PlayerPrefs.SetFloat($"{cardName}_Damage", newDamage);
        PlayerPrefs.SetFloat($"{cardName}_Health", newHealth);
        PlayerPrefs.SetInt($"{cardName}_UpgradeCost", newUpgradeCost);
        PlayerPrefs.Save();
    }
}
public enum CardType
{
    Cammon=0,
    Rare=1,
    Epic=2,
    Legendary=3
}


