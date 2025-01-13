using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Item", menuName = "Items/BattleItem/Attack Item")]
public class AttackItem : BattleItem
{
    public override List<string> UseItem(List<Character> targets, int turnCounter)
    {
        List<string> result = new List<string>();
        int newEffectVal;

        foreach (Character target in targets)
        {
            if (!target.isActive)
            {
                result.Add("");
                continue;
            }

            if (target.elemAbsorptions.Any(e => e == element)) result.Add("Immune");
            else
            {
                newEffectVal = (int)(effectValue * target.FindElementalDamageModifier(element));

                if (newEffectVal <= 0) newEffectVal = 1;

                target.currHP -= newEffectVal;

                if (target.currHP <= 0)
                {
                    target.currHP = 0;
                    target.isActive = false;
                }

                result.Add(newEffectVal.ToString());
            }
        }

        return result;
    }
}
