using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : CombatUnit
{
    public Stats stats;
    public Equipment equipment;

    protected override void Start()
    {
        base.Start();
    }

    public virtual void InitializeStats()
    {
        HP = stats.HP;
        maxHP = stats.maxHP;
        MP = stats.MP;
        maxMP = stats.maxMP;
        attack = stats.attack;
        defense = stats.defense;
        speed = stats.speed;
        statusEffectList = new List<StatusEffect>();
    }

    public void SetStats()
    {
        stats.HP = HP;
        stats.maxHP = maxHP;
        stats.MP = MP;
        stats.maxMP = maxMP;
        stats.attack = attack;
        stats.defense = defense;
        stats.speed = speed;
    }

    public override void PlayDeadAnimation()
    {
        animator.SetBool("isDead", true);
    }

    public virtual void LevelUp()
    {
        stats.level++;
        stats.requiredExp = (int)(stats.requiredExp * 1.5f);
    }

    public virtual void GainExp(int exp)
    {
        stats.exp += exp;
        if (stats.exp > stats.requiredExp)
        {
            stats.exp -= stats.requiredExp;
            this.LevelUp();
        }
        return;
    }

}
