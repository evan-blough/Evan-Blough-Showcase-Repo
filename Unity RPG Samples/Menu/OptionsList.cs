using UnityEngine;

public class OptionsList : MonoBehaviour
{
    PlayerControls controls;
    public MenuHandler menuHandler;

    private void Awake()
    {
        controls = ControlsHandler.instance.playerControls;
    }
    private void Start()
    {
        SceneManager.instance.TransitionTime("Enter_Scene", .5f);

        controls.menu.Return.performed += ctx => ReturnToOverworld();
    }

    public void OnItemsButton()
    {
        menuHandler.OpenItemMenu();
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void ReturnToOverworld()
    {
        SceneManager.instance.StartCoroutine(SceneManager.instance.CloseMenu());
    }
}
