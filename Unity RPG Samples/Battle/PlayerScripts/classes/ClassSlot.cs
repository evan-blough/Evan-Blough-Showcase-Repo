using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Class", menuName = "Classes/Class")]
public class ClassSlot : ScriptableObject
{
    public string className;
    public int classAtkBuff;
    public int classDefBuff;
    public int classMgAtkBuff;
    public int classMgDefBuff;
    public int classAgilityBuff;

    public List<ClassLevel> levels;
}
