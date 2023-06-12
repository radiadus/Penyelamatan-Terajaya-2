using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : StatusEffect
{
    public int dot;

    public override void DecreaseTurn()
    {
        unit.HP -= (int)(unit.HP * dot / 100);
        dot -= 5;
        base.DecreaseTurn();
    }

    public Poison()
    {
        dot = 15;
    }
}
