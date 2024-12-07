using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : PlayerCharacter
{
    public override int attack
    {
        get
        {
            if (weapon is null) return strength + level * 2;

            return strength + (weapon is null ? 0 : weapon.attackBuff) + (armor is null ? 0 : armor.attackBuff)
                + (accessory1 is null ? 0 : accessory1.attackBuff) + (accessory2 is null ? 0 : accessory2.attackBuff);
        }
    }
    public override int agility
    {
        get
        {
            if (armor is null && accessory1 is null && accessory2 is null) return speed + 20 + (level * 2);

            return speed + (weapon is null ? 0 : weapon.agilityBuff) + (armor is null ? 0 : armor.agilityBuff)
                + (accessory1 is null ? 0 : accessory1.agilityBuff) + (accessory2 is null ? 0 : accessory2.agilityBuff);
        }
    }
    public override void OnLevelUp()
    {
        if (level % 5 == 0)
        {
            base.OnLevelUp();
            return;
        }


        maxHP += Random.Range(8, 20);
        maxSP += Random.Range(3, 7);
        strength += Random.Range(2, 4);
        constitution += Random.Range(1, 4);
        intelligence += Random.Range(0, 2);
        spirit += Random.Range(1, 2);
        speed += Random.Range(1, 3);
        luck += Random.Range(0, 5);

        currHP = maxHP;
        currSP = maxSP;
    }
}
