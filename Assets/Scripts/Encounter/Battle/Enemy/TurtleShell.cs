using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : Enemy
{

    protected override void Start()
    {
        base.Start();
    }
    public override void Attack(CombatUnit user, List<CombatUnit> targets)
    {
        targets.ForEach(target =>
        {
            if (target.IsDead() || target.targetable)
            {
                targets.Remove(target);
            }
        });
        CombatUnit target = targets[Random.Range(0, targets.Count)];    
        int baseDamage = 20;
        int damage = (int)(GetAttack()*(baseDamage/100)*Random.Range(0.95f, 1.05f));
        target.TakeDamage(damage);
    }

    public override Enemy InitializeStats()
    {
        this.maxHP = 100;
        this.HP = 100;
        this.maxMP = 100;
        this.MP = 100;
        this.attack = 5;
        this.defense = 5;
        this.speed = 5;
        return this;
    }
}
