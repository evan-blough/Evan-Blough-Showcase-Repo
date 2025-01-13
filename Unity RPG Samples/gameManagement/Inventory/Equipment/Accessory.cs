using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Accessory", menuName = "Equipment/Accessory")]
public class Accessory : Equipment
{
    public List<Status> statusResistances;
    public List<Status> statusImmunities;
}
