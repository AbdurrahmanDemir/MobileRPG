using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wheel", menuName = "Creat Wheel")]

public class WheelSO : ScriptableObject
{
    public string wheelName;
    public Sprite[] slotsImage;
    public string[] slotsName;
}
