using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour
{
    public CharacterHUD hudPrefab;
    public EnemyHUD enemyHUDPrefab;
    public GameObject heroList;
    public GameObject enemyList;
    public TargetingUI targetingUI;
    public List<CharacterHUD> CreateCharacterHuds(List<PlayerCharacter> characters)
    {
        List<CharacterHUD> list = new List<CharacterHUD>(); 

        foreach (PlayerCharacter character in characters)
        {
            CharacterHUD newHud = Instantiate(hudPrefab);
            newHud.transform.SetParent(heroList.transform, false);
            newHud.character = character;
            newHud.SetHUD();
            Button tempButton = newHud.GetComponent<Button>();
            tempButton.interactable = false;
            tempButton.onClick.AddListener(() => targetingUI.FindTarget(character));
            list.Add(newHud);
        }
        List<Character> c = new List<Character>();
        c.AddRange(characters);

        heroList.GetComponent<Button>().interactable = false;
        heroList.GetComponent<Button>().onClick.AddListener(() => targetingUI.FindMultiTarget(c));

        return list;
    }

    public List<EnemyHUD> CreateEnemyHuds(List<Enemy> enemies)
    {
        List<EnemyHUD> list = new List<EnemyHUD>();

        foreach (Enemy enemy in enemies)
        {
            EnemyHUD newHud = Instantiate(enemyHUDPrefab);
            newHud.transform.SetParent(enemyList.transform, false);
            newHud.enemy = enemy;
            newHud.SetHUD();
            Button tempButton = newHud.GetComponent<Button>();
            tempButton.interactable = false;
            tempButton.onClick.AddListener(() => targetingUI.FindTarget(enemy));
            list.Add(newHud);
        }
        List<Character> c = new List<Character>();
        c.AddRange(enemies);

        enemyList.GetComponent<Button>().interactable = false;
        enemyList.GetComponent<Button>().onClick.AddListener(() => targetingUI.FindMultiTarget(c));

        return list;
    }

    public void DeactivateEnemyHud()
    {
        enemyList.gameObject.SetActive(false);
    }
}
