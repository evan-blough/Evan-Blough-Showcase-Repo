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
        if (character.currHP == 0)
        {
            nameText.color = Color.red;
            levelText.color = Color.red;
            hpText.color = Color.red;
            spText.color = Color.red;
        }
        else if (character.currHP <= character.maxHP * .33)
        {
            nameText.color = Color.yellow;
            levelText.color = Color.yellow;
            hpText.color = Color.yellow;
            spText.color = Color.yellow;
        }
        else
        {
            nameText.color = Color.white;
            levelText.color = Color.white;
            hpText.color = Color.white;
            spText.color = Color.white;
        }

        nameText.text = character.unitName;
        levelText.text = "Level " + character.level;
        hpText.text = character.currHP.ToString() + "/" + character.maxHP.ToString();
        spText.text = "SP:" + character.currSP.ToString();
    }
}
