using System;

[Serializable]
public class ClassExpHandler
{
    public int nextLevelExp;
    public void AddClassExperience(PlayerCharacter character, int amount)
    {
        character.currClass.classXp += amount;
        CheckForClassLevelUp(character);
    }

    public void CheckForClassLevelUp(PlayerCharacter character)
    {

        while (character.currClass.classXp >= nextLevelExp && !character.currClass.isMaxed)
        {
            character.currClass.classXp -= nextLevelExp;
            character.currClass.classLevel++;
            character.currClass.OnClassLevelUp(character);

            if (!character.currClass.isMaxed)
            {
                UpdateClassLevel(character);
            }
        }
    }
    public void UpdateClassLevel(PlayerCharacter character)
    {
        nextLevelExp = character.currClass.currLevel.expNeeded;
    }
}
