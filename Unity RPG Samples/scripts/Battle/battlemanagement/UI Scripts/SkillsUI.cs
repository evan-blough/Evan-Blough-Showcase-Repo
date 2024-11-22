using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillsHUD : MonoBehaviour
{
    public GameObject buttonPrefab;
    public BattleStateMachine bsm;
    public Skills currentSkill;
    public DescriptionBox descriptionBox;
    public CostBox costBox;
    public void CallSkillsHUD()
    {
        bsm.uiHandler.UIOnSkills();

        List<Skills> skills = new List<Skills>();   

        foreach (Transform child in bsm.uiHandler.skillHUD.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Skills skill in ((PlayerCharacter)bsm.currentCharacter).skills)
        {
            if (skill.levelReq <= bsm.currentCharacter.level && skill.active)
            {
                skills.Add(skill);
                GameObject tempButton = Instantiate(buttonPrefab);
                tempButton.transform.SetParent(bsm.uiHandler.skillHUD.transform, false);
                var s = tempButton.GetComponent<SkillButton>();
                s.skill = skill;
                s.currentCharacter = (PlayerCharacter)bsm.currentCharacter;
                s.uiHandler = bsm.uiHandler;

                Button skillButton = tempButton.GetComponentInChildren<Button>();
                skillButton.onClick.AddListener(() => OnSkillButton(skill));
                Text tempText = tempButton.GetComponentInChildren<Text>();
                tempText.text = skill.skillName;

                if ((!skill.isRanged && bsm.currentCharacter.isBackRow) || skill.skillPointCost > bsm.currentCharacter.currSP) skillButton.interactable = false;
            }
        }

        bsm.uiHandler.skillHUD.gameObject.SetActive(true);

    }

    public void OnSkillButton(Skills skill)
    {
        currentSkill = skill;
        bsm.uiHandler.targetingUI.ActivateTargets(skill, bsm.currentCharacter);
    }

    public IEnumerator OnTargetSelected(List<Character> targets)
    {
        List<string> returns;
        bsm.uiHandler.ResetUI();

        bsm.currentCharacter.currSP -= currentSkill.skillPointCost;
        switch (currentSkill.type)
        {
            case SkillType.ATTACK:
                returns = currentSkill.UseAttackingSkill((PlayerCharacter)bsm.currentCharacter, targets, bsm.turnCounter);
                break;
            case SkillType.HEAL:
                returns = currentSkill.UseHealingSkill((PlayerCharacter)bsm.currentCharacter, targets, bsm.turnCounter);
                break;
            case SkillType.STATUS:
                returns = currentSkill.HandleStatusApplication((PlayerCharacter)bsm.currentCharacter, targets, bsm.turnCounter);
                break;
            default:
                returns = currentSkill.UseMixedSkill((PlayerCharacter)bsm.currentCharacter, targets, bsm.turnCounter);
                break;
        }
        yield return StartCoroutine(ApplyDamage(targets, returns, currentSkill.type));
        currentSkill = null;
        
        StartCoroutine(bsm.FindNextTurn());
    }

    public IEnumerator ApplyDamage(List<Character> targets, List<string> returns, SkillType type)
    {
        HandleSkillText(targets, returns, type);
        yield return new WaitForSeconds(.55f);

        for (int i = 0; i < returns.Count; i++) returns[i] = string.Empty;
        HandleSkillText(targets, returns, SkillType.ATTACK);
        yield return new WaitForSeconds(.75f);
    }

    public void HandleSkillText(List<Character> target, List<string> text, SkillType type)
    {
        Color color = ((type == SkillType.HEAL || type == SkillType.REVIVE) ? Color.green : Color.white);

        bsm.battleStationManager.SetTextColor(color);

        for (int i = 0; i < text.Count && i < target.Count; i++)
        {
            bsm.battleStationManager.SetText(text[i], target[i]);
        }
    }
}


