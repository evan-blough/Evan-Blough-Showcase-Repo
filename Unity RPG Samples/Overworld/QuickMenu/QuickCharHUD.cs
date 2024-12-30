using UnityEngine;
using UnityEngine.UI;

public class QuickCharHUD : MonoBehaviour
{
    public Image portrait;
    public Text levelText;
    public Text hpText;
    public Text spText;
    public void SetHUD(PlayerCharacterData character)
    {
        portrait.sprite = character.portrait;
        levelText.text = "Level " + character.level.ToString();
        hpText.text = character.currHP.ToString() + " / " + character.maxHP.ToString();
        spText.text = character.currSP.ToString() + " / " + character.maxSP.ToString();
    }
}
