using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillsHUD : MonoBehaviour
{
    public GameObject buttonPrefab;
    public BattleStateMachine bsm;
    public Skill currentSkill;
    public DescriptionBox descriptionBox;
    public CostBox costBox;
    Character currentCharacter;
    public int turnCounter;
    public void CallSkillsHUD()
    {
        bsm.uiHandler.UIOnSkills();

        currentCharacter = bsm.currentCharacter;
        turnCounter = bsm.turnCounter;

        List<Skill> skills = new List<Skill>();

        foreach (Transform child in bsm.uiHandler.skillHUD.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Skill skill in ((PlayerCharacter)currentCharacter).equippedSkills)
        {
            skills.Add(skill);
            GameObject tempButton = Instantiate(buttonPrefab);
            tempButton.transform.SetParent(bsm.uiHandler.skillHUD.transform, false);
            var s = tempButton.GetComponent<SkillButton>();
            s.skill = skill;
            s.currentCharacter = (PlayerCharacter)currentCharacter;
            s.uiHandler = bsm.uiHandler;

            Button skillButton = tempButton.GetComponentInChildren<Button>();
            skillButton.onClick.AddListener(() => OnSkillButton(skill));
            Text tempText = tempButton.GetComponentInChildren<Text>();
            tempText.text = skill.skillName;

            if ((!skill.isRanged && currentCharacter.isBackRow) || skill.skillPointCost > currentCharacter.currSP) skillButton.interactable = false;

        }

        bsm.uiHandler.skillHUD.gameObject.SetActive(true);

    }

    public void OnSkillButton(Skill skill)
    {
        currentSkill = skill;
        bsm.uiHandler.targetingUI.ActivateTargets(skill);
    }

    public IEnumerator OnTargetSelected(List<Character> targets)
    {
        List<string> returns;
        bsm.uiHandler.ResetUI();

        currentCharacter.currSP -= currentSkill.skillPointCost;

        returns = currentSkill.UseSkill(currentCharacter, targets, turnCounter);

        yield return StartCoroutine(ApplyDamage(targets, returns));
        currentSkill = null;
        StartCoroutine(bsm.FindNextTurn());
    }

    public IEnumerator ApplyDamage(List<Character> targets, List<string> returns)
    {
        HandleSkillText(targets, returns, (currentSkill is HealingSkill || currentSkill is RevivalSkill));
        yield return new WaitForSeconds(.55f);

        for (int i = 0; i < returns.Count; i++) returns[i] = string.Empty;
        HandleSkillText(targets, returns, false);
        yield return new WaitForSeconds(.75f);
    }

    public void HandleSkillText(List<Character> target, List<string> text, bool isHealing)
    {
        Color color = isHealing ? Color.green : Color.white;

        bsm.battleStationManager.SetTextColor(color);

        for (int i = 0; i < text.Count && i < target.Count; i++)
        {
            bsm.battleStationManager.SetText(text[i], target[i]);
        }
    }
}


