using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class PlayerCharacter : Character
{
    public EquipmentWeight weightClass;
    public List<Skills> skills;
    public Weapon weapon;
    public Armor armor;
    public Accessory accessory1;
    public Accessory accessory2;
    public int gold;
    public bool isInParty;
    public int exp;

    public override int attack 
    { 
        get 
        {
            return strength + (weapon is null ? 0 : weapon.attackBuff) + (armor is null ? 0 : armor.attackBuff) 
                + (accessory1 is null ? 0 : accessory1.attackBuff) + (accessory2 is null ? 0 : accessory2.attackBuff); 
        } 
    }

    public override int defense
    {
        get
        {
            return constitution + (weapon is null ? 0 : weapon.defenseBuff) + (armor is null ? 0 : armor.defenseBuff)
                + (accessory1 is null ? 0 : accessory1.defenseBuff) + (accessory2 is null ? 0 : accessory2.defenseBuff);
        }
    }

    public override int magAtk
    {
        get
        {
            return intelligence + (weapon is null ? 0 : weapon.magicAttackBuff) + (armor is null ? 0 : armor.magicAttackBuff)
                + (accessory1 is null ? 0 : accessory1.magicAttackBuff) + (accessory2 is null ? 0 : accessory2.magicAttackBuff);
        }
    }

    public override int magDef
    {
        get
        {
            return spirit + (weapon is null ? 0 : weapon.magicDefenseBuff) + (armor is null ? 0 : armor.magicDefenseBuff)
                + (accessory1 is null ? 0 : accessory1.magicDefenseBuff) + (accessory2 is null ? 0 : accessory2.magicDefenseBuff);
        }
    }
    public override int agility
    {
        get
        {
            return speed + (weapon is null ? 0 : weapon.agilityBuff) + (armor is null ? 0 : armor.agilityBuff)
                + (accessory1 is null ? 0 : accessory1.agilityBuff) + (accessory2 is null ? 0 : accessory2.agilityBuff);
        }
    }
    public override List<Status> resistances 
    { 
        get 
        {
            var statusList = new List<Status>();
            if (accessory1 is not null) statusList.AddRange(accessory1.statusResistances);
            if (accessory2 is not null) statusList.AddRange(accessory2.statusResistances);
            return statusList; 
        } 
    }
    public override List<Status> immunities
    {
        get
        {
            var statusList = new List<Status>();
            if (accessory1 is not null) statusList.AddRange(accessory1.statusImmunities);
            if (accessory2 is not null) statusList.AddRange(accessory2.statusImmunities);
            return statusList;
        }
    }
    public override List<Elements> elemWeaknesses
    {
        get
        {
            var elemList = new List<Elements>();
            if (armor is not null) elemList.AddRange(armor.elemWeaknesses);
            return elemList;
        }
    }
    public override List<Elements> elemResistances
    {
        get
        {
            var elemList = new List<Elements>();
            if (armor is not null) elemList.AddRange(armor.elemResists);
            return elemList;
        }
    }
    public override List<Elements> elemAbsorptions
    {
        get
        {
            var elemList = new List<Elements>();
            if (armor is not null) elemList.AddRange(armor.elemAbsorption);
            return elemList;
        }
    }
    public override int Attack(Character enemy, int turnCounter)
    {
        double enemyAgility = enemy.agility;
        double hitChance = ((agility / enemyAgility) + .01) * 100;

        if (weapon is not null && weapon.element != Elements.NONE && enemy.elemAbsorptions.Where(e => e == weapon.element).Any()) { return -1; }

        if (hitChance >= UnityEngine.Random.Range(0, 100))
        {
            int criticalValue = UnityEngine.Random.Range(1, 20) == 20 ? 2 : 1;
            int damage = (int)((attack * 
                FindPhysicalAttackStatusModifier() * UnityEngine.Random.Range(1f, 1.25f) * criticalValue) - enemy.defense);

            damage = (int)(damage * enemy.FindPhysicalDamageStatusModifier());

            if (weapon is not null)
            {
                if (weapon.element != Elements.NONE)
                    damage = (int)(damage * enemy.FindElementalDamageModifier(weapon.element));

                weapon.ApplyWeaponStatuses(enemy, turnCounter);
            }

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

    public override bool FleeCheck(Character enemy, int turnCounter)
    {
        double playerHpMultiplier = maxHP / currHP;
        double enemyHpMultiplier = enemy.currHP / enemy.maxHP;
        int playerChance = Random.Range(1, 25);

        double playerFleeNum = playerHpMultiplier * (.5 * (luck) + agility) + playerChance;
        double enemyFleeNum = enemyHpMultiplier * (enemy.luck + enemy.agility);

        return playerFleeNum > enemyFleeNum;
    }
}
