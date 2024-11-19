using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentWeight { MAGIC = 0, LIGHT = 1, MEDIUM = 2, HEAVY = 3, VERY_HEAVY = 4 }
public class Equipment : MonoBehaviour
{
    public string equipmentName;
    public string equipmentDescription;
    public int attackBuff;
    public int defenseBuff;
    public int magicAttackBuff;
    public int magicDefenseBuff;
    public int agilityBuff;
    public EquipmentWeight weight;
    public Skills skill;
}
