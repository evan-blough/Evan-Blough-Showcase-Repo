using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum BattleStates
{
    START, PLAYERTURN, ENEMYTURN, WIN, LOSS, WAIT
}
public class BattleStateMachine: MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Hero playerUnit;
    Enemy enemyUnit;

    public HeroHUD playerHUD;
    public EnemyHUD enemyHUD;

    public BattleStates state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleStates.START;
        StartCoroutine(SetupBattle());
    }
    
    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Hero>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Enemy>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleStates.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        
    }
    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.attack);

        enemyHUD.SetHP(enemyUnit.currHP);

        yield return new WaitForSeconds(2f);

        if(isDead)
        {
            state = BattleStates.WIN;
            EndBattle();
        }
        else
        {
            state = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }
    void EndBattle()
    {
        
    }
    IEnumerator EnemyTurn()
    {
        bool isHeroDead = playerUnit.TakeDamage(enemyUnit.attack);

        playerHUD.SetHP(playerUnit.currHP);

        yield return new WaitForSeconds(1f);

        if (isHeroDead)
        {
            state = BattleStates.LOSS;
            EndBattle();
        }
        else
        {
            state = BattleStates.PLAYERTURN;
            PlayerTurn();
        }
    }
    public void OnAttackButton()
    {
        if(state != BattleStates.PLAYERTURN)
        {
            return;
        }
        state = BattleStates.WAIT;
        StartCoroutine(PlayerAttack());
    }
}
