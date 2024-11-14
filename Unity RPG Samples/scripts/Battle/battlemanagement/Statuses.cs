using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status { POISONED, PARALYZED, BLEED, VULNERABLE, BERSERK, AFRAID, DEFENDING, DEATH }
[System.Serializable]
public class Statuses
{
    public Status status;
    public int expirationTurn;
    public bool canBeCured;

    public Statuses(Status status, int expirationTurn)
    {
        this.status = status;
        this.expirationTurn = expirationTurn;
    } 

    public static void HandleStatuses(Character character, int turn)
    {
        character.currStatuses.RemoveAll(s => s.expirationTurn == turn);
    }
}
