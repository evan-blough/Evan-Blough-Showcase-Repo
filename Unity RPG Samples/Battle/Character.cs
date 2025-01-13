using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public virtual int attack => strength;
    public virtual int defense => constitution;
    public virtual int magAtk => intelligence;
    public virtual int magDef => spirit;
    public virtual int agility => speed;
    public virtual int hitPercent => 100;
    public virtual int dodgePercent => 0;

    public virtual float FindPhysicalDamageStatusModifier()
    {
        float modifier = 1;

        if (currStatuses.Where(s => s.status == Status.Vulnerable).FirstOrDefault() != null) { modifier *= 2; }

        if (currStatuses.Where(s => s.status == Status.Afraid).FirstOrDefault() != null) { modifier *= 1.5f; }

        if (currStatuses.Where(s => s.status == Status.Defending).FirstOrDefault() != null) { modifier /= 2; }

        return modifier;
    }

    public virtual float FindPhysicalAttackStatusModifier()
    {
        float modifier = 1;

        if (currStatuses.Where(s => s.status == Status.Vulnerable).FirstOrDefault() != null) { modifier /= 2; }

        if (currStatuses.Where(s => s.status == Status.Afraid).FirstOrDefault() != null) { modifier /= 1.5f; }

        if (currStatuses.Where(s => s.status == Status.Berserk).FirstOrDefault() != null) { modifier *= 1.5f; }

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
        double hitChance = hitPercent - enemy.dodgePercent;

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
