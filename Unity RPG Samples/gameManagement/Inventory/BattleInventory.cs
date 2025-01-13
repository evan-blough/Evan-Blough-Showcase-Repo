using System.Collections.Generic;

[System.Serializable]
public class BattleInventory
{
    public List<BattleItem> items;
    public int maxItems;

    public void AddItem(BattleItem item)
    {
        if (items.Count == maxItems) return;

        items.Add(item);
    }
}
