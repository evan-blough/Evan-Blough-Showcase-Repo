using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionBox : MonoBehaviour
{
    public Text description;

    public void SetDescription (string desc)
    {
        description.text = desc;
    }
}
