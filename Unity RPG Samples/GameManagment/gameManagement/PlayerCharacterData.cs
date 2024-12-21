using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PlayerCharacterData : CharacterData
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

    public override void DeepDataCopy(Character c)
    {
        PlayerCharacter pc;
        if (c is PlayerCharacter) pc = c as PlayerCharacter;
        else return;

        base.DeepDataCopy(c);
        expHandler = pc.expHandler;
        weightClass = pc.weightClass;
        skills = pc.skills.ConvertAll(cs => new Skills(cs)).ToList();
        charInventory = pc.charInventory;
        weapon = pc.weapon;
        armor = pc.armor;
        accessory1 = pc.accessory1;
        accessory2 = pc.accessory2;
        isInParty = pc.isInParty;
        exp = pc.exp;
    }
}

[System.Serializable]
public class HeroData : PlayerCharacterData { }

[System.Serializable]
public class WizardData : PlayerCharacterData { }

[System.Serializable]
public class SenatorData : PlayerCharacterData { }
