using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : CombatUnit
{
    public int id;
    public enum AttackType
    {
        SINGLE,
        ALL
    }
    public AttackType attackType;
    public string attackTarget;
    public int goldGain;
    public int expGain;
    
    public override void PlayDeadAnimation()
    {
        if (animator!= null)
        {
            if (animator.runtimeAnimatorController!= null)
            {
                animator.SetBool("isDead", true);
                StartCoroutine(RemoveBody());
            }
        }
    }

    IEnumerator RemoveBody()
    {
        yield return new WaitForSecondsRealtime(3);
        gameObject.SetActive(false);
    }

    public virtual Enemy InitializeStats()
    {
        this.accuracy = 100;
        this.evasion = 0;
        this.baseStats = new BaseStats(this.attack, this.defense, this.speed, this.accuracy, this.evasion);
        this.statusEffectList = new List<StatusEffect>();
        return this;
    }

    public virtual int Attack(CombatUnit user, List<CombatUnit> targets)
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
            if (damage == -1) return -3;
            animator.SetTrigger("attack");
            targetUnit.TakeDamage(this, damage);
            return damage;
        }
        return -2;
    }

    
}
