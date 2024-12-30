using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Inventory
{
    public List<InventorySlots> items;
    public List<KeyItems> keyItems;

    public string AddItem(Items item, int count)
    {
        if (item is KeyItems)
        {
            keyItems.Add((KeyItems)item);
            return $"Acquired the {item.itemName}!";
        }
        else if (items.Any(i => i.item == item))
        {
            var toAdd = items.First(i => i.item == item);
            var beforeExcess = (toAdd.itemCount + count) - 999;
            toAdd.itemCount += count;
            if (toAdd.itemCount > 999)
            {
                toAdd.itemCount = 999;
                return $"What could we possibly need this many {item.itemName}s for? I can't carry any more! (Acquired {count - beforeExcess} {item.itemName}{((count - beforeExcess) > 1 ? "s" : string.Empty)})";
            }
        }
        else
        {
            InventorySlots newSlot = new InventorySlots(item, count);
            if (newSlot.itemCount > 999)
            {
                newSlot.itemCount = 999;
                items.Add(newSlot);
                return $"What could we possibly need this many {item.itemName}s for? I can't carry any more! (Acquired 999 {item.itemName}s.)";
            }

            items.Add(newSlot);
        }
        return $"Acquired {count} {item.itemName}{(count > 1 ? "s" : string.Empty)}.";
    }

    public string DiscardItem(Items item, int count)
    {
        if (item is KeyItems) return "I don't think I should throw this out.";

        if (!items.Any(i => i.item == item)) return "Error: Item not found.";

        InventorySlots toDiscard = items.First(i => i.item == item);
        toDiscard.itemCount -= count;

        return $"Threw out {count} {item.itemName}{(count > 1 ? "s" : string.Empty)}.";
    }
}
