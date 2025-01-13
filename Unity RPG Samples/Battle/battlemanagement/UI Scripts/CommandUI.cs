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

        if (bsm.playerCharacterList.Any(pc => !pc.isActive))
            swapPositionsButton.gameObject.SetActive(false);
        else
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
            bsm.currentCharacter.currStatuses.RemoveAll(s => s.status == Status.Poisoned || s.status == Status.Bleeding || s.status == Status.Vulnerable);

        StartCoroutine(bsm.FindNextTurn());
    }

    public void OnSwapPositionsButton()
    {
        bsm.uiHandler.ResetUI();

        bsm.battleStationManager.SwapStations(bsm.currentCharacter);

        StartCoroutine(bsm.FindNextTurn());
    }

    public void OnDefendButton()
    {
        bsm.uiHandler.ResetUI();
        Statuses status = new Statuses(Status.Defending, bsm.turnCounter + 1);
        if (bsm.currentCharacter.currStatuses.Where(s => s.status == Status.Defending).Any())
            bsm.currentCharacter.currStatuses.First(s => s.status == Status.Defending).expirationTurn = bsm.turnCounter + 1;
        else
            bsm.currentCharacter.currStatuses.Add(status);

        StartCoroutine(bsm.FindNextTurn());
    }
}
