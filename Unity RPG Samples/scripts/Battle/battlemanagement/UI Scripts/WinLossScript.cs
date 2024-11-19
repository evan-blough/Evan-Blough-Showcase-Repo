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

    public void SetWinBox(List<Enemy> enemies, Hero hero, Wizard wizard, Traitor senator)
    {
        int expGained = 0;
        int goldGained = 0;

        foreach (Enemy enemy in enemies)
        {
            expGained += enemy.expValue;
            goldGained += enemy.goldValue;
        }

        if (hero.isInParty)
        {
            heroText.text = hero.unitName;
            heroExpText.text = "0";
            heroGoldText.text = "0";
            if (hero.isActive)
            {
                hero.exp += expGained;
                hero.gold += goldGained;
                heroExpText.text = expGained.ToString();
                heroGoldText.text = goldGained.ToString();
            }
        }
        if (wizard.isInParty)
        {
            wizardText.text = wizard.unitName;
            wizardExpText.text = "0";
            wizardGoldText.text = "0";
            if (wizard.isActive)
            {
                wizard.exp += expGained;
                wizard.gold += goldGained;
                wizardExpText.text = expGained.ToString();
                wizardGoldText.text = goldGained.ToString();
            }
        }
        if (senator.isInParty)
        {
            senatorText.text = senator.unitName;
            senatorExpText.text = "0";
            senatorGoldText.text = "0";
            if (senator.isActive)
            {
                senator.exp += expGained;
                senator.gold += goldGained;
                senatorExpText.text = expGained.ToString();
                senatorGoldText.text = goldGained.ToString();
            }
        }
    }
}
