using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ogre : Enemy
{
    public override IEnumerator EnemyTurn(List<PlayerCharacter> playerCharacters, List<Enemy> enemies, int turnCounter, BattleStationManager bsm, EnemySkillUI esu)
    {
        List<string> damages = new List<string>();
        List<Character> targets = new List<Character>();
        int selection = Random.Range(1, 100) % 4;
        Character targetUnit = this.FindTarget(playerCharacters.Where(c => c.isActive).ToList());

        if (selection == 0 || isBackRow)
        {
            if (playerCharacters.Any(c => c.isBackRow && c.isActive) && !isBackRow) targetUnit = playerCharacters.Where(c => c.isBackRow).First();

            esu.SetText("Club Throw");
            esu.gameObject.SetActive(true);
            targets.Add(targetUnit);
            damages.Add(ClubThrow(targetUnit).ToString());
            yield return new WaitForSeconds(.35f);

        }
        else
        {
            targets.Add(targetUnit);
            damages.Add(RegularAttack(targetUnit, turnCounter).ToString());
            yield return null;
        }

        for (int i = 0; i < damages.Count && i < targets.Count; i++) bsm.SetText(damages[i], targets[i]);

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < targets.Count; i++) bsm.SetText(string.Empty, targets[i]);
        esu.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.75f);

        foreach (Character target in targets)
        {
            if (target.currHP <= 0)
            {
                target.isActive = false;
            }
        }
    }

    public int ClubThrow(Character enemy)
    {

        double charAgility, enemyAgility, criticalValue, hitChance;
        int damage, enemyDefense;

        charAgility = agility;

        PlayerCharacter target = (PlayerCharacter)enemy;
        enemyAgility = target.agility + (target.weapon is null ? 0 : target.weapon.agilityBuff);
        enemyDefense = target.defense + (target.weapon is null ? 0 : target.weapon.defenseBuff);

        hitChance = ((charAgility * 3 / enemyAgility) + .01) * 100;

        if (hitChance >= UnityEngine.Random.Range(0, 100))
        {
            criticalValue = UnityEngine.Random.Range(1, 20) == 20 ? 2 : 1;
            damage = (int)(((attack * 2) * FindPhysicalAttackStatusModifier() * UnityEngine.Random.Range(1f, 1.25f) * criticalValue) - (enemyDefense));
            damage = (int)(damage / enemy.FindPhysicalDamageStatusModifier());
            if (damage <= 0)
            {
                damage = 1;
            }

            enemy.currHP -= damage;
            if (enemy.currHP < 0) enemy.currHP = 0;

            return damage;
        }
        return 0;
    }
}
