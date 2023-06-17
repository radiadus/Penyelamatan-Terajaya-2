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
        accuracy = 100;
        evasion = 0;
        baseStats = new BaseStats(attack, defense, speed, accuracy, evasion);
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
        maxHP += stats.hpGrowth;
        maxMP += stats.mpGrowth;
        attack += stats.attackGrowth;
        defense += stats.defenseGrowth;
        speed += stats.speedGrowth;
        stats.level++;
        if (stats.attackBonusLevel > 0 && stats.level % stats.attackBonusLevel == 0) attack++;
        if (stats.defenseBonusLevel > 0 && stats.level % stats.defenseBonusLevel == 0) defense++;
        if (stats.speedBonusLevel > 0 && stats.level % stats.speedBonusLevel == 0) speed++;
        stats.requiredExp = (int)(stats.requiredExp * 1.25f);
        SetStats();
    }

    public virtual void GainExp(int exp)
    {
        stats.exp += exp;
        while (stats.exp >= stats.requiredExp)
        {
            stats.exp -= stats.requiredExp;
            this.LevelUp();
        }
        return;
    }

    

}
