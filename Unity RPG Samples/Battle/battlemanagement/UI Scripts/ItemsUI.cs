using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemsUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public BattleStateMachine bsm;
    public Items currentItem;
    public DescriptionBox descriptionBox;

    public void CallItemsHUD()
    {
        bsm.uiHandler.UIOnItems();

        List<Items> items = new List<Items>();

        foreach (Transform child in bsm.uiHandler.itemsUI.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Items item in ((PlayerCharacter)bsm.currentCharacter).charInventory.items)
        {
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
    public void OnItemButton(Items item)
    {
        currentItem = item;
        bsm.uiHandler.targetingUI.ActivateTargets(item, bsm.currentCharacter);
    }

    public IEnumerator OnTargetSelected(List<Character> targets)
    {
        List<string> returns;
        bsm.uiHandler.ResetUI();
        ((PlayerCharacter)bsm.currentCharacter).charInventory.items.Remove(currentItem);

        returns = currentItem.UseItem(targets, bsm.turnCounter);


        yield return StartCoroutine(ApplyText(targets, returns, currentItem.itemEffect.itemType));
        currentItem = null;

        StartCoroutine(bsm.FindNextTurn());
    }

    public IEnumerator ApplyText(List<Character> targets, List<string> returns, ItemEffectType type)
    {
        HandleItemText(targets, returns, type);
        yield return new WaitForSeconds(.55f);

        for (int i = 0; i < returns.Count; i++) returns[i] = string.Empty;
        HandleItemText(targets, returns, ItemEffectType.NONE);
        yield return new WaitForSeconds(.75f);
    }

    public void HandleItemText(List<Character> target, List<string> text, ItemEffectType type)
    {
        Color color = ((type == ItemEffectType.HEAL || ItemEffectType.RECOVER_SP == type || ItemEffectType.REVIVE == type) ? Color.green : Color.white);

        bsm.battleStationManager.SetTextColor(color);

        for (int i = 0; i < text.Count && i < target.Count; i++)
        {
            bsm.battleStationManager.SetText(text[i], target[i]);
        }
    }
}
