using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "New Revive Skill")]
public class RevivalSkill : Skill
{
    public override List<string> UseSkill(Character character, List<Character> targets, int turnCounter)
    {
        List<string> result = new List<string>();

        foreach (var target in targets)
        {
            if (!target.isActive)
            {
                target.isActive = true;

                int heals = (int)((character.magAtk * powerModifier * Random.Range(.85f, 1.25f)) / targets.Count);

                if (heals > 9999) heals = 9999;

                target.currHP += heals;

                if (target.currHP > target.maxHP) target.currHP = target.maxHP;

                result.Add(heals == 0 ? "Miss" : heals.ToString());
            }
            else
                result.Add("Miss");
        }

        return result;
    }
}
