using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Linq;
public class Enemy : Character
{
    public int expValue;
    public int goldValue;
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

    public virtual Character FindTarget(List<PlayerCharacter> characters)
    {
        List<PlayerCharacter> target = new List<PlayerCharacter>();

        if (characters.Count == 1) return characters.First();

        if (characters.Where(c => c.currHP < (.25*c.maxHP)).Count() > 0)
        {
            target = characters.Where(c => c.currHP < (.25*c.maxHP)).ToList();

            if (target.Where(c => !c.isBackRow).Count() > 0) 
            {
                target = target.Where(c => !c.isBackRow).ToList();
                int random = UnityEngine.Random.Range(0, target.Count() - 1);
                return target[random];
            }
        }
        while (true)
        {
            int random = UnityEngine.Random.Range(0, characters.Count() - 1);

            if (!characters[random].isBackRow) return characters[random];
        }
    }
}
