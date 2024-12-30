using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon")]
public class Weapon : Equipment
{
    public Elements element;
    public List<Statuses> statuses;
    public void ApplyWeaponStatuses(Character enemy, int turnCounter)
    {
        foreach (var status in statuses)
        {
            Statuses newStatus = new Statuses(status);
            if (enemy.immunities.Any(s => s == newStatus.status))
            {
                continue;
            }
            else if (!enemy.resistances.Any(s => s == newStatus.status) || Random.Range(0, 1) == 1)
            {
                if (newStatus.accuracy * 100 >= Random.Range(1, 100))
                {
                    if (enemy.currStatuses.FirstOrDefault(s => s.status == newStatus.status) != null)
                    {
                        enemy.currStatuses.RemoveAll(s => s.status == newStatus.status);
                    }

                    newStatus.expirationTurn += turnCounter;
                    enemy.currStatuses.Add(newStatus);

                    if (newStatus.status == Status.DEATH)
                    {
                        enemy.currHP = 0;
                        enemy.isActive = false;
                    }
                }
            }
        }
    }
}
