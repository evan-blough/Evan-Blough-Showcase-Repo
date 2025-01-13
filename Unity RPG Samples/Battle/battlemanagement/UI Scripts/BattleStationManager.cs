using UnityEngine;
using UnityEngine.UI;

public class BattleStationManager : MonoBehaviour
{
    public Transform topBattleStation;
    public Transform bottomBattleStation;
    public Transform backBattleStation;
    public Transform enemyTopLeftStation;
    public Transform enemyTopBackStation;
    public Transform enemyBottomBackStation;
    public Transform enemyBottomLeftStation;

    public Text upperStationDamage;
    public Text lowerStationDamage;
    public Text backRowDamage;
    public Text topLeftDamage;
    public Text bottomLeftDamage;
    public Text bottomBackDamage;
    public Text topBackDamage;


    public Character SetStation(GameObject prefab, int station)
    {
        GameObject thisChar;
        switch (station)
        {
            case 0:
                thisChar = Instantiate(prefab, topBattleStation);
                break;
            case 1:
                thisChar = Instantiate(prefab, bottomBattleStation);
                break;
            case 2:
                thisChar = Instantiate(prefab, backBattleStation);
                break;
            case 3:
                thisChar = Instantiate(prefab, enemyTopLeftStation);
                break;
            case 4:
                thisChar = Instantiate(prefab, enemyBottomLeftStation);
                break;
            case 5:
                thisChar = Instantiate(prefab, enemyTopBackStation);
                break;
            default:
                thisChar = Instantiate(prefab, enemyBottomBackStation);
                break;
        }
        var returnChar = thisChar.GetComponent<Character>();
        if (station == 2 || station == 5 || station >= 6) returnChar.isBackRow = true;

        return returnChar;
    }

    public void SwapStations(Character currentCharacter)
    {
        Character tempChar = backBattleStation.GetComponentInChildren<Character>();
        Transform temp = currentCharacter.transform.parent;

        currentCharacter.transform.SetParent(backBattleStation);
        currentCharacter.transform.position = backBattleStation.position;
        currentCharacter.isBackRow = true;

        if (tempChar != null)
        {
            tempChar.transform.SetParent(temp);
            tempChar.transform.position = temp.position;
            tempChar.isBackRow = false;
        }
    }

    public void SetText(string text, Character unit)
    {
        if (unit.gameObject.transform.parent == topBattleStation)
        {
            upperStationDamage.text = text == "0" ? "Miss" : (text == "-1" ? "Immune" : text);
        }
        if (unit.gameObject.transform.parent == bottomBattleStation)
        {
            lowerStationDamage.text = text == "0" ? "Miss" : (text == "-1" ? "Immune" : text);
        }
        if (unit.gameObject.transform.parent == backBattleStation)
        {
            backRowDamage.text = text == "0" ? "Miss" : (text == "-1" ? "Immune" : text);
        }
        if (unit.gameObject.transform.parent == enemyTopLeftStation)
        {
            topLeftDamage.text = text == "0" ? "Miss" : (text == "-1" ? "Immune" : text);
        }
        if (unit.gameObject.transform.parent == enemyBottomLeftStation)
        {
            bottomLeftDamage.text = text == "0" ? "Miss" : (text == "-1" ? "Immune" : text);
        }
        if (unit.gameObject.transform.parent == enemyTopBackStation)
        {
            topBackDamage.text = text == "0" ? "Miss" : (text == "-1" ? "Immune" : text);
        }
        if (unit.gameObject.transform.parent == enemyBottomBackStation)
        {
            bottomBackDamage.text = text == "0" ? "Miss" : (text == "-1" ? "Immune" : text);
        }
    }
    public void SetTextColor(Color color)
    {
        upperStationDamage.color = color;
        lowerStationDamage.color = color;
        backRowDamage.color = color;
        topLeftDamage.color = color;
        bottomLeftDamage.color = color;
    }
}
