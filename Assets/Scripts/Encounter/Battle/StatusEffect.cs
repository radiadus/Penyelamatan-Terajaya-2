using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public CombatUnit unit;
    public int remainingTurn;
    public virtual void TakeEffect(CombatUnit unit)
    {
        this.unit.statusEffectList.Add(this);
    }
    public virtual void DecreaseTurn(CombatUnit unit)
    {
        this.remainingTurn--;
        if (this.remainingTurn == 0)
        {
            this.RemoveEffect(unit);
        }
    }
    public virtual void RemoveEffect(CombatUnit unit)
    {
        this.unit.statusEffectList.Remove(this);
    }
}
