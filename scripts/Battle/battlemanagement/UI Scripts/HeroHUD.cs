using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
public class CharacterHUD: MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Text hpText;
    public Text spText;
    public Character character;
    public void SetHUD()
    {
        nameText.text = character.unitName;
        levelText.text = "Level " + character.level;
        hpText.text = character.currHP.ToString() + "/" + character.maxHP.ToString();
        spText.text = "SP:" + character.currSP.ToString();
    }
}
