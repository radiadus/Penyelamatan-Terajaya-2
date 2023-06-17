using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public GameObject statusPrefab;
    public int remainingTurn;
    public CombatUnit unit;
    public string path = "Prefab/UI/Encounter/Debuffs and Buffs/";
    public virtual void TakeEffect(CombatUnit unit)
    {
        int index = unit.statusEffectList.FindIndex(e => e.GetType() == this.GetType());
        if (index >= 0)
        {
            unit.statusEffectList[index].RemoveEffect();
        }
        this.unit = unit;
        unit.statusEffectList.Add(this);
    }
    public virtual int DecreaseTurn()
    {
        this.remainingTurn--;
        if (this.remainingTurn == 0)
        {
            RemoveEffect();
        }
        return -1;
    }
    public virtual void RemoveEffect()
    {
        unit.statusEffectList.Remove(this);
    }
}
