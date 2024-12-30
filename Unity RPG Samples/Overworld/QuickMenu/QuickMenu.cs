using UnityEngine;

public class QuickMenu : MonoBehaviour
{
    BattlePartyHandler partyHandler;
    public QuickCharHUD hudPrefab;

    private void Start()
    {
        partyHandler = BattlePartyHandler.instance;

        gameObject.SetActive(false);
    }

    public void SetMenu()
    {
        foreach (PlayerCharacterData character in partyHandler.partyData)
        {
            QuickCharHUD newHud = Instantiate(hudPrefab);
            newHud.transform.SetParent(transform, false);
            newHud.SetHUD(character);
        }
    }

    public void DestroyHUDs()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnDisable()
    {
        DestroyHUDs();
    }
}
