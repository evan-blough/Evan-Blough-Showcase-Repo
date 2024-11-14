using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class EnemyHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Text hpText;
    public Enemy enemy;

    public void SetHUD()
    {
        nameText.text = enemy.unitName;
        levelText.text = "Level " + enemy.level;
        hpText.text = enemy.currHP.ToString();
    }
}
