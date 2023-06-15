using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseUp : StatusEffect
{
    public int potency;
    public DefenseUp(int turns, int potency, CombatUnit unit)
    {
        this.unit = unit;
        this.remainingTurn = turns;
        this.potency = potency;
        this.statusPrefab = Resources.Load<GameObject>(path + "DefUp");
        this.TakeEffect(unit);
    }

    public override void TakeEffect(CombatUnit unit)
    {
        int index = unit.statusEffectList.FindIndex(e => e.GetType() == this.GetType() || e.GetType() == typeof(DefenseDown));
        if (index >= 0)
        {
            unit.statusEffectList[index].RemoveEffect();
        }
        this.unit = unit;
        unit.defense += (int)(unit.defense * (float)potency / 100);
        unit.statusEffectList.Add(this);
    }

    public override void RemoveEffect()
    {
        unit.defense = unit.baseStats.defense;
        base.RemoveEffect();
    }
}
