using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : StatusEffect
{
    public int potency = 0;
    public Counter(int turns, CombatUnit unit)
    {
        this.remainingTurn = turns;
        this.unit = unit;
        this.TakeEffect(unit);
    }

    public override void RemoveEffect()
    {
        new AttackUp(1, potency, unit);
        base.RemoveEffect();
    }
}
