using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDown : StatusEffect
{
    public int potency;
    public AttackDown(int turns, int potency, CombatUnit unit)
    {
        this.remainingTurn = turns;
        this.potency = potency;
        this.unit = unit;
        this.statusPrefab = Resources.Load<GameObject>(path + "AtkDown");
        this.TakeEffect(unit);
    }

    public override void TakeEffect(CombatUnit unit)
    {
        int index = unit.statusEffectList.FindIndex(e => e.GetType() == this.GetType() || e.GetType() == typeof(AttackUp));
        if (index >= 0)
        {
            unit.statusEffectList[index].RemoveEffect();
        }
        this.unit = unit;
        unit.attack -= (int)(unit.attack * (float)potency / 100);
        unit.statusEffectList.Add(this);
    }

    public override void RemoveEffect()
    {
        unit.attack = unit.baseStats.attack;
        base.RemoveEffect();
    }
}
