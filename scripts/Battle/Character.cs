using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public abstract class Character : MonoBehaviour
{
    public string unitName;
    public int maxHP;
    public int maxSP;
    public int currSP;
    public int currHP;
    public int attack;
    public int defense;
    public int luck;
    public int agility;
    public int magAtk;
    public int magDef;
    public List<Skills> skills;
    public int level;
    public List<Statuses> currStatuses;
    public List<Statuses> resistances;
    public List<Statuses> immunities;
    public bool isActive;
    public bool isIncapacitated;
    public bool isBackRow;

    public virtual double FindPhysicalDamageStatusModifier()
    {
        double modifier = 1;

        if (currStatuses.Where(s => s.status == Status.VULNERABLE).FirstOrDefault() != null) modifier *= 2;
        if (currStatuses.Where(s => s.status == Status.AFRAID).FirstOrDefault() != null) modifier = modifier * 1.5;
        if (currStatuses.Where(s => s.status == Status.DEFENDING).FirstOrDefault() != null) modifier /= 2;

       return modifier;
    }

    public virtual double FindPhysicalAttackStatusModifier()
    {
        double modifier = 1;

        if (currStatuses.Where(s => s.status == Status.VULNERABLE).FirstOrDefault() != null) modifier /= 2;
        if (currStatuses.Where(s => s.status == Status.AFRAID).FirstOrDefault() != null) modifier = modifier / 1.5;
        if (currStatuses.Where(s => s.status == Status.BERSERK).FirstOrDefault() != null) modifier *= 2;

        return modifier;
    }

    public virtual int Attack(Character enemy)
    {
        double charAgility = agility, enemyAgility = enemy.agility;
        double hitChance = ((charAgility / enemyAgility) + .01) * 100;

        if (hitChance >= UnityEngine.Random.Range(0, 100))
        {
            int damage = (int)(attack * FindPhysicalAttackStatusModifier() * UnityEngine.Random.Range(1f, 1.25f) * (UnityEngine.Random.Range(1, 20) == 20 ? 2 : 1))
                - (int)(enemy.defense * enemy.FindPhysicalDamageStatusModifier());

            if (damage <= 0)
            {
                damage = 1;
            }

            enemy.currHP -= damage;

            return damage;
        }
        return 0;
    }
    public virtual bool FleeCheck(Character enemy)
    {
        float playerHpMultiplier = maxHP / currHP;
        float enemyHpMultiplier = enemy.currHP / enemy.maxHP;
        int playerChance = UnityEngine.Random.Range(1, 25);
        int playerFleeNum = (int)(playerHpMultiplier * (luck + agility + (level * 5)));
        int enemyFleeNum = (int)(enemyHpMultiplier * (enemy.luck + enemy.agility + (level * 5)));
        return playerFleeNum > enemyFleeNum;
    }
}
