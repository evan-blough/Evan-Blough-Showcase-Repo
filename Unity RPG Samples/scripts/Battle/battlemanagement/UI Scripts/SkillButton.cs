using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour
{
    public Button button;
    public Skills skill;
    public PlayerCharacter currentCharacter;
    public UIHandler uiHandler;

    public void SetDetails()
    {
        uiHandler.SetSkillDetails(skill, currentCharacter.currSP.ToString());
    }

    public void RemoveDetails()
    {
        if (button != EventSystem.current.currentSelectedGameObject)
            uiHandler.RemoveSkillDetails();
    }
}
