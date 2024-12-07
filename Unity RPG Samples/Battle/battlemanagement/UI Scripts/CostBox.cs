using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostBox : MonoBehaviour
{
    public Text spCost;

    public void SetCostBox(string cost, string currentSP)
    {
        spCost.text = $"{cost} / {currentSP}";
    }
}
