using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Skill : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public int skillPointCost;
    public float criticalModifier;
    public float powerModifier;
    public float accuracyModifier;
    public bool isMagic;
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
        if (!character.immunities.Any(s => s == status.status) &&
            (!character.resistances.Any(s => s == status.status) ||
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
        return "Miss";
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

    public virtual List<string> UseSkill(Character character, List<Character> targets, int turnCounter)
    {
        return new List<string>();
    }
}




