using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum Elements { NONE, FIRE, WATER, EARTH, ELECTRIC, WIND, LIGHT, DARK, }
public abstract class Character : MonoBehaviour
{
    public string unitName;
    public int level;
    public int maxHP;
    public int maxSP;
    public int currSP;
    public int currHP;
    public int strength;
    public int constitution;
    public int intelligence;
    public int spirit;
    public int speed;
    public int luck;
    public List<Statuses> currStatuses;
    public virtual List<Status> resistances { get; }
    public virtual List<Status> immunities { get; }
    public virtual List<Elements> elemWeaknesses { get; }
    public virtual List<Elements> elemResistances { get; }
    public virtual List<Elements> elemAbsorptions { get; }
    public bool isActive;
    public bool isBackRow;

    public virtual int attack { get { return strength; } }
    public virtual int defense { get { return constitution; } }
    public virtual int magAtk {  get { return intelligence; } }
    public virtual int magDef {  get { return spirit; } }
    public virtual int agility { get { return speed; } }

    public virtual float FindPhysicalDamageStatusModifier()
    {
        float modifier = 1;

        if (currStatuses.Where(s => s.status == Status.VULNERABLE).FirstOrDefault() != null) { modifier *= 2; }

        if (currStatuses.Where(s => s.status == Status.AFRAID).FirstOrDefault() != null) { modifier *= 1.5f; }

        if (currStatuses.Where(s => s.status == Status.DEFENDING).FirstOrDefault() != null) { modifier /= 2; }

        return modifier;
    }

    public virtual float FindPhysicalAttackStatusModifier()
    {
        float modifier = 1;

        if (currStatuses.Where(s => s.status == Status.VULNERABLE).FirstOrDefault() != null) { modifier /= 2; }

        if (currStatuses.Where(s => s.status == Status.AFRAID).FirstOrDefault() != null) { modifier /= 1.5f; }

        if (currStatuses.Where(s => s.status == Status.BERSERK).FirstOrDefault() != null) { modifier *= 1.5f; }

        return modifier;
    }

    public virtual float FindElementalDamageModifier(Elements atkElement)
    {
        float modifier = 1;

        foreach (var element in elemResistances)
        {
            if (element == atkElement) modifier /= 2;
        }

        foreach (var element in elemWeaknesses)
        {
            if (element == atkElement) modifier *= 2;
        }

        return modifier;
    }

    public virtual int Attack(Character enemy, int turnCounter)
    {
        double charAgility = agility, enemyAgility = enemy.agility;
        double hitChance = ((charAgility * 2 / enemyAgility) + .01) * 100;

        if (hitChance >= UnityEngine.Random.Range(0, 100))
        {
            int criticalValue = UnityEngine.Random.Range(1, 20) == 20 ? 2 : 1;
            int damage = (int)((attack * FindPhysicalAttackStatusModifier() * UnityEngine.Random.Range(1f, 1.25f) * criticalValue) - (enemy.defense));
            damage = (int)(damage / enemy.FindPhysicalDamageStatusModifier());
            if (damage <= 0)
            {
                damage = 1;
            }

            enemy.currHP -= damage;
            if (enemy.currHP < 0) enemy.currHP = 0;

            return damage;
        }
        return 0;
    }
    public virtual bool FleeCheck(Character enemy, int turnCounter)
    {
        double playerHpMultiplier = maxHP / currHP;
        double enemyHpMultiplier = enemy.currHP / enemy.maxHP;
        int playerChance = UnityEngine.Random.Range(1, 25);

        double playerFleeNum = playerHpMultiplier * (.5 * luck + agility) + playerChance;
        double enemyFleeNum = enemyHpMultiplier * (enemy.luck + enemy.agility);

        return playerFleeNum > enemyFleeNum;
    }

    public virtual void DeepCopyFrom(CharacterData c)
    {
        unitName = c.unitName;
        level = c.level;
        maxHP = c.maxHP;
        maxSP = c.maxSP;
        currSP = c.currSP;
        currHP = c.currHP;
        strength = c.strength;
        constitution = c.constitution;
        intelligence = c.intelligence;
        spirit = c.spirit;
        speed = c.speed;
        luck = c.luck;
        currStatuses = c.currStatuses.ConvertAll(cs => new Statuses(cs)).ToList();
        isActive = c.isActive;
        isBackRow = c.isBackRow;
    }
}
