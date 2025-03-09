using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Creat Enemy")]

public class EnemySO : ScriptableObject
{
    [Header("Enemy")]
    public string enemyName;
    public Sprite enemyImage;
    [Header("Enemy Settings")]
    public string attackType;
    public bool isAreaOfEffect;
    public int maxHealth;
    public int damage;
    public float range;
    public float moveSpeed;
    public float cooldown;
}
