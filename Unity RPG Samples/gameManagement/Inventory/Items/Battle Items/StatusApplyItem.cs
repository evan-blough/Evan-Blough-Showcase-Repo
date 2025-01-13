using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status Apply Item", menuName = "Items/BattleItem/Status Apply Item")]
public class StatusApplyItem : BattleItem
{

    public override List<string> UseItem(List<Character> targets, int turnCounter)
    {
        List<string> result = new List<string>();
        Statuses newStatus;

        foreach (Character target in targets)
        {
            if (!target.isActive)
            {
                result.Add("");
                continue;
            }

            foreach (Statuses status in statuses)
            {
                if (target.immunities.Any(s => s == status.status)) { result.Add("Immune"); }

                else if (target.resistances.Any(s => s == status.status) && Random.Range(0, 1) == 0) { result.Add("Resisted"); }

                else if (status.accuracy * 100 < Random.Range(1, 100)) { result.Add("Missed"); }

                else
                {
                    if (target.currStatuses.Any(s => s.status == status.status))
                    {
                        newStatus = new Statuses(target.currStatuses.First(s => s.status == status.status));
                        target.currStatuses.RemoveAll(s => s.status == status.status);
                        newStatus.expirationTurn = turnCounter + status.expirationTurn;
                    }
                    else
                    {
                        newStatus = new Statuses(status);
                        newStatus.expirationTurn += turnCounter;
                    }
                    target.currStatuses.Add(newStatus);
                }
            }
        }

        return result;
    }
}
