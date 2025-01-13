using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]
public class Items : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int itemPrice;

    public virtual List<string> UseItemInField(List<PlayerCharacterData> target)
    {
        return new List<string>();
    }
}


