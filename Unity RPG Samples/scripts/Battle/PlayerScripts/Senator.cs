using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senator : PlayerCharacter
{
    public override void OnLevelUp()
    {
        if (level % 5 == 0)
        {
            base.OnLevelUp();
            return;
        }

        maxHP += Random.Range(5, 15);
        maxSP += Random.Range(5, 10);
        strength += Random.Range(0, 2);
        constitution += Random.Range(1, 2);
        intelligence += Random.Range(2, 4);
        spirit += Random.Range(1, 4);
        speed += Random.Range(1, 3);
        luck += Random.Range(0, 5);

        currHP = maxHP;
        currSP = maxSP;
    }
}
