using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBlue : Enemy
{

    protected override void Start()
    {
        base.Start();
    }
    public override int Attack(CombatUnit user, List<CombatUnit> targets)
    {
        targets.ForEach(target =>
        {
            if (target.IsDead() || !target.targetable)
            {
                targets.Remove(target);
            }
        });
        if (targets.Count > 0)
        {
            int target = Random.Range(0, targets.Count);
            this.attackTarget = target;
            CombatUnit targetUnit = targets[target];
            int baseDamage = 100;
            int damage = (int)(GetAttack() * ((float)baseDamage / 100) * Random.Range(0.95f, 1.05f));
            animator.SetTrigger("attack");
            targetUnit.TakeDamage(damage);
            return damage;
        }
        return -2;
    }

    public override Enemy InitializeStats()
    {
        this.maxHP = 100;
        this.HP = 100;
        this.maxMP = 100;
        this.MP = 100;
        this.attack = 10;
        this.defense = 5;
        this.speed = 5;
        return this;
    }
}

