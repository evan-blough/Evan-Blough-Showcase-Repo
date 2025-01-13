using System.Collections.Generic;
using System.Linq;

public class ClassItem : FieldItem
{
    public ClassSlot classToAdd;

    public override List<string> UseItemInField(List<PlayerCharacterData> targets)
    {
        List<string> returns = new List<string>();
        bool shouldBreak = false;

        foreach (PlayerCharacterData target in targets)
        {
            shouldBreak = false;
            foreach (Class knownClass in target.classes)
            {
                if (knownClass.classSlot == classToAdd)
                {
                    returns.Add("I've already got this!");
                    shouldBreak = true;
                }
            }

            if (!shouldBreak)
            {
                returns.Add("All right! This is gonna be good!");
                var addedClass = new Class(classToAdd);
                target.classes.Add(addedClass);

                if (target.equippedClass.isMaxed)
                {
                    target.equippedClass = target.classes.Where(c => c == addedClass).First();
                }
            }
        }

        return returns;
    }
}
