using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatViolet : Enemy
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
            CombatUnit targetUnit;
            int provokeIndex = availableTargets.FindIndex(t => t.statusEffectList.Exists(e => e.GetType() == typeof(Provoke)));
            if (provokeIndex >= 0)
            {
                targetUnit = availableTargets[provokeIndex];
            }
            else
            {
                int target = Random.Range(0, availableTargets.Count);
                targetUnit = availableTargets[target];
            }
            this.attackTarget = targetUnit.name;
            int baseDamage = 100;
            int damage = CombatUnit.CalculateDamage(user, targetUnit, baseDamage);
            animator.SetTrigger("attack");
            if (damage == -1) return -3;
            targetUnit.TakeDamage(this, damage);
            bool success = Random.Range(0, 100) < 50;
            if (success)
            {
                new Poison(3, 15, targetUnit);
            }
            return damage;
        }
        return -2;
    }

}

