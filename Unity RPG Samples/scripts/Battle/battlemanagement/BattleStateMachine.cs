using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
public enum BattleStates
{
    START, PLAYERTURN, ENEMYTURN, WIN, LOSS, FLEE, WAIT
}
public class BattleStateMachine: MonoBehaviour
{
    public GameObject heroPrefab;
    public GameObject enemyPrefab;
    public GameObject wizardPrefab;
    public GameObject senatorPrefab;

    public Transform topBattleStation;
    public Transform lowerBattleStation;
    public Transform enemyBattleStation;
    public Transform backRowStation;
    public Button attackButton;

    Hero heroUnit;
    Wizard wizardUnit;
    Traitor senatorUnit;
    Enemy enemyUnit;

    public Text upperStationDamage;
    public Text lowerStationDamage;
    public Text backRowDamage;
    public Text enemyDamage;

    public float animSpeed = 5f;
    public GameObject skillHUD;
    public GameObject commandHUD;
    public GameObject battleHUD;
    public HUDHandler dataHudHandler;
    public WinLossScript winBox;
    public GameObject lossBox;
    public FleeScript fleeBox;
    public TargetingUI targetingUI;

    public List<Character> turnOrder;
    public Character currentCharacter;
    public List<PlayerCharacter> playerCharacterList;
    public List<Enemy> enemies;
    public List<CharacterHUD> characterHUDList;
    public List<EnemyHUD> enemyHUDList;
    public int turnCounter;
    public BattleStates state;
    // Start is called before the first frame update
    public void Start()
    {
        battleHUD.gameObject.SetActive(false);
        skillHUD.gameObject.SetActive(false);
        commandHUD.gameObject.SetActive(false);
        winBox.gameObject.SetActive(false);
        lossBox.gameObject.SetActive(false);
        fleeBox.gameObject.SetActive(false);
        targetingUI.DeactivateButtons();
        state = BattleStates.START;
        StartCoroutine(SetupBattle());
    }

    public void ResetUI()
    {
        battleHUD.gameObject.SetActive(false);
        skillHUD.gameObject.SetActive(false);
        commandHUD.gameObject.SetActive(false);
        targetingUI.DeactivateButtons();
    }

    public IEnumerator SetupBattle()
    {
        if (heroPrefab.GetComponent<Hero>().isInParty)
        {
            GameObject heroGO = Instantiate(heroPrefab, topBattleStation);
            heroUnit = heroGO.GetComponent<Hero>();
            playerCharacterList.Add(heroUnit);
        }

        if (wizardPrefab.GetComponent<Wizard>().isInParty)
        {
            GameObject wizardGO = Instantiate(wizardPrefab, lowerBattleStation);
            wizardUnit = wizardGO.GetComponent<Wizard>();
            playerCharacterList.Add(wizardUnit);
        }

        if (senatorPrefab.GetComponent<Traitor>().isInParty)
        {
            GameObject senatorGO = Instantiate(senatorPrefab, backRowStation);
            senatorUnit = senatorGO.GetComponent<Traitor>();
            playerCharacterList.Add(senatorUnit);
        }

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);

        enemyUnit = enemyGO.GetComponent<Enemy>();
        enemyUnit.rarity = Enemy.InitializeRarity();
        enemies.Add(enemyUnit);

        characterHUDList = dataHudHandler.CreateCharacterHuds(playerCharacterList);
        enemyHUDList = dataHudHandler.CreateEnemyHuds(enemies);

        yield return new WaitForSeconds(1.5f);

