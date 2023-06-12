using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatGreen : Enemy
{
    public override void Attack(CombatUnit user, List<CombatUnit> target)
    {
        target[0].TakeDamage(5);
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
