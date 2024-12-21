using UnityEngine;

public class ControlsHandler : MonoBehaviour
{
    public static ControlsHandler instance;

    public PlayerControls playerControls;
    private void Awake()
    {
        playerControls = new PlayerControls();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep the ControlsHandler object alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (playerControls is null) playerControls = new PlayerControls();
    }
}
