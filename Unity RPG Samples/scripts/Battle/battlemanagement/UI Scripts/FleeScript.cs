using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FleeScript : MonoBehaviour
{
    public Text fleeText;
    public void SetFleeBox(bool isFled)
    {
        fleeText.text = isFled ? "Successfully ran!" : "Failed to run.";
    }
}
