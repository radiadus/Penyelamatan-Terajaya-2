using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sturdy : StatusEffect
{
    public Sturdy(CombatUnit unit)
    {
        statusPrefab = Resources.Load<GameObject>(path + "Sturdy");
        remainingTurn = int.MaxValue;
        this.TakeEffect(unit);
    }
}
