using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public static InventoryHandler instance;
    public Inventory inventory;
    public int gold;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
            Destroy(instance);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
