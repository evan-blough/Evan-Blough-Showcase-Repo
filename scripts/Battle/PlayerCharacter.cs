using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public int gold;
    public bool isInParty;
    public int exp;

    public override bool FleeCheck(Character enemy)
    {
        float playerHpMultiplier = maxHP / currHP;
        float enemyHpMultiplier = enemy.currHP / enemy.maxHP;
        int playerChance = Random.Range(1, 25);
        int playerFleeNum = (playerHpMultiplier * (luck + agility + (level * 5))) * playerChance == 25 ? 999 : playerChance;
        int enemyFleeNum = (int)(enemyHpMultiplier * (enemy.luck + enemy.agility + (level * 5))) * Random.Range(1, 16);
        return playerFleeNum > enemyFleeNum;
    }
}
