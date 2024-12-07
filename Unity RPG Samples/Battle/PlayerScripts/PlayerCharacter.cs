using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class PlayerCharacter : Character
{
    public ExperienceHandler expHandler;
    public EquipmentWeight weightClass;
    public List<Skills> skills;
    public BattleInventory charInventory;
    public Weapon weapon;
    public Armor armor;
    public Accessory accessory1;
    public Accessory accessory2;
    public bool isInParty;
    public int exp;

    private void Start()
    {
        expHandler.UpdateLevel(this);
    }

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
            if (damage > 9999) damage = 9999;

            enemy.currHP -= damage;
            if (enemy.currHP < 0) enemy.currHP = 0;

            return damage;
        }
        return 0;
    }

    public virtual void OnLevelUp()
    {
        maxHP += 20;
        maxSP += 10;
        strength += 3;
        constitution += 3;
        intelligence += 3;
        spirit += 3;
        speed += 3;
        luck += 3;

        currHP = maxHP;
        currSP = maxSP;
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

    public override void DeepCopyFrom(CharacterData c)
    {
        PlayerCharacterData pc;
        if (c is PlayerCharacterData) pc = c as PlayerCharacterData;
        else return;

        base.DeepCopyFrom(c);
        expHandler = pc.expHandler;
        weightClass = pc.weightClass;
        charInventory = pc.charInventory;
        skills = pc.skills.ConvertAll(x => new Skills(x)).ToList();
        weapon = pc.weapon;
        armor = pc.armor;
        accessory1 = pc.accessory1;
        accessory2 = pc.accessory2;
        isInParty = pc.isInParty;
        exp = pc.exp;
    }

    public string EquipSkill(Skills skill)
    {
        if (skills.Count == 12) return "You've equipped the max amount of skills: remove a skill first!";

        skill.isEquipped = true;
        return $"Equipped {skill.skillName}";
    }

    public string UnequipSkill(Skills skill)
    {
        skill.isEquipped = false;
        return $"Unequipped {skill.skillName}";
    }
}
