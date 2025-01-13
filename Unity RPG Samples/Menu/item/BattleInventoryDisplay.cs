using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleInventoryDisplay : MonoBehaviour
{
    public Image portrait;
    public GameObject battleInventory;
    public GameObject charDataView;
    public Text hpView;
    public Text spView;
    public Text unitName;
    public Text level;
    public Text status;
    public GameObject battleInventoryButtonPrefab;
    public PlayerCharacterData pcd;

    public void CreateCharacterDisplay(PlayerCharacterData data)
    {
        pcd = data;
        portrait.sprite = data.portrait;
        battleInventory.gameObject.SetActive(false);
        PopulateCharacterDisplay();
    }

    public void PopulateBattleInventoryDisplay()
    {
        GameObject temp;
        foreach (Transform child in battleInventory.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Items item in pcd.charInventory.items)
        {
            temp = Instantiate(battleInventoryButtonPrefab, battleInventory.transform);
            temp.GetComponentInChildren<BattleItemButton>().PopulateButton(item);
        }

        battleInventory.SetActive(true);
        charDataView.SetActive(false);
    }

    public void PopulateCharacterDisplay()
    {
        level.text = $"Level {pcd.level}";
        unitName.text = pcd.unitName;
        string statusText = pcd.currStatuses.Any() ? pcd.currStatuses.First().status.ToString() : string.Empty;
        status.text = statusText;
        hpView.text = $"HP: {pcd.currHP} / {pcd.maxHP}";
        spView.text = $"SP: {pcd.currSP} / {pcd.maxSP}";
        charDataView.SetActive(true);
        battleInventory.SetActive(false);
    }
}
