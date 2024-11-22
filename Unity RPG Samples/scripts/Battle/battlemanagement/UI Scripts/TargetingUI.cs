using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
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
            StartCoroutine (itemsUI.OnTargetSelected(targets));
            return;
        }

        foreach (Character target in targets) bsm.currentCharacter.Attack(target, bsm.turnCounter);
        return;
    }

    // normal target activation
    public void ActivateTargets(Character character)
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
            if (enemies[i].enemy.isActive && (enemies.Where(e => e.enemy.isActive).ToList().Count == enemies.Where(e => e.enemy.isBackRow).ToList().Count 
                                            || !enemies[i].enemy.isBackRow))
                enemyButtons[i].interactable = true;
        }
    }

    // activating targets for a skill
    public void ActivateTargets(Skills skill, Character character)
    {
        var buttons = allHeroes.GetComponentsInChildren<Button>().Where(b => b != allHeroes).ToArray();
        var characters = allHeroes.GetComponentsInChildren<CharacterHUD>();

        var enemyButtons = allEnemies.GetComponentsInChildren<Button>().Where(b => b != allEnemies).ToArray();
        var enemies = allEnemies.GetComponentsInChildren<EnemyHUD>();

        if (skill.isMultiTargeted)
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

            for (int i = 0; i < enemyButtons.Length && i < enemies.Length; ++i)
            {
                if (enemies[i].enemy.isActive && (enemies.Length == 1 || !enemies[i].enemy.isBackRow))
                    enemyButtons[i].interactable = true;
            }
        }
    }

    // activating targets for an item
    public void ActivateTargets(Items item, Character character)
    {
        var buttons = allHeroes.GetComponentsInChildren<Button>().Where(b => b != allHeroes).ToArray();
        var characters = allHeroes.GetComponentsInChildren<CharacterHUD>();

        var enemyButtons = allEnemies.GetComponentsInChildren<Button>().Where(b => b != allEnemies).ToArray();
        var enemies = allEnemies.GetComponentsInChildren<EnemyHUD>();

        if (item.itemEffect.isMultiTargeted)
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
