using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status Removal Item", menuName = "Items/BattleItem/Status Removal Item")]
public class StatusRemovalItem : BattleItem
{
    public override List<string> UseItem(List<Character> targets, int turnCounter)
    {
        List<string> result = new List<string>();

        foreach (Character target in targets)
        {
            if (!target.isActive)
            {
                result.Add("");
                continue;
            }

            foreach (Statuses status in statuses)
            {
                if (target.currStatuses.Any(s => s.status == status.status))
                {
                    target.currStatuses.RemoveAll(s => s.status == status.status);
                }
            }
        }

        return result;
    }
}
