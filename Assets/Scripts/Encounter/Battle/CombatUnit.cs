using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatUnit : MonoBehaviour
{
    public int HP, maxHP, MP, maxMP, attack, defense, speed;
    public List<StatusEffect> statusEffectList;
    protected Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsDead()
    {
        if (this.HP <= 0) return true;
        return false;
    }

    public virtual void TakeDamage(int damage)
    {
        this.HP -= damage;
        if (this.IsDead())
        {
            this.HP = 0;
            this.PlayDeadAnimation();
        }
    }

    public void CheckMaxHPMP()
    {
        if (HP > maxHP) HP = maxHP;
        if (MP > maxMP) MP = maxMP;
    }

    public abstract void PlayDeadAnimation();

    public virtual int GetAttack()
    {
        return attack;
    }
}
