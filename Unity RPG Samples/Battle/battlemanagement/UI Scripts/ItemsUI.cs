using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public BattleStateMachine bsm;
    public BattleItem currentItem;
    public DescriptionBox descriptionBox;

    public void CallItemsHUD()
    {
        bsm.uiHandler.UIOnItems();

        List<BattleItem> items = new List<BattleItem>();

        foreach (Transform child in bsm.uiHandler.itemsUI.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (BattleItem item in ((PlayerCharacter)bsm.currentCharacter).charInventory.items)
        {
            if (item is null) continue;

            items.Add(item);
            GameObject tempButton = Instantiate(buttonPrefab);
            tempButton.transform.SetParent(bsm.uiHandler.itemsUI.transform, false);
            var i = tempButton.GetComponent<ItemButton>();
            i.item = item;
            i.uiHandler = bsm.uiHandler;

            Button itemsButton = tempButton.GetComponentInChildren<Button>();
            itemsButton.onClick.AddListener(() => OnItemButton(item));
            Text tempText = tempButton.GetComponentInChildren<Text>();
            tempText.text = item.itemName;
        }

        bsm.uiHandler.itemsUI.gameObject.SetActive(true);
    }
    public void OnItemButton(BattleItem item)
    {
        currentItem = item;
        bsm.uiHandler.targetingUI.ActivateTargets(item);
    }

    public IEnumerator OnTargetSelected(List<Character> targets)
    {
        List<string> returns;
        bsm.uiHandler.ResetUI();
        ((PlayerCharacter)bsm.currentCharacter).charInventory.items.Remove(currentItem);

        returns = currentItem.UseItem(targets, bsm.turnCounter);

        bool isRecoveryItem = currentItem is ReviveItem || currentItem is HealItem || currentItem is StatusRemovalItem || currentItem is SPRecoveryItem;
        yield return StartCoroutine(ApplyText(targets, returns, isRecoveryItem));
        currentItem = null;

        StartCoroutine(bsm.FindNextTurn());
    }

    public IEnumerator ApplyText(List<Character> targets, List<string> returns, bool isRecoveryItem)
    {
        HandleItemText(targets, returns, isRecoveryItem);
        yield return new WaitForSeconds(.55f);

        for (int i = 0; i < returns.Count; i++) returns[i] = string.Empty;
        HandleItemText(targets, returns, false);
        yield return new WaitForSeconds(.75f);
    }

    public void HandleItemText(List<Character> target, List<string> text, bool isRecoveryItem)
    {
        Color color = isRecoveryItem ? Color.green : Color.white;

        bsm.battleStationManager.SetTextColor(color);

        for (int i = 0; i < text.Count && i < target.Count; i++)
        {
            bsm.battleStationManager.SetText(text[i], target[i]);
        }
    }
}
