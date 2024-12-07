using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : PlayerCharacter
{

    public override void OnLevelUp()
    {
        if (level % 5 == 0)
        {
            base.OnLevelUp();
            return;
        }

        maxHP += Random.Range(5, 20);
        maxSP += Random.Range(7, 12);
        strength += Random.Range(1, 2);
        constitution += Random.Range(1, 3);
        intelligence += Random.Range(1, 4);
        spirit += Random.Range(1, 3);
        speed += Random.Range(1, 3);
        luck += Random.Range(0, 5);

        currHP = maxHP;
        currSP = maxSP;
    }
}
