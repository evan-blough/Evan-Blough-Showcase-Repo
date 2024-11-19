using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CommandUI : MonoBehaviour
{
    public BattleStateMachine bsm;
    public Button recoverButton;
    public Button swapPositionsButton;

    public void CommandButtonClick()
    {
        bsm.uiHandler.UIOnCommand();
        swapPositionsButton.gameObject.SetActive(!bsm.currentCharacter.isBackRow);
        recoverButton.gameObject.SetActive(bsm.currentCharacter.isBackRow);
    }

    public void OnRecoverButton()
    {
        bsm.uiHandler.ResetUI();

        bsm.currentCharacter.currHP += (bsm.currentCharacter.maxHP / 10) + 1;
        bsm.currentCharacter.currSP += (bsm.currentCharacter.maxSP / 10) + 1;

        if (bsm.currentCharacter.currHP > bsm.currentCharacter.maxHP) bsm.currentCharacter.currHP = bsm.currentCharacter.maxHP;
        if (bsm.currentCharacter.currSP > bsm.currentCharacter.maxSP) bsm.currentCharacter.currSP = bsm.currentCharacter.maxSP;

        if (bsm.currentCharacter.currStatuses.Count > 0)
            bsm.currentCharacter.currStatuses.RemoveAll(s => s.status == Status.POISONED || s.status == Status.BLEED || s.status == Status.VULNERABLE);

        StartCoroutine(bsm.FindNextTurn());
    }

    public void OnSwapPositionsButton()
    {
        bsm.uiHandler.ResetUI();

        if (bsm.playerCharacterList.Where(c => c.isBackRow).FirstOrDefault() is null)
        {
            bsm.currentCharacter.transform.SetParent(bsm.battleStationManager.backBattleStation);
        }
        else
        {
            bsm.battleStationManager.SwapStations(bsm.currentCharacter);
        }

        StartCoroutine(bsm.FindNextTurn());
    }

    public void OnDefendButton()
    {
        bsm.uiHandler.ResetUI();
        Statuses status = new Statuses(Status.DEFENDING, bsm.turnCounter + 1);
        if (bsm.currentCharacter.currStatuses.Where(s => s.status == Status.DEFENDING).Any())
            bsm.currentCharacter.currStatuses.First(s => s.status == Status.DEFENDING).expirationTurn = bsm.turnCounter + 1;
        else
            bsm.currentCharacter.currStatuses.Add(status);

        StartCoroutine(bsm.FindNextTurn());
    }
    public void OnItemsButton()
    {
        bsm.uiHandler.ResetUI();
        StartCoroutine(bsm.FindNextTurn());
    }
}
