using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEffectType { NONE, ATTACK, HEAL, RECOVER_SP, APPLY_STATUS, REMOVE_STATUS, REVIVE, GAG }
public class Items : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public bool isKeyItem;
    public int itemPrice;
    public ItemEffects itemEffect;

    public virtual List<string> UseItem(List<Character> targets, int turnCounter)
    {
        List<string> results = new List <string>();

        switch (itemEffect.itemType)
        {
            case ItemEffectType.ATTACK:
                results = itemEffect.AttackEffect(targets);
                break;
            case ItemEffectType.HEAL:
                results = itemEffect.HealEffect(targets);
                break;
            case ItemEffectType.RECOVER_SP:
                results = itemEffect.RecoverSPEffect(targets);
                break;
            case ItemEffectType.APPLY_STATUS:
                results = itemEffect.ApplyStatusEffect(targets, turnCounter);
                break;
            case ItemEffectType.REMOVE_STATUS:
                results = itemEffect.RemoveStatusEffect(targets);
                break;
            case ItemEffectType.REVIVE:
                results = itemEffect.ReviveEffect(targets);
                break;
            case ItemEffectType.GAG:
                results = itemEffect.GagEffect(targets);
                break;
            default:
                return new List<string>();
        }
        return results;
    }
}
