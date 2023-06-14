using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Untargetable : StatusEffect
{
    public override void TakeEffect(CombatUnit unit)
    {
        base.TakeEffect(unit);
        unit.targetable = false;
    }

    public override void RemoveEffect()
    {
        unit.targetable = true;
        base.RemoveEffect();
    }

    public Untargetable(int turns, CombatUnit unit)
    {
        this.remainingTurn = turns;
        this.statusPrefab = Resources.Load<GameObject>(path + "Untargetable");
        this.TakeEffect(unit);
    }
}
