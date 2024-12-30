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

        ControlsHandler.OverworldToBattle();

        overworld = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        foreach (var thing in overworld.GetRootGameObjects())
        {
            thing.SetActive(false);
        }

        yield return StartCoroutine(TransitionTime("Exit_Scene", .5f));

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);
    }

    public IEnumerator TransitionFromBattle(List<PlayerCharacter> playerCharacterList, bool battleWon)
    {
        BattlePartyHandler.instance.SetPartyData(playerCharacterList);

        ControlsHandler.BattleToOverworld();

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        yield return StartCoroutine(TransitionTime("Exit_Scene", .5f));

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Battle");


        foreach (var thing in overworld.GetRootGameObjects())
        {
            thing.SetActive(true);
        }
        if (battleWon) Destroy(enemyInBattle.gameObject);

        yield return StartCoroutine(TransitionTime("Enter_Scene", .5f));

    }

    public IEnumerator OpenMenu()
    {
        overworld = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        ControlsHandler.OverworldToMenu();

        foreach (var thing in overworld.GetRootGameObjects())
        {
            thing.SetActive(false);
        }

        yield return StartCoroutine(TransitionTime("Exit_Scene", .5f));

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
    }

    public IEnumerator CloseMenu()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("Menu");

        ControlsHandler.MenuToOverworld();

        foreach (var thing in overworld.GetRootGameObjects())
        {
            thing.SetActive(true);
        }

        yield return StartCoroutine(TransitionTime("Enter_Scene", .5f));
    }

    public IEnumerator TransitionTime(string trigger, float time)
    {
        animator.SetTrigger(trigger);

        yield return new WaitForSeconds(time);
    }

    public void SaveGame()
    {
        return;
    }
}
