using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : StatusEffect
{
    public int potency;
    public Burn(int turns, int potency, CombatUnit unit)
    {
        this.remainingTurn = turns;
        this.potency = potency;
        this.unit = unit;
        this.statusPrefab = Resources.Load<GameObject>(path + "Burn");
        this.TakeEffect(unit);
    }

    public override int DecreaseTurn()
    {
        unit.TakeDamage(null, potency);
        base.DecreaseTurn();
        return potency;
    }
}
