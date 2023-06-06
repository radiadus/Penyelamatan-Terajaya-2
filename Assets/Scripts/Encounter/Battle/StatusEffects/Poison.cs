using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    public int dot;

    public override void DecreaseTurn(CombatUnit unit)
    {
        this.unit.HP -= dot;
        base.DecreaseTurn(unit);
    }

}
