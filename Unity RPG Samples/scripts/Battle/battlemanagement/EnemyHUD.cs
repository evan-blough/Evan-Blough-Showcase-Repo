using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class EnemyHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Text hpText;

    public void SetHUD(Enemy unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Level " + unit.level;
        hpText.text = unit.currHP.ToString();
    }

    public void SetHP(int hp)
    {
        hpText.text = hp.ToString();
    }
}
