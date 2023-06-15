using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    public int dot;

    public override void DecreaseTurn()
    {
        unit.HP -= (int)(unit.HP * (float)dot / 100);
        dot -= 5;
        base.DecreaseTurn();
    }

    public Poison(int dot, CombatUnit unit)
    {
        this.dot = dot;
        this.unit = unit;
        this.TakeEffect(unit);
    }
}
