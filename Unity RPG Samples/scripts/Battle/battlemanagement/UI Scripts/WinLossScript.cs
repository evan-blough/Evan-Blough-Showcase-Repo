using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WinLossScript : MonoBehaviour
{
    public Text heroExpText;
    public Text wizardExpText;
    public Text senatorExpText;
    public Text heroText;
    public Text wizardText;
    public Text senatorText;
    public Text heroGoldText;
    public Text wizardGoldText;
    public Text senatorGoldText;

    public void SetWinBox(Enemy enemy, Hero hero, Wizard wizard, Traitor senator)
    {
        if (hero.isInParty)
        {
            heroText.text = hero.unitName;
            heroExpText.text = "0";
            heroGoldText.text = "0";
            if (!hero.isIncapacitated)
            {
                hero.exp += enemy.expValue;
                hero.gold += enemy.goldValue;
                heroExpText.text = enemy.expValue.ToString();
                heroGoldText.text = enemy.goldValue.ToString();
            }
        }
        if (wizard.isInParty)
        {
            wizardText.text = wizard.unitName;
            wizardExpText.text = "0";
            wizardGoldText.text = "0";
            if (!wizard.isIncapacitated)
            {
                wizard.exp += enemy.expValue;
                wizard.gold += enemy.goldValue;
                wizardExpText.text = enemy.expValue.ToString();
                wizardGoldText.text = enemy.goldValue.ToString();
            }
        }
        if (senator.isInParty)
        {
            senatorText.text = senator.unitName;
            senatorExpText.text = "0";
            senatorGoldText.text = "0";
            if (!senator.isIncapacitated)
            {
                senator.exp += enemy.expValue;
                senator.gold += enemy.goldValue;
                senatorExpText.text = enemy.expValue.ToString();
                senatorGoldText.text = enemy.goldValue.ToString();
            }
        }
    }
}
