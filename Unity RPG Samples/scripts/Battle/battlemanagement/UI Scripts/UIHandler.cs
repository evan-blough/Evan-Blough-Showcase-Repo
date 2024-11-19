using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public DescriptionBox descriptionBox;
    public CostBox costBox;
    public GameObject skillHUD;
    public GameObject commandHUD;
    public GameObject battleHUD;
    public WinLossScript winBox;
    public GameObject lossBox;
    public FleeScript fleeBox;
    public TargetingUI targetingUI;
    public EnemySkillUI enemySkillUI;

    public void OnStart()
    {
        battleHUD.gameObject.SetActive(false);
        skillHUD.gameObject.SetActive(false);
        commandHUD.gameObject.SetActive(false);
        winBox.gameObject.SetActive(false);
        lossBox.gameObject.SetActive(false);
        fleeBox.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        enemySkillUI.gameObject.SetActive(false);
        descriptionBox.gameObject.SetActive(false);
        targetingUI.DeactivateButtons();
    }

    public void ResetUI()
    {
        battleHUD.gameObject.SetActive(false);
        skillHUD.gameObject.SetActive(false);
        commandHUD.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        descriptionBox.gameObject.SetActive(false);
        enemySkillUI.gameObject.SetActive(false);
        targetingUI.DeactivateButtons();
    }

    public void OnLoss()
    {
        lossBox.gameObject.SetActive(true);
    }

    public void OnWin(List<Enemy> enemy, Hero hero, Wizard wizard, Traitor senator)
    {
        winBox.SetWinBox(enemy, hero, wizard, senator);
        winBox.gameObject.SetActive(true);
    }

    public void OnFlee(bool state)
    {
        fleeBox.gameObject.SetActive(state);
    }

    public void UIOnAttack(Character currentCharacter)
    {
        commandHUD.gameObject.SetActive(false);
        skillHUD.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        descriptionBox.gameObject.SetActive(false);
        targetingUI.ActivateTargets(currentCharacter);
    }

    public void UIOnSkills()
    {
        commandHUD.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        descriptionBox.gameObject.SetActive(false);
    }

    public void UIOnCommand()
    {
        commandHUD.gameObject.SetActive(true);
        skillHUD.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        descriptionBox.gameObject.SetActive(false);
    }

    public void SetSkillDetails(Skills skill, string currSP)
    {
        costBox.SetCostBox(skill.skillPointCost.ToString(), currSP);
        descriptionBox.SetDescription(skill.skillDescription);
        descriptionBox.gameObject.SetActive(true);
        costBox.gameObject.SetActive(true);
    }

    public void RemoveSkillDetails()
    {
        costBox.gameObject.SetActive(false);
        descriptionBox.gameObject.SetActive(false);
    }
}
