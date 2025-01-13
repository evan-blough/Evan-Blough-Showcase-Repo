using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gag Item", menuName = "Items/BattleItem/GagItem")]
public class GagItem : BattleItem
{
    public override List<string> UseItem(List<Character> targets, int turnCounter)
    {
        List<string> result = new List<string>();

        foreach (Character target in targets)
        {
            result.Add("Yippee!");
        }

        return result;
    }
}
