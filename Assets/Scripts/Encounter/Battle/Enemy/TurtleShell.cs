using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : Enemy
{
    public override void Attack(CombatUnit user, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        int baseDamage = 20;
        int damage = (int)(GetAttack()*(baseDamage/100)*Random.Range(0.95f, 1.05f));
        target.TakeDamage(damage);
    }

}
