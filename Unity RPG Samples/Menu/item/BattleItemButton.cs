using UnityEngine;
using UnityEngine.UI;

public class BattleItemButton : MonoBehaviour
{
    public Text itemName;
    public Items item;

    public void PopulateButton(Items newItem)
    {
        itemName.text = newItem.name;
        item = newItem;
    }
}
