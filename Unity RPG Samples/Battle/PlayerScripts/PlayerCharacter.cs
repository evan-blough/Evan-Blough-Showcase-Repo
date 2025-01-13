using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacter : Character
{
    public ExperienceHandler expHandler;
    public ClassExpHandler classExpHandler;
    public EquipmentWeight weightClass;
    public List<Skill> skills;
    public List<Skill> equippedSkills;
    public List<Class> classes;
    [SerializeReference]
    public Class currClass;
    public int excessClassPoints = 0;
    public BattleInventory charInventory;
    public Weapon weapon;
    public Armor armor;
    public Accessory accessory1;
    public Accessory accessory2;
    public bool isInParty;
    public int exp;
    public int skillCount = 12;

    private void Start()
    {
        expHandler.UpdateLevel(this);
        if (!classes.Contains(currClass))
        {
            currClass = classes.Any() ? classes.First() : null;
        }
    }

    public override int attack => strength + (weapon is null ? 0 : weapon.attackBuff) + (armor is null ? 0 : armor.attackBuff)
                + (accessory1 is null ? 0 : accessory1.attackBuff) + (accessory2 is null ? 0 : accessory2.attackBuff)
                + (currClass is null ? 0 : currClass.classAtkMod);

    public override int defense => constitution + (weapon is null ? 0 : weapon.defenseBuff) + (armor is null ? 0 : armor.defenseBuff)
                + (accessory1 is null ? 0 : accessory1.defenseBuff) + (accessory2 is null ? 0 : accessory2.defenseBuff)
                + (currClass is null ? 0 : currClass.classDefMod);

    public override int magAtk => intelligence + (weapon is null ? 0 : weapon.magicAttackBuff) + (armor is null ? 0 : armor.magicAttackBuff)
                + (accessory1 is null ? 0 : accessory1.magicAttackBuff) + (accessory2 is null ? 0 : accessory2.magicAttackBuff)
                + (currClass is null ? 0 : currClass.classMgAtkMod);

    public override int magDef => spirit + (weapon is null ? 0 : weapon.magicDefenseBuff) + (armor is null ? 0 : armor.magicDefenseBuff)
                + (accessory1 is null ? 0 : accessory1.magicDefenseBuff) + (accessory2 is null ? 0 : accessory2.magicDefenseBuff)
                + (currClass is null ? 0 : currClass.classMgDefMod);
    public override int agility => speed + (weapon is null ? 0 : weapon.agilityBuff) + (armor is null ? 0 : armor.agilityBuff)
                + (accessory1 is null ? 0 : accessory1.agilityBuff) + (accessory2 is null ? 0 : accessory2.agilityBuff)
                + (currClass is null ? 0 : currClass.classAgilityMod);

    public override int hitPercent => 100 + (weapon is null ? 0 : weapon.accuracyMod);
    public override int dodgePercent => 0 + (armor is null ? 0 : armor.accuracyMod) + (accessory1 is null ? 0 : accessory1.accuracyMod)
        + (accessory2 is null ? 0 : accessory2.accuracyMod);

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
        double hitChance = hitPercent - enemy.dodgePercent;

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
        classExpHandler = pc.classExpHandler;
        weightClass = pc.weightClass;
        charInventory = pc.charInventory;
        skills = pc.skills;
        equippedSkills = pc.equippedSkills;
        classes = pc.classes;
        currClass = pc.equippedClass;
        weapon = pc.weapon;
        armor = pc.armor;
        accessory1 = pc.accessory1;
        accessory2 = pc.accessory2;
        isInParty = pc.isInParty;
        exp = pc.exp;
        skillCount = pc.skillCount;
        excessClassPoints = pc.excessClassPoints;
    }

    public string EquipSkill(Skill skill)
    {
        if (equippedSkills.Count == skillCount) return "You've equipped the max amount of skills: remove a skill first!";

        equippedSkills.Add(skill);
        return $"Equipped {skill.skillName}";
    }

    public string UnequipSkill(Skill skill)
    {
        equippedSkills.Remove(skill);
        return $"Unequipped {skill.skillName}";
    }

    public void AutoUpdateCurrentClass()
    {
        // pick the first class that hasn't been maxed. If all classes have been maxed, do nothing as there is nothing to do.
        for (int i = 0; i < classes.Count; i++)
        {
            if (!classes[i].isMaxed)
            {
                currClass = classes[i];
                classExpHandler.nextLevelExp = currClass.classSlot.levels[currClass.classLevel - 1].expNeeded;
                break;
            }
        }


    }
}
