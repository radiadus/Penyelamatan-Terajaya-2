using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : CombatUnit
{
    public Stats stats;
    public Equipment equipment;
    public List<QuestionCategory> questionCategories;
    

    void Start()
    {
        
    }

    public override void PlayDeadAnimation()
    {
        animator.SetBool("isDead", true);
    }

    public virtual void LevelUp()
    {
        stats.level++;
        stats.exp = 0;
        stats.requiredExp = (int)(stats.requiredExp * 1.5f);
    }

    public virtual void GainExp(int exp)
    {
        if (stats.exp + exp > stats.requiredExp)
        {
            stats.exp = exp - stats.requiredExp + stats.exp;
            this.LevelUp();
            stats.exp = exp;
            return;
        }
        stats.exp += exp;
    }

    public override int GetAttack()
    {
        return attack + equipment.attackStat;
    }

}