        state = BattleStates.WAIT;
        turnCounter = 0;
        StartCoroutine(FindNextTurn());
    }

    public IEnumerator FindNextTurn()
    {
        yield return new WaitForSeconds(.25f);
        targetingUI.DeactivateButtons();
        UpdateHUDs();
        turnOrder.RemoveAll(c => !c.isActive);

        if (playerCharacterList.Where(c => !c.isIncapacitated).Count() == 0)
        {
            state = BattleStates.LOSS;
            EndBattle();
            yield break;
        }
        else if (enemies.Where(e => e.isActive).ToList().Count == 0)
        {
            state = BattleStates.WIN;
            EndBattle();
            yield break;
        }

        if (turnOrder.Count == 0)
        {
            turnCounter++;

            if (heroUnit.isActive && heroUnit.isInParty) turnOrder.Add(heroUnit);

            if (enemyUnit.isActive) turnOrder.Add(enemyUnit);

            if (wizardUnit.isActive && wizardUnit.isInParty) turnOrder.Add(wizardUnit);

            if (senatorUnit.isActive && senatorUnit.isInParty) turnOrder.Add(senatorUnit);

            for (int i = 0; i < turnOrder.Count - 1; i++)
            {
                for (int j = 1; j < turnOrder.Count; j++)
                {
                    if (turnOrder[i].agility + (.1 * turnOrder[i].luck + Random.Range(0,3)) <
                        turnOrder[j].agility + (.1 * turnOrder[j].luck + Random.Range(0, 3)))
                    {
                        (turnOrder[i], turnOrder[j]) = (turnOrder[j], turnOrder[i]);
                    }
                }
            }
        }
        currentCharacter = turnOrder.First();
        turnOrder.Remove(turnOrder[0]);

        if (currentCharacter.currStatuses.Count > 0 )
            Statuses.HandleStatuses(currentCharacter, turnCounter);

        if (currentCharacter is PlayerCharacter)
        {
            state = BattleStates.PLAYERTURN;
            PlayerTurn();
            yield break;
        }

        else if (currentCharacter is Enemy)
        {
            state = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn((Enemy)currentCharacter));
            yield break;
        }
        yield break;
    }

    public void PlayerTurn()
    {
        if (currentCharacter.currStatuses.Where(s => s.status == Status.POISONED).Any()) currentCharacter.currHP -= (int)(currentCharacter.maxHP / 16);
        if (currentCharacter.currStatuses.Where(s => s.status == Status.BLEED).Any()) currentCharacter.currHP -= (int)(currentCharacter.maxHP / 10);
        if (currentCharacter.currStatuses.Where(s => s.status == Status.PARALYZED).Any())
        {
            StartCoroutine(FindNextTurn());
            return;
        }
        if (currentCharacter.currStatuses.Where(s => s.status == Status.BERSERK).Any())
        {
            if (enemies.Count > 1)
            StartCoroutine(PlayerAttack(enemies.Where(t => !t.isBackRow && t.isActive).ToList()[Random.Range(0, enemies.Count - 1)]));
            else
            StartCoroutine(PlayerAttack(enemies.First()));
            return;
        }
        battleHUD.gameObject.SetActive(true);
        attackButton.gameObject.SetActive(!currentCharacter.isBackRow); 
    }
    public IEnumerator PlayerAttack(Character target)
    {
        state = BattleStates.WAIT;

        ResetUI();

        int attackPower = currentCharacter.Attack(target);

        SetText(attackPower.ToString(), target);

        yield return new WaitForSeconds(.55f);

        SetText(string.Empty, target);

        if(target.currHP == 0) { target.isActive = false; }

        yield return new WaitForSeconds(.75f);

        state = BattleStates.WAIT;
        StartCoroutine(FindNextTurn());
    }
    public void EndBattle()
    {
        ResetUI();

        if(state == BattleStates.LOSS )
        {
            lossBox.gameObject.SetActive(true);
        }
        else if (state == BattleStates.WIN )
        {
            winBox.SetWinBox(enemyUnit, heroUnit, wizardUnit, senatorUnit);
            winBox.gameObject.SetActive(true);
        }
        else if (state == BattleStates.FLEE)
        {
            fleeBox.gameObject.SetActive(true);
        }
    }
    public IEnumerator EnemyTurn(Enemy enemy)
    {
        if (currentCharacter.currStatuses.Where(s => s.status == Status.POISONED).Any()) currentCharacter.currHP -= (int)(currentCharacter.maxHP / 16);
        if (currentCharacter.currStatuses.Where(s => s.status == Status.BLEED).Any()) currentCharacter.currHP -= (int)(currentCharacter.maxHP / 10);
        if (currentCharacter.currStatuses.Where(s => s.status == Status.PARALYZED).Any())
        {
            StartCoroutine(FindNextTurn());
            yield break;
        }

        Character targetUnit = enemy.FindTarget(playerCharacterList.Where(c => c.isActive && !c.isIncapacitated).ToList());

        int damage = enemyUnit.Attack(targetUnit);

        if(targetUnit.currHP < 0 ) { heroUnit.currHP = 0;}

        SetText(damage.ToString(), targetUnit);

        yield return new WaitForSeconds(0.5f);

        SetText(string.Empty, targetUnit);

        yield return new WaitForSeconds(0.75f);

        if (targetUnit.currHP <= 0)
        {
            targetUnit.isActive = false;
            targetUnit.isIncapacitated = true;
            turnOrder.Remove(targetUnit);
        }
        
        state = BattleStates.WAIT;
        StartCoroutine(FindNextTurn());
    }
    public void OnAttackButton()
    {
        targetingUI.ActivateTargets(currentCharacter);
    }

    public void OnFleeButton()
    {
        ResetUI();

        bool fleeCheck = heroUnit.FleeCheck(enemyUnit);
        fleeBox.SetFleeBox(fleeCheck);
        
        if (fleeCheck)
        {
            state = BattleStates.FLEE;
            EndBattle();
        }
        else
        {
            StartCoroutine(FailedFlee());
        }
    }

    IEnumerator FailedFlee()
    {
        fleeBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        fleeBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(.5f);

        StartCoroutine(FindNextTurn());
    }

    public void UpdateHUDs()
    {
        foreach (CharacterHUD hud in characterHUDList) hud.SetHUD();
        foreach (EnemyHUD hud in enemyHUDList) hud.SetHUD();
    }

    public void SetText(string text, Character unit)
    {
        if (unit.gameObject.transform.parent == topBattleStation)
        {
            upperStationDamage.text = text == "0" ? "MISS" : text;
        }
        if (unit.gameObject.transform.parent == lowerBattleStation)
        {
            lowerStationDamage.text = text == "0" ? "MISS" : text;
        }
        if (unit.gameObject.transform.parent == backRowStation)
        {
            backRowDamage.text = text == "0" ? "MISS" : text;
        }
        if (unit is Enemy)
        {
            enemyDamage.text = text == "0" ? "MISS" : text;
        }
    }

    public void SetTextColor(Color color)
    {
        upperStationDamage.color = color;
        lowerStationDamage.color = color;
        backRowDamage.color = color;
        enemyDamage.color = color;
    }
}
