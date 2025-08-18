using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Units/Unit Data")]
public class PlacementHeroData : ScriptableObject
{
    public string unitName;
    public Sprite cardIcon;
    public UnitType unitType;
    public GameObject prefab;
    public int size;
    public int cost;

}

public enum UnitType
{
    Hero,
    Building,
    Spell
}
