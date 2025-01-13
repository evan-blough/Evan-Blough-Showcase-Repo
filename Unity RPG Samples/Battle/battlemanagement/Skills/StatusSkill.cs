using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "New Status Skill", menuName = "Skills/Status Skill")]
public class StatusSkill : Skill
{
    public override List<string> UseSkill(Character character, List<Character> targets, int turnCounter)
    {
        List<string> results = new List<string>();
        if (applySelfStatuses.Count > 0)
        {
            foreach (var status in applySelfStatuses)
                SelfStatusApplication(character, status, turnCounter);
        }

        if (removeSelfStatuses.Count > 0)
        {
            foreach (var status in removeSelfStatuses)
                SelfStatusCure(character, status, turnCounter);
        }

        foreach (var target in targets)
        {
            if (!target.isActive)
            {
                results.Add("");
                continue;
            }

            if (applyTargetStatuses.Count > 0)
            {
                foreach (var status in applyTargetStatuses)
                {
                    results.Add(TargetStatusApplication(character, status, target, turnCounter));
                }
            }
            if (removeTargetStatuses.Count > 0)
            {
                foreach (var status in removeTargetStatuses)
                    TargetStatusRemoval(character, status, target, turnCounter);
            }
        }
        return results;
    }
}
