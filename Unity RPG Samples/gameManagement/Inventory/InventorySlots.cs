using System;

[Serializable]
public class InventorySlots
{
    public Items item;
    public int itemCount;

    // for items that have more than 1 item
    public InventorySlots(Items newItem, int newItemCount)
    {
        item = newItem;
        itemCount = newItemCount;
    }

    // for items that only have one copy
    public InventorySlots(Items newItem)
    {
        item = newItem;
        itemCount = 1;
    }
}
