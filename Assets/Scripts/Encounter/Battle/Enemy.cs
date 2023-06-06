using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : CombatUnit
{
    public int id;
    public override void PlayDeadAnimation()
    {
        throw new System.NotImplementedException();
    }

    public abstract void Attack(CombatUnit user, List<CombatUnit> target);
}
