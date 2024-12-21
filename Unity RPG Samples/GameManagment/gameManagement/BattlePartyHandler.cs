using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlePartyHandler : MonoBehaviour
{
    public static BattlePartyHandler instance;
    public List<PlayerCharacterData> partyData;
    public HeroData heroData;
    public WizardData wizardData;
    public SenatorData senatorData;
    public Inventory inventory;
    public int gold;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetCurrentPartyData();
        }
        else
            Destroy(gameObject);
    }

    public void SetCurrentPartyData()
    {
        SetCharacterPartyStatus(heroData);
        SetCharacterPartyStatus(wizardData);
        SetCharacterPartyStatus(senatorData);
    }

    public void SetPartyData(List<PlayerCharacter> characters)
    {
        foreach (PlayerCharacter character in characters)
        {
            if (character is Hero) heroData.DeepDataCopy(character);
            else if (character is Wizard) wizardData.DeepDataCopy(character);
            else if (character is Senator) senatorData.DeepDataCopy(character);
        }
    }

    public void SetCharacterPartyStatus(PlayerCharacterData data)
    {
        if (data is not null && data.isInParty && !partyData.Any(c => c == data))
            partyData.Add(data);

        else if (data is null || (data is not null && !data.isInParty && partyData.Any(d => d == data)))
            partyData.Remove(data);
    }
}
