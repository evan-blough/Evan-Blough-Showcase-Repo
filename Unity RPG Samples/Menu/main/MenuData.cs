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
        GameObject temp;
        foreach (PlayerCharacterData data in dataList)
        {
            if (!data.isInParty) continue;

            temp = Instantiate(menuSlotPrefab, transform);
            temp.GetComponent<CharacterDisplay>().SetDisplayData(data);
        }
    }
}
