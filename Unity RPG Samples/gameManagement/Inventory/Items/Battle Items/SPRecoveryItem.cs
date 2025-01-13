using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SP Recovery Item", menuName = "Items/BattleItem/SP Recovery Item")]
public class SPRecoveryItem : BattleItem
{
    public override List<string> UseItem(List<Character> targets, int turnCounter)
    {
        List<string> result = new List<string>();

        foreach (Character target in targets)
        {
            if (!target.isActive)
            {
                result.Add("");
                continue;
            }

            target.currSP += effectValue;

            if (target.currSP > target.maxSP) target.currSP = target.maxSP;

            result.Add(effectValue.ToString());
        }

        return result;
    }
}
