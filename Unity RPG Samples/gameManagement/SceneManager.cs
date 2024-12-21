using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    public CameraHandler cam;
    public EnemyHandler enemyInBattle;
    public Animator animator;
    Scene overworld;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the SceneManager object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator StartBattle(EnemyHandler enemy)
    {
        enemyInBattle = enemy;

        overworld = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        foreach (var thing in overworld.GetRootGameObjects())
        {
            thing.SetActive(false);
        }

        yield return StartCoroutine(BattleTransitionTime("Exit_Scene"));

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);
    }

    public IEnumerator TransitionFromBattle(List<PlayerCharacter> playerCharacterList, bool battleWon)
    {
        BattlePartyHandler.instance.SetPartyData(playerCharacterList);

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        yield return StartCoroutine(BattleTransitionTime("Exit_Scene"));

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Battle");


        foreach (var thing in overworld.GetRootGameObjects())
        {
            thing.SetActive(true);
        }
        if (battleWon) Destroy(enemyInBattle.gameObject);

        yield return StartCoroutine(BattleTransitionTime("Enter_Scene"));

    }

    public IEnumerator BattleTransitionTime(string trigger)
    {
        animator.SetTrigger(trigger);

        yield return new WaitForSeconds(.5f);
    }

    public void SaveGame()
    {
        return;
    }
}
