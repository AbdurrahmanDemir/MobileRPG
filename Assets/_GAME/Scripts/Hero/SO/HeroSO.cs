using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Creat Hero")]
public class HeroSO : ScriptableObject
{
    [Header("Hero")]
    public string heroName;
    public Sprite heroImage;
    [Header("Hero Settings")]
    public string attackType;
    public bool isAreaOfEffect;
    public int maxHealth;
    public int damage;
    public float range;
    public float cooldown;
}

