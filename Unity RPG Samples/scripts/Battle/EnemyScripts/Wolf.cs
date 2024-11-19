using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wolf : Enemy
{
    public override IEnumerator EnemyTurn(List<PlayerCharacter> playerCharacters, List<Enemy> enemies, int turnCounter, BattleStationManager bsm, EnemySkillUI esu)
    {
        List<string> damages = new List<string>();
        List<Character> targets = new List<Character>();
        int selection = Random.Range(1, 100) % 4;
        Character targetUnit = this.FindTarget(playerCharacters.Where(c => c.isActive && c.isActive).ToList());

        if (selection == 0)
        {
            esu.SetText("Maul");
            esu.gameObject.SetActive(true);
            targets.Add(targetUnit);
            damages.Add(Maul(targetUnit, turnCounter).ToString());
            yield return new WaitForSeconds(.35f);

        }
        else
        {
            targets.Add(targetUnit);
            damages.Add(RegularAttack(targetUnit, turnCounter).ToString());
            yield return null;
        }

        for(int i = 0; i < damages.Count && i < targets.Count; i++) bsm.SetText(damages[i], targets[i]);

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

    public int Maul(Character enemy, int turnCounter)
    {
        double charAgility, enemyAgility, criticalValue, hitChance;
        int damage, enemyDefense;

        charAgility = agility;

        PlayerCharacter target = (PlayerCharacter)enemy;
        enemyAgility = target.agility + (target.weapon is null ? 0 : target.weapon.agilityBuff);
        enemyDefense = target.defense + (target.weapon is null ? 0 : target.weapon.defenseBuff);
        
        hitChance = ((charAgility * 2 / enemyAgility) + .01) * 100;

        if (hitChance >= UnityEngine.Random.Range(0, 100))
        {
            if (Random.Range(1, 100) >= 50)
            {
                if (target.currStatuses.Where(s => s.status == Status.BLEED).Any())
                {
                    Statuses status = new Statuses(target.currStatuses.Where(s => s.status == Status.BLEED).First());
                    target.currStatuses.RemoveAll(s => s.status == Status.BLEED);
                    status.expirationTurn += 3;
                    target.currStatuses.Add(status);
                }
                else
                {
                    Statuses status = new Statuses(Status.BLEED, 3, true, .5f);
                }
            }

            criticalValue = UnityEngine.Random.Range(1, 20) >= 18 ? 2 : 1;
            damage = (int)(((attack * 1.5) * FindPhysicalAttackStatusModifier() * UnityEngine.Random.Range(1f, 1.25f) * criticalValue) - (enemyDefense));
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
