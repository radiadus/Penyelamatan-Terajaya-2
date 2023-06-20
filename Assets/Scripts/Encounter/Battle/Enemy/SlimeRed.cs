using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRed : Enemy
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
            int burnPotency = (int)(user.GetAttack() * 0.5f);
            foreach(CombatUnit target in availableTargets)
            {
                int damage = CombatUnit.CalculateDamage(user, target, baseDamage);
                if (damage == -1)
                {
                    continue;
                }
                target.TakeDamage(user, damage);
                bool success = Random.Range(0, 100) < 20;
                if (success)
                {
                    new Burn(2, burnPotency, target);
                }
                totalDamage += damage;
            }
            return totalDamage;
        }
        return -2;
    }

}

