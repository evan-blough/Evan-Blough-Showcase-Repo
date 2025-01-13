using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterItemDisplay : MonoBehaviour
{
    public GameObject characterDisplayPrefab;
    public Button allHeroesButton;
    public InventoryDisplayHandler inventoryDisplayHandler;

    private void Start()
    {
        SetupMenu();
    }

    public void SetupMenu()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        GameObject temp;
        allHeroesButton.onClick.RemoveAllListeners();
        var list = BattlePartyHandler.instance.partyData.Where(pcd => pcd.isInParty).ToList();
        allHeroesButton.onClick.AddListener(() => FieldItemUse(list));
        foreach (PlayerCharacterData pcd in BattlePartyHandler.instance.partyData)
        {
            if (!pcd.isInParty) continue;

            temp = Instantiate(characterDisplayPrefab, transform);
            temp.GetComponentInChildren<BattleInventoryDisplay>().CreateCharacterDisplay(pcd);
            temp.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            temp.GetComponentInChildren<Button>().onClick.AddListener(() => FieldItemUse(new List<PlayerCharacterData> { pcd }));
        }
    }

    public void ChangeToBattleInventoryView()
    {
        allHeroesButton.onClick.RemoveAllListeners();
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<BattleInventoryDisplay>().PopulateBattleInventoryDisplay();
            child.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            var pcd = child.gameObject.GetComponent<BattleInventoryDisplay>().pcd;
            child.GetComponentInChildren<Button>().onClick.AddListener(() => BattleItemUse(pcd));
        }
    }

    public void ChangeToCharDataView()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<BattleInventoryDisplay>().PopulateCharacterDisplay();
        }
    }

    public List<PlayerCharacterData> FindMultiTarget()
    {
        List<PlayerCharacterData> list = new List<PlayerCharacterData>();
        foreach (var pcd in BattlePartyHandler.instance.partyData)
        {
            if (pcd.isInParty)
                list.Add(pcd);
        }
        return list;
    }

    public void ActivateFieldTargets(Items item)
    {
        var buttons = GetComponentsInChildren<Button>();
        var characters = GetComponentsInChildren<BattleInventoryDisplay>();

        if (item is BattleItem && ((BattleItem)item).isMultiTargeted)
        {
            Debug.Log(item);
            allHeroesButton.interactable = true;
            foreach (Button button in buttons)
                button.interactable = false;
        }
        else if (item is BattleItem && item is ReviveItem)
        {
            allHeroesButton.interactable = false;
            for (int i = 0; i < buttons.Length && i < characters.Length; i++)
            {
                if (!characters[i].pcd.isActive)
                {
                    buttons[i].interactable = true;
                }
            }
        }
        else
        {
            allHeroesButton.interactable = false;
            for (int i = 0; i < buttons.Length && i < characters.Length; i++)
            {
                if (characters[i].pcd.isActive)
                {
                    buttons[i].interactable = true;
                }
            }
        }
    }

    public void ActivateBattleTargets(Items item)
    {
        var buttons = GetComponentsInChildren<Button>();
        allHeroesButton.interactable = false;
        var characters = GetComponentsInChildren<BattleInventoryDisplay>();

        for (int i = 0; i < buttons.Length && i < characters.Length; i++)
        {
            if (characters[i].pcd.charInventory.items.Count < characters[i].pcd.charInventory.maxItems)
            {
                buttons[i].interactable = true;
            }
        }
    }

    public void FieldItemUse(List<PlayerCharacterData> pcd)
    {
        DeactivateButtons();
        inventoryDisplayHandler.OnFieldItemUse(pcd);
    }

    public void BattleItemUse(PlayerCharacterData pcd)
    {
        DeactivateButtons();
        inventoryDisplayHandler.OnBattleItemUse(pcd);
    }

    public void DeactivateButtons()
    {
        var buttons = GetComponentsInChildren<Button>();
        allHeroesButton.interactable = false;
        foreach (Button button in buttons)
            button.interactable = false;
    }
}
