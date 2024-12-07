using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WinLossScript : MonoBehaviour
{
    public Text[] characterExpText = new Text[3];
    public Text[] characterNameText = new Text[3];
    public Text goldText;

    public void SetWinBox(List<Enemy> enemies, List<PlayerCharacter> playerCharacterList)
    {
        int expGained = 0;
        int goldGained = 0;
        int index = 0;

        foreach (Enemy enemy in enemies)
        {
            expGained += enemy.expValue;
            goldGained += enemy.goldValue;
        }

        GameManager gm = GameManager.instance;
        gm.gold += goldGained;
        goldText.text = goldGained.ToString();

        for (int i = 0; i < 3; i++)
        {
            characterNameText[i].text = string.Empty;
            characterExpText[i].text = string.Empty;
        }

        foreach (PlayerCharacter character in playerCharacterList)
        {
                characterNameText[index].text = character.unitName;
                characterExpText[index].text = "0";
                if (character.isActive)
                {
                    character.expHandler.AddExperience(character, expGained);
                    characterExpText[index].text = expGained.ToString();
                }
            
            index++;
        }
    }
}
