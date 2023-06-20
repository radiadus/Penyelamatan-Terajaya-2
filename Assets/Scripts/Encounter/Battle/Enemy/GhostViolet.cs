using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostViolet : Enemy
{

    protected override void Start()
    {
        base.Start();
    }
    public override int Attack(CombatUnit user, List<CombatUnit> targets)
    {
        List<CombatUnit> availableTargets = targets.FindAll(t => !t.IsDead() && t.targetable);
        if (availableTargets.Count > 0)
        {
            int totalDamage = 0;
            int baseDamage = 100;
            animator.SetTrigger("attack");
            foreach (CombatUnit target in availableTargets)
            {
                int damage = CombatUnit.CalculateDamage(user, target, baseDamage);
                if (damage == -1)
                {
                    continue;
                }
                target.TakeDamage(user, damage);
                totalDamage += damage;
            }
            return totalDamage;
        }
        return -2;
    }

}

