using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamuEnergi : Consumable
{
    public override void Use(CombatUnit user, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        target.MP += 50;
        target.CheckMaxHPMP();
    }
}
