using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleInventory
{
    public List<Items> items;
    public int maxItems;

    public void AddItem(Items item)
    {
        if (items.Count == maxItems) return;

        items.Add(item);
    }
}
