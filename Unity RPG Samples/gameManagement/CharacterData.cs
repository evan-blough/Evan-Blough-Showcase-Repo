using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CharacterData
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

    public virtual void DeepDataCopy(Character c)
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
