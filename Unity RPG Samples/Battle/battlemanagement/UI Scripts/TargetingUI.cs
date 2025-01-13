using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetingUI : MonoBehaviour
{
    public Button allHeroes;
    public Button allEnemies;
    public SkillsHUD skillsHUD;
    public ItemsUI itemsUI;
    public BattleStateMachine bsm;

    public void FindTarget(Character target)
    {
        if (skillsHUD.currentSkill != null)
        {
            List<Character> list = new List<Character>();
            list.Add(target);
            StartCoroutine(skillsHUD.OnTargetSelected(list));
            return;
        }
        else if (itemsUI.currentItem != null)
        {
            List<Character> list = new List<Character>();
            list.Add(target);
            StartCoroutine(itemsUI.OnTargetSelected(list));
            return;
        }
        StartCoroutine(bsm.PlayerAttack(target));
        return;
    }

    public void FindMultiTarget(List<Character> targets)
    {
        if (skillsHUD.currentSkill != null)
        {
            StartCoroutine(skillsHUD.OnTargetSelected(targets));
            return;
        }
        else if (itemsUI.currentItem != null)
        {
            StartCoroutine(itemsUI.OnTargetSelected(targets));
            return;
        }

        foreach (Character target in targets) bsm.currentCharacter.Attack(target, bsm.turnCounter);
        return;
    }

    // normal target activation
    public void ActivateTargets()
    {
        allHeroes.interactable = false;
        allEnemies.interactable = false;

        var buttons = allHeroes.GetComponentsInChildren<Button>().Where(b => b != allHeroes).ToArray();
        var characters = allHeroes.GetComponentsInChildren<CharacterHUD>();
        var enemyButtons = allEnemies.GetComponentsInChildren<Button>().Where(b => b != allEnemies).ToArray();
        var enemies = allEnemies.GetComponentsInChildren<EnemyHUD>();

        for (int i = 0; i < characters.Length && i < buttons.Length; ++i)
        {
            if (characters[i].character.isActive)
                buttons[i].interactable = true;
        }

        for (int i = 0; i < enemyButtons.Length && i < enemies.Length; ++i)
        {
            int frontRowEnemies = enemies.Where(e => e.enemy.isActive && !e.enemy.isBackRow).Count();

            if (enemies[i].enemy.isActive && (frontRowEnemies == 0 || !enemies[i].enemy.isBackRow))
                enemyButtons[i].interactable = true;
        }
    }

    // activating targets for a skill
    public void ActivateTargets(Skill skill)
    {
        var buttons = allHeroes.GetComponentsInChildren<Button>().Where(b => b != allHeroes).ToArray();
        var characters = allHeroes.GetComponentsInChildren<CharacterHUD>();

        var enemyButtons = allEnemies.GetComponentsInChildren<Button>().Where(b => b != allEnemies).ToArray();
        var enemies = allEnemies.GetComponentsInChildren<EnemyHUD>();

        if (skill is RevivalSkill)
        {
            if (skill.isMultiTargeted)
            {
                allHeroes.interactable = true;
                allEnemies.interactable = true;

                for (int i = 0; i < buttons.Length && i < characters.Length; ++i) buttons[i].interactable = false;
                for (int i = 0; i < enemies.Length && i < enemyButtons.Length; ++i) enemyButtons[i].interactable = false;
            }
            else
            {
                allHeroes.interactable = false;
                allEnemies.interactable = false;

                for (int i = 0; i < buttons.Length && i < characters.Length; ++i)
                {
                    if (!characters[i].character.isActive)
                        buttons[i].interactable = true;
                }
                for (int i = 0; i < enemies.Length && i < enemyButtons.Length; ++i)
                {
                    if (!enemies[i].enemy.isActive)
                        enemyButtons[i].interactable = true;
                }
            }
        }
        else if (skill.isMultiTargeted)
        {
            allHeroes.interactable = true;
            allEnemies.interactable = true;

            for (int i = 0; i < buttons.Length && i < characters.Length; ++i) buttons[i].interactable = false;
            for (int i = 0; i < enemies.Length && i < enemyButtons.Length; ++i) enemyButtons[i].interactable = false;
        }
        else if (skill.isRanged)
        {
            allHeroes.interactable = false;
            allEnemies.interactable = false;

            for (int i = 0; i < buttons.Length && i < characters.Length; ++i)
            {
                if (characters[i].character.isActive)
                    buttons[i].interactable = true;
            }

            for (int i = 0; i < enemyButtons.Length && i < enemies.Length; ++i)
            {
                if (enemies[i].enemy.isActive)
                    enemyButtons[i].interactable = true;
            }
        }
        else
        {
            allHeroes.interactable = false;
            allEnemies.interactable = false;

            for (int i = 0; i < buttons.Length && i < characters.Length; ++i)
            {
                if (characters[i].character.isActive)
                    buttons[i].interactable = true;
            }

            int frontRowEnemies = enemies.Where(e => e.enemy.isActive && !e.enemy.isBackRow).Count();

            for (int i = 0; i < enemyButtons.Length && i < enemies.Length; ++i)
            {
                if (enemies[i].enemy.isActive && (frontRowEnemies == 0 || !enemies[i].enemy.isBackRow))
                    enemyButtons[i].interactable = true;
            }
        }
    }

    // activating targets for an item
    public void ActivateTargets(BattleItem item)
    {
        var buttons = allHeroes.GetComponentsInChildren<Button>().Where(b => b != allHeroes).ToArray();
        var characters = allHeroes.GetComponentsInChildren<CharacterHUD>();

        var enemyButtons = allEnemies.GetComponentsInChildren<Button>().Where(b => b != allEnemies).ToArray();
        var enemies = allEnemies.GetComponentsInChildren<EnemyHUD>();

        if (item is ReviveItem)
        {
            if (item.isMultiTargeted)
            {
                allHeroes.interactable = true;
                allEnemies.interactable = true;

                for (int i = 0; i < buttons.Length && i < characters.Length; ++i) buttons[i].interactable = false;
                for (int i = 0; i < enemies.Length && i < enemyButtons.Length; ++i) enemyButtons[i].interactable = false;
            }
            else
            {
                allHeroes.interactable = false;
                allEnemies.interactable = false;

                for (int i = 0; i < buttons.Length && i < characters.Length; ++i)
                {
                    if (!characters[i].character.isActive)
                        buttons[i].interactable = true;
                }
                for (int i = 0; i < enemies.Length && i < enemyButtons.Length; ++i)
                {
                    if (!enemies[i].enemy.isActive)
                        enemyButtons[i].interactable = true;
                }
            }

        }
        if (item.isMultiTargeted)
        {
            allHeroes.interactable = true;
            allEnemies.interactable = true;

            for (int i = 0; i < buttons.Length && i < characters.Length; ++i) buttons[i].interactable = false;
            for (int i = 0; i < enemies.Length && i < enemyButtons.Length; ++i) enemyButtons[i].interactable = false;
        }
        else
        {
            allHeroes.interactable = false;
            allEnemies.interactable = false;

            for (int i = 0; i < buttons.Length && i < characters.Length; ++i)
            {
                if (characters[i].character.isActive)
                    buttons[i].interactable = true;
            }

            for (int i = 0; i < enemyButtons.Length && i < enemies.Length; ++i)
            {
                if (enemies[i].enemy.isActive)
                    enemyButtons[i].interactable = true;
            }
        }
    }
    public void DeactivateButtons()
    {
        allHeroes.interactable = false;
        allEnemies.interactable = false;
        foreach (Button button in allHeroes.GetComponentsInChildren<Button>()) button.interactable = false;
        foreach (Button button in allEnemies.GetComponentsInChildren<Button>()) button.interactable = false;
    }
}
