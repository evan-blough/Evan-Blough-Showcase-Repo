using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Revive Item", menuName = "Items/BattleItem/Revive Item")]
public class ReviveItem : BattleItem
{
    public override List<string> UseItem(List<Character> targets, int turnCounter)
    {
        List<string> result = new List<string>();

        foreach (Character target in targets)
        {
            if (target.isActive)
            {
                result.Add("");
                continue;
            }

            target.isActive = true;

            target.currHP += effectValue;

            if (target.currHP > target.maxHP) target.currHP = target.maxHP;

            result.Add(effectValue.ToString());
        }

        return result;
    }
    public override List<string> UseItemInField(List<PlayerCharacterData> targets)
    {
        List<string> result = new List<string>();

        foreach (PlayerCharacterData target in targets)
        {
            if (target.isActive)
            {
                result.Add("");
                continue;
            }

            target.isActive = true;

            target.currHP += effectValue;

            if (target.currHP > target.maxHP) target.currHP = target.maxHP;

            result.Add(effectValue.ToString());
        }

        return result;
    }
}
