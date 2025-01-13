using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCharacterData : CharacterData
{
    public Sprite portrait;
    public ExperienceHandler expHandler;
    public ClassExpHandler classExpHandler;
    public EquipmentWeight weightClass;
    public List<Skill> skills;
    public List<Skill> equippedSkills;
    public List<Class> classes;
    [SerializeReference]
    public Class equippedClass;
    public int excessClassPoints = 0;
    public BattleInventory charInventory;
    public Weapon weapon;
    public Armor armor;
    public Accessory accessory1;
    public Accessory accessory2;
    public bool isInParty;
    public int exp;
    public int skillCount = 12;

    public override void DeepDataCopy(Character c)
    {
        PlayerCharacter pc;
        if (c is PlayerCharacter) pc = c as PlayerCharacter;
        else return;

        base.DeepDataCopy(c);
        expHandler = pc.expHandler;
        classExpHandler = pc.classExpHandler;
        weightClass = pc.weightClass;
        skills = pc.skills;
        equippedSkills = pc.equippedSkills;
        classes = pc.classes;
        equippedClass = pc.currClass;
        charInventory = pc.charInventory;
        weapon = pc.weapon;
        armor = pc.armor;
        accessory1 = pc.accessory1;
        accessory2 = pc.accessory2;
        isInParty = pc.isInParty;
        exp = pc.exp;
        skillCount = pc.skillCount;
        excessClassPoints = pc.excessClassPoints;
    }
}

[System.Serializable]
public class HeroData : PlayerCharacterData { }

[System.Serializable]
public class WizardData : PlayerCharacterData { }

[System.Serializable]
public class SenatorData : PlayerCharacterData { }
