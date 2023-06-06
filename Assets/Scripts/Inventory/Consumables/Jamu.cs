using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jamu : Consumable
{
    public override void Use(CombatUnit user, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        target.HP += 100;
        target.CheckMaxHPMP();
    }

}
