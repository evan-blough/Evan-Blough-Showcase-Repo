using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
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
    public int expValue;
    public int goldValue;
    public int level;
    public Rarity rarity;

    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        LEGENDARY
    }
    public static Rarity InitializeRarity()
    {
        System.Random random = new System.Random();
        var enumNum = random.Next(1, 4);

        if (enumNum == 1) return Rarity.COMMON;

        else if (enumNum == 2) return Rarity.UNCOMMON;

        else if (enumNum == 3) return Rarity.RARE;

        else if (enumNum == 4) return Rarity.LEGENDARY;

        throw new Exception("Error generating enemy rarity");
    }
    public bool TakeDamage(int attack)
    {
        currHP -= (attack * 2) - defense;

        if (currHP <= 0)
        {
            currHP = 0;
            return true;
        }
        return false;
    }
}
