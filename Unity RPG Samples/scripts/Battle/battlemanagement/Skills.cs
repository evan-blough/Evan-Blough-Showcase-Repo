using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public enum SkillType { ATTACK, HEAL, STATUS, REVIVE };

    [System.Serializable]
    public class Skills
    {
        public string skillName;

        public string skillDescription;

        public int skillPointCost;

        public bool active;

        public int levelReq;

        public float criticalModifier;

        public float powerModifier;

        public float accuracyModifier;

        public bool isMagic;

        public SkillType type;

        public bool isRanged;

        public bool isMultiTargeted;

        public Elements elemAttribute;

        public List<Statuses> applySelfStatuses;

        public List<Statuses> applyTargetStatuses;

        public List<Statuses> removeSelfStatuses;

        public List<Statuses> removeTargetStatuses;

    public void SelfStatusApplication(Character character, Statuses oldStatus, int turnCounter)
    {
        Statuses status = new Statuses(oldStatus);
        if (character.immunities.Any(s => s == status.status) &&
            (character.resistances.Any(s => s == status.status) ||
            Random.Range(0, 1) == 1))
        {
            status.expirationTurn += turnCounter;
            character.currStatuses.Add(status);
        }
    }

    public void SelfStatusCure(Character character, Statuses oldStatus, int turnCounter)
    {
        Statuses status = new Statuses(oldStatus);
        var curingStatus = character.currStatuses.FirstOrDefault(s => s.status == status.status);
        if (curingStatus != null && curingStatus.canBeCured)
        {
            character.currStatuses.RemoveAll(s => s.status == status.status);

        }
    }

    public string TargetStatusApplication(Character character, Statuses statusToApply, Character target, int turnCounter)
    {
        Statuses status = new Statuses(statusToApply);
        if (target.immunities.Where(s => s == status.status).Any())
            return "Immune";
        if (status.accuracy * 100 >= Random.Range(0, 100))
        {
            if (!target.resistances.Where(s => s == status.status).Any() || Random.Range(0, 1) == 1)
            {
                if (target.currStatuses.Any(s => s.status == status.status))
                {
                    target.currStatuses.RemoveAll(s => s.status == status.status);
                }

                status.expirationTurn += turnCounter;
                target.currStatuses.Add(status);
                if (status.status == Status.DEATH)
                {
                    target.currHP = 0;
                    target.isActive = false;
                }
                return status.status.ToString();
            }
            return "Resisted";
        }
        return "Missed";
    }

    public void TargetStatusRemoval(Character character, Statuses statusToRemove, Character target, int turnCounter)
    {
        Statuses status = new Statuses(statusToRemove);
        var curingStatus = target.currStatuses.FirstOrDefault(s => s.status == status.status);
        if (curingStatus != null && curingStatus.canBeCured)
        {
            target.currStatuses.RemoveAll(s => s.status == status.status);
        }
    }

    public List<string> HandleStatusApplication(Character character, List<Character> targets, int turnCounter)
    {
        List<string> results = new List<string>();
        if (applySelfStatuses.Count > 0)
        {
            foreach (var status in applySelfStatuses)
                SelfStatusApplication(character, status, turnCounter);
        }

        if (removeSelfStatuses.Count > 0) 
        {
            foreach (var status in removeSelfStatuses)
                SelfStatusCure(character, status, turnCounter);
        }

        foreach (var target in targets)
        {
            if (applyTargetStatuses.Count > 0)
            {
                foreach (var status in applyTargetStatuses)
                {
                   results.Add(TargetStatusApplication(character, status, target, turnCounter));
                }
            }
            if (removeTargetStatuses.Count > 0)
            {
                foreach (var status in applyTargetStatuses)
                    TargetStatusRemoval(character, status, target, turnCounter);
            }
        }
        return results;
    }

    public List<string> UseAttackingSkill(PlayerCharacter character, List<Character> targets, int turnCounter)
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

    public List<string> UseHealingSkill(PlayerCharacter character, List<Character> targets, int turnCounter)
    {
        List<string> returnHeals = new List<string>();

        foreach (var target in targets)
        {
            int heals = (int)((character.magAtk * powerModifier * Random.Range(.85f, 1.25f)) / targets.Count);

            target.currHP += heals;

            if (target.currHP > target.maxHP) target.currHP = target.maxHP;

            returnHeals.Add(heals == 0 ? "Miss" : heals.ToString());
        }

        return returnHeals;
    }

    public List<string> UseRevivalSkill(PlayerCharacter character, List<Character> targets, int turnCounter)
    {
        List<string> result = new List<string>();

        foreach (var target in targets)
        {
            target.isActive = true;

            int heals = (int)((character.magAtk * powerModifier * Random.Range(.85f, 1.25f)) / targets.Count);

            target.currHP += heals;

            if (target.currHP > target.maxHP) target.currHP = target.maxHP;

            result.Add(heals == 0 ? "Miss" : heals.ToString());
        }

        return result;
    }

    public List<string> UseStatusSkill(PlayerCharacter character, List<Character> targets, int turnCounter)
    {
        // animation call goes here
        return HandleStatusApplication(character, targets, turnCounter);
    }

    public List<string> UseMixedSkill(PlayerCharacter character, List<Character> targets, int turnCounter) 
    { 
        return HandleStatusApplication(character, targets, turnCounter); 
    }
}