using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public DescriptionBox skillDescriptionBox;
    public DescriptionBox itemDescriptionBox;
    public CostBox costBox;
    public GameObject skillHUD;
    public GameObject commandHUD;
    public GameObject battleHUD;
    public WinLossScript winBox;
    public GameObject lossBox;
    public FleeScript fleeBox;
    public TargetingUI targetingUI;
    public EnemySkillUI enemySkillUI;
    public GameObject itemsUI;

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
        itemsUI.gameObject.SetActive(false);
        skillDescriptionBox.gameObject.SetActive(false);
        itemDescriptionBox.gameObject.SetActive(false);
        targetingUI.DeactivateButtons();
    }

    public void ResetUI()
    {
        battleHUD.gameObject.SetActive(false);
        itemsUI.gameObject.SetActive(false);
        skillHUD.gameObject.SetActive(false);
        commandHUD.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        skillDescriptionBox.gameObject.SetActive(false);
        itemDescriptionBox.gameObject.SetActive(false);
        enemySkillUI.gameObject.SetActive(false);
        targetingUI.DeactivateButtons();
    }

    public void OnLoss()
    {
        lossBox.gameObject.SetActive(true);
    }

    public void OnWin(List<Enemy> enemy, List<PlayerCharacter> playerCharacterList)
    {
        winBox.SetWinBox(enemy, playerCharacterList);
        winBox.gameObject.SetActive(true);
    }

    public void OnFlee(bool state)
    {
        fleeBox.gameObject.SetActive(state);
    }

    public void UIOnAttack()
    {
        commandHUD.gameObject.SetActive(false);
        itemsUI.gameObject.SetActive(false);
        skillHUD.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        skillDescriptionBox.gameObject.SetActive(false);
        itemDescriptionBox.gameObject.SetActive(false);
        targetingUI.ActivateTargets();
    }

    public void UIOnSkills()
    {
        commandHUD.gameObject.SetActive(false);
        itemsUI.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        skillDescriptionBox.gameObject.SetActive(false);
        itemDescriptionBox.gameObject.SetActive(false);
    }

    public void UIOnCommand()
    {
        commandHUD.gameObject.SetActive(true);
        skillHUD.gameObject.SetActive(false);
        itemsUI.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        skillDescriptionBox.gameObject.SetActive(false);
        itemDescriptionBox.gameObject.SetActive(false);
    }

    public void UIOnItems()
    {
        itemsUI.gameObject.SetActive(true);
        skillHUD.gameObject.SetActive(false);
        costBox.gameObject.SetActive(false);
        skillDescriptionBox.gameObject.SetActive(false);
        itemDescriptionBox.gameObject.SetActive(false);
    }

    public void SetSkillDetails(Skill skill, string currSP)
    {
        costBox.SetCostBox(skill.skillPointCost.ToString(), currSP);
        skillDescriptionBox.SetDescription(skill.skillDescription);
        skillDescriptionBox.gameObject.SetActive(true);
        costBox.gameObject.SetActive(true);
    }

    public void RemoveSkillDetails()
    {
        costBox.gameObject.SetActive(false);
        skillDescriptionBox.gameObject.SetActive(false);
    }

    public void SetItemDetails(Items item)
    {
        itemDescriptionBox.SetDescription(item.itemDescription);
        itemDescriptionBox.gameObject.SetActive(true);
    }
    public void RemoveItemDetails()
    {
        itemDescriptionBox.gameObject.SetActive(false);
    }
}
