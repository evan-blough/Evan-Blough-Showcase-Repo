using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlots : MonoBehaviour
{
    public Items item;
    public int itemCount;
    
    public InventorySlots(Items newItem, int newItemCount)
    {
        item = newItem;
        itemCount = newItemCount;
    }
}
