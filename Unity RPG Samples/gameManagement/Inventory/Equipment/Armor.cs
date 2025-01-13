using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Equipment/Armor")]
public class Armor : Equipment
{
    public List<Elements> elemResists;
    public List<Elements> elemWeaknesses;
    public List<Elements> elemAbsorption;
}
