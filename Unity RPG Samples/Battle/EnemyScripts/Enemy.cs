using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum Rarity { COMMON, UNCOMMON, RARE, LEGENDARY }
public class Enemy : Character
{
    public Elements atkElement;
    public List<Status> enemyStatusResists;
    public List<Status> enemyStatusAbsorb;
    public List<Elements> enemyElemResist;
    public List<Elements> enemyElemAbsorb;
    public List<Elements> enemyElemWeakness;
    public int expValue;
    public int goldValue;
    public int classXpValue;
    public Rarity rarity;
    public override List<Status> resistances { get { return enemyStatusResists; } }
    public override List<Status> immunities { get { return enemyStatusAbsorb; } }
    public override List<Elements> elemWeaknesses { get { return enemyElemWeakness; } }
    public override List<Elements> elemResistances { get { return enemyElemResist; } }
    public override List<Elements> elemAbsorptions { get { return enemyElemAbsorb; } }
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
    public override int Attack(Character enemy, int turnCounter)
    {
        double charAgility, enemyAgility, criticalValue, hitChance;
        int damage, enemyDefense;

        charAgility = agility;
        if (enemy is PlayerCharacter)
        {
            PlayerCharacter target = (PlayerCharacter)enemy;
            enemyAgility = target.agility + (target.weapon is null ? 0 : target.weapon.agilityBuff);
            enemyDefense = target.defense + (target.weapon is null ? 0 : target.weapon.defenseBuff);
        }
        else
        {
            enemyAgility = enemy.agility;
            enemyDefense = enemy.defense;
        }
        hitChance = ((charAgility * 2 / enemyAgility) + .01) * 100;

        if (enemy.elemAbsorptions.Where(e => e == atkElement).Any()) { return -1; }

        if (hitChance >= UnityEngine.Random.Range(0, 100))
        {
            criticalValue = UnityEngine.Random.Range(1, 20) == 20 ? 2 : 1;
            damage = (int)((attack * FindPhysicalAttackStatusModifier() * UnityEngine.Random.Range(1f, 1.25f) * criticalValue) - (enemyDefense));
            damage = (int)(damage / enemy.FindPhysicalDamageStatusModifier());
            if (damage <= 0)
            {
                damage = 1;
            }
            if (damage > 9999) damage = 9999;

            enemy.currHP -= damage;
            if (enemy.currHP < 0) enemy.currHP = 0;
            if (enemy.currHP <= 0) enemy.isActive = false;

            return damage;
        }
        return 0;
    }

    public virtual int RegularAttack(Character targetUnit, int turnCounter)
    {
        int damage = this.Attack(targetUnit, turnCounter);

        if (targetUnit.currHP < 0) { targetUnit.currHP = 0; }

        return damage;
    }

    public virtual IEnumerator EnemyTurn(List<PlayerCharacter> playerCharacters, List<Enemy> enemies, int turnCounter, BattleStationManager bsm, EnemySkillUI esu)
    {
        Character targetUnit = this.FindTarget(playerCharacters.Where(c => c.isActive && c.isActive).ToList());

        int damage = RegularAttack(targetUnit, turnCounter);

        bsm.SetText(damage.ToString(), targetUnit);

        yield return new WaitForSeconds(0.5f);

        bsm.SetText(string.Empty, targetUnit);

        yield return new WaitForSeconds(0.75f);

        if (targetUnit.currHP <= 0)
        {
            targetUnit.isActive = false;
        }
    }

    public virtual Character FindTarget(List<PlayerCharacter> characters)
    {
        List<PlayerCharacter> target = new List<PlayerCharacter>();

        if (characters.Count == 1) return characters.First();

        if (characters.Where(c => c.currHP < (.25 * c.maxHP)).Count() > 0)
        {
            target = characters.Where(c => c.currHP < (.25 * c.maxHP)).ToList();

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
