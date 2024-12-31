using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Attacking Skill")]
public class AttackSkill : Skill
{
    public override List<string> UseSkill(Character character, List<Character> targets, int turnCounter)
    {
        List<string> returnDamages = new List<string>();
        double hitCheck;
        double charAgility = character.agility, enemyAgility;

        foreach (var target in targets)
        {
            if (target.elemAbsorptions.Where(e => e == elemAttribute).Any()) { returnDamages.Add("Immune"); }
            else
            {
                enemyAgility = target.agility;
                hitCheck = (((charAgility / enemyAgility) * accuracyModifier) + .01) * 100;
                if (hitCheck >= Random.Range(0, 100))
                {
                    foreach (var status in applyTargetStatuses) TargetStatusApplication(character, status, target, turnCounter);
                    foreach (var status in removeTargetStatuses) TargetStatusRemoval(character, status, target, turnCounter);

                    int damage = (int)((isMagic ? character.magAtk : character.attack * character.FindPhysicalAttackStatusModifier())
                        * Random.Range(1f, 1.25f) * powerModifier * ((Random.Range(1, 20) * criticalModifier) >= Random.Range(1, 20) ? 2 : 1)) / targets.Count;

                    damage = (int)(damage - (isMagic ? target.magDef : target.defense));

                    damage = (int)(damage * target.FindElementalDamageModifier(elemAttribute));

                    if (damage <= 0) damage = 1;
                    if (damage > 9999) damage = 9999;

                    target.currHP -= damage;

                    if (target.currHP < 0) target.currHP = 0;

                    if (target.currHP <= 0) target.isActive = false;

                    returnDamages.Add(damage.ToString());
                }
                else returnDamages.Add("Miss");
            }
        }

        return returnDamages;
    }
}

