using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Playables;
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

        public List<Statuses> applySelfStatuses;

        public List<Statuses> applyTargetStatuses;

        public List<Statuses> removeSelfStatuses;

        public List<Statuses> removeTargetStatuses;

    public List<string> HandleStatusApplication(Character character, List<Character> targets, int turnCounter)
    {
        List<string> results = new List<string>();
        if (applySelfStatuses.Count > 0)
        {
            foreach (var status in applySelfStatuses)
            {
                if (character.immunities.Where(s => s.status == status.status).FirstOrDefault() == null &&
                    (character.resistances.Where(s => s.status == status.status).FirstOrDefault() == null ||
                    Random.Range(0, 1) == 1))
                {
                    status.expirationTurn += turnCounter;
                    character.currStatuses.Add(status);
                }
            }
        }

        if (removeSelfStatuses.Count > 0) 
        { 
            foreach (var status in removeSelfStatuses)
            {
                var curingStatus = character.currStatuses.FirstOrDefault(s => s.status == status.status);
                if (curingStatus != null && curingStatus.canBeCured)
                {
                    character.currStatuses.RemoveAll(s => s.status == status.status);

                }
            }
        }

        foreach (var target in targets)
        {
            if (applyTargetStatuses.Count > 0)
            {
                foreach (var status in applyTargetStatuses)
                {
                    if (character.immunities.Where(s => s.status == status.status).FirstOrDefault() == null &&
                        (character.resistances.Where(s => s.status == status.status).FirstOrDefault() == null ||
                        Random.Range(0, 1) == 1))
                    { 
                        status.expirationTurn += turnCounter;
                        target.currStatuses.Add(status);
                        if (status.status == Status.DEATH)
                        {
                            target.currHP = 0;
                            target.isActive = false;
                        }
                            results.Add(status.status.ToString());
                    }
                    else if (character.immunities.Where(s => s.status == status.status).FirstOrDefault() != null)
                        results.Add("Immune");
                    else
                        results.Add("Missed");
                }
            }
            if (removeTargetStatuses.Count > 0)
            {
                foreach (var status in applyTargetStatuses)
                {
                    var curingStatus = target.currStatuses.FirstOrDefault(s => s.status == status.status);
                    if (curingStatus != null && curingStatus.canBeCured)
                    {
                        target.currStatuses.RemoveAll(s => s.status == status.status);
                    }
                }
            }
        }
        return results;
    }

    public List<string> UseAttackingSkill(Character character, List<Character> targets, int turnCounter)
    {
        List<string> returnDamages = new List<string>();
        double hitCheck;
        double charAgility = character.agility, enemyAgility;

        foreach (var target in targets)
        {
            enemyAgility = target.agility;
            hitCheck = (((charAgility / enemyAgility) * accuracyModifier) + .01) * 100;
            if (isMagic || hitCheck >= Random.Range(0, 100))
            {
                int damage = (int)((isMagic ? character.magAtk : character.attack * character.FindPhysicalAttackStatusModifier())
                    * Random.Range(1f, 1.25f) * powerModifier * ((Random.Range(1, 20) * criticalModifier) >= Random.Range(1, 20) ? 2 : 1)) / targets.Count;

                damage = (int)(damage - (isMagic ? target.defense * target.FindPhysicalDamageStatusModifier() : target.magDef));

                if (damage <= 0) damage = 1;

                target.currHP -= damage;

                if (target.currHP < 0) target.currHP = 0;

                if (target.currHP == 0) target.isActive = false;

                returnDamages.Add(damage.ToString());
            }
            else returnDamages.Add("MISS");
        }

        return returnDamages;
    }

    public List<string> UseHealingSkill(Character character, List<Character> targets, int turnCounter)
    {
        List<string> returnHeals = new List<string>();

        foreach (var target in targets)
        {
            int heals = (int)((character.magAtk * powerModifier * Random.Range(.85f, 1.25f)) / targets.Count);

            target.currHP += heals;

            if (target.currHP > target.maxHP) target.currHP = target.maxHP;

            returnHeals.Add(heals == 0 ? "MISS" : heals.ToString());
        }

        return returnHeals;
    }

    public void UseStatusSkill(Character character, List<Character> targets, int turnCounter)
    {
        // animation call goes here
    }

    public void UseMixedSkill(Character character, List<Character> targets) { return; }
}