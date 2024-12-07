using UnityEngine;
using UnityEngine.UI;

public class EnemySkillUI : MonoBehaviour
{
    public Text text;

    public void SetText(string toSet)
    {
        text.text = toSet;
    }
}
