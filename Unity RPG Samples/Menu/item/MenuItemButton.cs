using UnityEngine;
using UnityEngine.UI;

public class MenuItemButton : MonoBehaviour
{
    public Text quantity;
    public Text itemName;
    public InventorySlots currSlot;
    public Items item;

    public void CreateButton(InventorySlots slot)
    {
        currSlot = slot;
        item = slot.item;
        quantity.text = currSlot.itemCount.ToString();
        itemName.text = item.itemName;
    }

    public void CreateButton(KeyItems keyItem)
    {
        quantity.text = "1";
        itemName.text = keyItem.itemName;
        item = keyItem;
    }

    public void UpdateButtonData()
    {
        quantity.text = currSlot.itemCount.ToString();
    }
}

