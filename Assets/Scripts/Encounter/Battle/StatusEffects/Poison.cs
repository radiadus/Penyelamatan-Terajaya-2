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

    public Poison(int turns, int dot, CombatUnit unit)
    {
        this.remainingTurn = turns;
        this.dot = dot;
        this.unit = unit;
        this.statusPrefab = Resources.Load<GameObject>(path + "Poison");
        this.TakeEffect(unit);
    }
}
