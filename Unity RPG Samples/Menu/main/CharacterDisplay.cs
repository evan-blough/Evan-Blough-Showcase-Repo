using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{
    public Image portrait;
    public Text level;
    public Text nameText;
    public Text className;
    public Text health;
    public Text skillPoints;
    public Text status;

    public void SetDisplayData(PlayerCharacterData data)
    {
        portrait.sprite = data.portrait;
        level.text = data.level.ToString();
        string statusText = data.currStatuses.Any() ? data.currStatuses.First().status.ToString() : string.Empty;
        status.text = statusText;
        nameText.text = data.unitName;
        health.text = $"{data.currHP} / {data.maxHP}";
        skillPoints.text = $"{data.currSP} / {data.maxHP}";
    }
}
