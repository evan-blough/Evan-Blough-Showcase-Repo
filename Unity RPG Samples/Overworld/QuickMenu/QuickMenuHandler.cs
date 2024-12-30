using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    PlayerControls controls;
    public QuickMenu menu;

    private void Start()
    {
        controls = ControlsHandler.instance.playerControls;

        controls.overworld.QuickMenu.performed += ctx => ActivateQuickMenu();

        controls.overworld.Cancel.performed += ctx => DeactivateQuickMenu();

        controls.overworld.Menu.performed += ctx => OpenMenu();
    }

    public void ActivateQuickMenu()
    {
        if (menu.gameObject.activeInHierarchy)
        {
            DeactivateQuickMenu();
            return;
        }

        menu.SetMenu();

        menu.gameObject.SetActive(true);
    }

    public void DeactivateQuickMenu()
    {
        if (menu.gameObject.activeInHierarchy)
        {
            menu.gameObject.SetActive(false);
        }
    }

    public void OpenMenu()
    {
        SceneManager.instance.StartCoroutine(SceneManager.instance.OpenMenu());
    }
}
