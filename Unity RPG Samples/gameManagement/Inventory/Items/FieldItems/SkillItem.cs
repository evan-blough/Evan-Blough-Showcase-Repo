using System.Collections.Generic;

public class SkillItem : FieldItem
{
    public Skill skillToAdd;

    public override List<string> UseItemInField(List<PlayerCharacterData> targets)
    {
        List<string> returns = new List<string>();

        foreach (PlayerCharacterData target in targets)
        {
            if (target.skills.Contains(skillToAdd))
            {
                returns.Add("I already know that.");
                continue;
            }

            target.skills.Add(skillToAdd);
            if (target.equippedSkills.Count < target.skillCount)
            {
                target.equippedSkills.Add(skillToAdd);
            }
            returns.Add("This could be nice...");
        }

        return new List<string>();
    }
}
