using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Provoke : StatusEffect
{
    public Provoke(CombatUnit unit)
    {
        this.statusPrefab = Resources.Load<GameObject>(path + "Provoke");
        this.unit = unit;
        this.remainingTurn = 2;
        this.TakeEffect(unit);
    }
}
