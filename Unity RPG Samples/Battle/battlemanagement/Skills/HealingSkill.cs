using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "New Healing Skill")]
public class HealingSkill : Skill
{
    public override List<string> UseSkill(Character character, List<Character> targets, int turnCounter)
    {
        List<string> returnHeals = new List<string>();

        foreach (var target in targets)
        {
            int heals = (int)((character.magAtk * powerModifier * Random.Range(.85f, 1.25f)) / targets.Count);

            if (heals > 9999) heals = 9999;

            target.currHP += heals;

            if (target.currHP > target.maxHP) target.currHP = target.maxHP;

            returnHeals.Add(heals == 0 ? "Miss" : heals.ToString());
        }

        return returnHeals;
    }
}
