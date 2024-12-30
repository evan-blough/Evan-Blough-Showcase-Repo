using System.Collections.Generic;
using UnityEngine;
public class MenuData : MonoBehaviour
{
    public GameObject menuSlotPrefab;

    private void Start()
    {
        FillSlotData(BattlePartyHandler.instance.partyData);
    }
    public void FillSlotData(List<PlayerCharacterData> dataList)
    {
        CharacterDisplay newSlot;
        foreach (PlayerCharacterData data in dataList)
        {
            if (!data.isInParty) continue;

            GameObject temp = Instantiate(menuSlotPrefab, transform);
            newSlot = temp.GetComponent<CharacterDisplay>();
            newSlot.SetDisplayData(data);
        }
    }
}
