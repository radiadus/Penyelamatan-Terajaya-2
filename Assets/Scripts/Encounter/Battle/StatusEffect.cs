using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public GameObject statusPrefab;
    public int remainingTurn;
    public CombatUnit unit;
    public string path = "Prefab/UI/Encounter/Debuffs and Buffs/";
    public virtual void TakeEffect(CombatUnit unit)
    {
        this.unit = unit;
        unit.statusEffectList.Add(this);
    }
    public virtual void DecreaseTurn()
    {
        this.remainingTurn--;
        if (this.remainingTurn == 0)
        {
            RemoveEffect();
        }
    }
    public virtual void RemoveEffect()
    {
        unit.statusEffectList.Remove(this);
    }
}
