using UnityEngine;
using UnityEngine.UI;

public class MainModalHandler : MonoBehaviour
{
    public Text currTime;
    public Text gold;
    public Text location;

    // Start is called before the first frame update
    void Start()
    {
        PopulateModal();
    }

    public void PopulateModal()
    {
        // need game timer and location name
        gold.text = BattlePartyHandler.instance.gold.ToString();
    }
}
