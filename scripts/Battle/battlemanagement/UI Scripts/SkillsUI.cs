using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
public class SkillsHUD : MonoBehaviour
{
    public GameObject buttonPrefab;
    public BattleStateMachine bsm;
    public Skills currentSkill;
    public void CallSkillsHUD()
    {
        bsm.commandHUD.SetActive(false);

        List<Skills> skills = new List<Skills>();   

        foreach (Transform child in bsm.skillHUD.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Skills skill in bsm.currentCharacter.skills)
        {
            if (skill.levelReq <= bsm.currentCharacter.level && skill.active)
            {
                skills.Add(skill);
                GameObject tempButton = Instantiate(buttonPrefab);
                tempButton.transform.SetParent(bsm.skillHUD.transform, false);

                Button skillButton = tempButton.GetComponentInChildren<Button>();
                skillButton.onClick.AddListener(() => OnSkillButton(skill));

                Text tempText = tempButton.GetComponentInChildren<Text>();
                tempText.text = skill.skillName;

                if ((!skill.isRanged && bsm.currentCharacter.isBackRow) || skill.skillPointCost > bsm.currentCharacter.currSP) skillButton.interactable = false;
            }
        }

        bsm.skillHUD.gameObject.SetActive(true);
    }

    public void OnSkillButton(Skills skill)
    {
        currentSkill = skill;
        bsm.targetingUI.ActivateTargets(skill, bsm.currentCharacter);
    }

    public IEnumerator OnTargetSelected(List<Character> targets)
    {
        List<string> returns;
        bsm.ResetUI();

        bsm.currentCharacter.currSP -= currentSkill.skillPointCost;
        var statusReturns = currentSkill.HandleStatusApplication(bsm.currentCharacter, targets, bsm.turnCounter);
        switch (currentSkill.type)
        {
            case SkillType.ATTACK:
                returns = currentSkill.UseAttackingSkill(bsm.currentCharacter, targets, bsm.turnCounter);
                yield return StartCoroutine(ApplyDamage(targets, returns, currentSkill.type));
                break;
            case SkillType.HEAL:
                returns = currentSkill.UseHealingSkill(bsm.currentCharacter, targets, bsm.turnCounter);
                yield return StartCoroutine(ApplyDamage(targets, returns, currentSkill.type));
                break;
            case SkillType.STATUS:
                yield return StartCoroutine(ApplyDamage(targets, statusReturns, currentSkill.type));
                break;
            default:
                currentSkill.UseMixedSkill(bsm.currentCharacter, targets);
                break;
        }
        currentSkill = null;
        
        StartCoroutine(bsm.FindNextTurn());
    }

    public IEnumerator ApplyDamage(List<Character> targets, List<string> returns, SkillType type)
    {
        HandleSkillText(targets, returns, type);
        yield return new WaitForSeconds(.55f);

        for (int i = 0; i < returns.Count; i++) returns[i] = string.Empty;
        HandleSkillText(targets, returns, type);
        yield return new WaitForSeconds(.75f);
    }

    public void HandleSkillText(List<Character> target, List<string> text, SkillType type)
    {
        Color color = (type == SkillType.HEAL ? Color.green : Color.white);

        bsm.SetTextColor(color);

        for (int i = 0; i < text.Count && i < target.Count; i++)
        {
            if (target[i].gameObject.transform.parent == bsm.topBattleStation)
            {
                bsm.upperStationDamage.text = text[i];
            }
            if (target[i].gameObject.transform.parent == bsm.lowerBattleStation)
            {
                bsm.lowerStationDamage.text = text[i];
            }
            if (target[i].gameObject.transform.parent == bsm.backRowStation)
            {
                bsm.backRowDamage.text = text[i];
            }
            if (target[i] is Enemy)
            {
                bsm.enemyDamage.text = text[i];
            }
        }

        bsm.SetTextColor(Color.white);
    }
}


