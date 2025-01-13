using System.Collections.Generic;

public class BattleItem : Items
{
    public int effectValue;
    public bool isMultiTargeted;
    public List<Statuses> statuses;
    public Elements element;

    public virtual List<string> UseItem(List<Character> targets, int turnCounter)
    {
        List<string> results = new List<string>();

        return results;
    }

}

