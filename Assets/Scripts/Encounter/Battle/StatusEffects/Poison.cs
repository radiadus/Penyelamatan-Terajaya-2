using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    public int dot;

    public override int DecreaseTurn()
    {
        int damage = (int)(unit.HP * (float)dot / 100);
        if (damage == 0) damage = 1;
        unit.TakeDamage(null, damage);
        dot -= 5;
        base.DecreaseTurn();
        return damage;
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
