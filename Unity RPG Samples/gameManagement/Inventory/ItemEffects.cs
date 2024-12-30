using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ItemEffects
{
    public int effectValue;
    public bool isMultiTargeted;
    public ItemEffectType itemType;
    public List<Statuses> statuses;
    public Elements element;

    public List<string> AttackEffect(List<Character> targets)
    {
        List<string> result = new List<string>();
        int newEffectVal;

        foreach (Character target in targets)
        {
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

    public List<string> HealEffect(List<Character> targets)
    {
        List<string> result = new List<string>();

        foreach (Character target in targets)
        {
            target.currHP += effectValue;

            if (target.currHP > target.maxHP) target.currHP = target.maxHP;

            result.Add(effectValue.ToString());
        }

        return result; 
    }

    public List<string> RecoverSPEffect(List<Character> targets)
    {
        List<string> result = new List<string>();

        foreach (Character target in targets)
        {
            target.currSP += effectValue;
            
            if (target.currSP > target.maxSP) target.currSP = target.maxSP;

            result.Add(effectValue.ToString());
            
        }

        return result;
    }

    public List<string> ApplyStatusEffect(List<Character> targets, int turnCounter)
    {
        List<string> result = new List<string>();
        Statuses newStatus;

        foreach (Character target in targets)
        {
            foreach (Statuses status in statuses)
            {
                if (target.immunities.Any(s => s == status.status)) { result.Add("Immune"); }

                else if (target.resistances.Any(s => s == status.status) && Random.Range(0,1) == 0) { result.Add("Resisted"); }

                else if (status.accuracy * 100 < Random.Range(1, 100)) { result.Add("Missed"); }

                else
                {
                    if (target.currStatuses.Any(s => s.status == status.status))
                    {
                        newStatus = new Statuses(target.currStatuses.First(s => s.status == status.status));
                        target.currStatuses.RemoveAll(s => s.status == status.status);
                        newStatus.expirationTurn = turnCounter + status.expirationTurn;
                    }
                    else
                    {
                        newStatus = new Statuses(status);
                        newStatus.expirationTurn += turnCounter;
                    }
                    target.currStatuses.Add(newStatus);
                }
            }
        }

        return result; 
    }

    public List<string> RemoveStatusEffect(List<Character> targets)
    {
        List<string> result = new List<string>();

        foreach (Character target in targets)
        {
            foreach (Statuses status in statuses)
            {
                if (target.currStatuses.Any(s => s.status == status.status))
                {
                    target.currStatuses.RemoveAll(s => s.status == status.status);
                }
            }
        }

        return result;
    }

    public List<string> ReviveEffect(List<Character> targets)
    {
        List<string> result = new List<string>();

        foreach (Character target in targets)
        {
            target.isActive = true;

            target.currHP += effectValue;
            
            if (target.currHP > target.maxHP) target.currHP = target.maxHP;

            result.Add(effectValue.ToString());
        }

        return result; 
    }

    public List<string> GagEffect(List<Character> targets)
    {
        List<string> result = new List<string>();

        foreach (Character target in targets)
        {
            result.Add("Yippee!");
        }

        return result; 
    }


}
