using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doom : StatusEffect
{
    public Doom(int turns, CombatUnit unit)
    {
        this.remainingTurn = turns;
        this.unit = unit;
        this.statusPrefab = Resources.Load<GameObject>(path + "Doom");
        this.TakeEffect(unit);
    }

    public override void RemoveEffect()
    {
        unit.TakeDamage(null, unit.HP);
    }
}
