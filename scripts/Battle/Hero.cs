using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public string unitName;
    public int maxHP;
    public int currHP;
    public int attack;
    public int defense;
    public int luck;
    public int agility;
    public int magAtk;
    public int magDef;
    public List<string> skills;
    public int exp;
    public int level;

    public bool TakeDamage(int attack)
    {
        currHP -= (attack * 2) - defense;

        if(currHP <= 0 )
        {
            currHP = 0;
            return true;
        }
        return false;
    }

}
