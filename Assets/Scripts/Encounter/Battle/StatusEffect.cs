using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public Sprite sprite;
    public int remainingTurn;
    public CombatUnit unit;
    public virtual void TakeEffect(CombatUnit unit)
    {
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
