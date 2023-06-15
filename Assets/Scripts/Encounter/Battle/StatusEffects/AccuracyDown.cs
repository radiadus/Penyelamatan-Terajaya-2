using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AccuracyDown : StatusEffect
{
    public int potency;
    public AccuracyDown(int turns, int potency, CombatUnit unit)
    {
        this.statusPrefab = Resources.Load<GameObject>(path + "AccDown");
        this.remainingTurn = turns;
        this.unit = unit;
        this.potency = potency;
        this.TakeEffect(unit);
    }

    public override void TakeEffect(CombatUnit unit)
    {
        unit.accuracy -= potency;
        unit.statusEffectList.Add(this);
    }

    public override void RemoveEffect()
    {
        unit.accuracy = 100;
        base.RemoveEffect();
    }
}
