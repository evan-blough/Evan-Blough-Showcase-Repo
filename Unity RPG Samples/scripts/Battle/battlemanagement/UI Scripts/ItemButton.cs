using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Button button;
    public Items item;
    public UIHandler uiHandler;
    public void SetDetails()
    {
        uiHandler.SetItemDetails(item);
    }

    public void RemoveDetails()
    {
        if (button != EventSystem.current.currentSelectedGameObject)
            uiHandler.RemoveSkillDetails();
    }
}
