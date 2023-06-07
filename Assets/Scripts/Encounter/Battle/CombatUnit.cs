using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatUnit : MonoBehaviour
{
    public int HP, maxHP, MP, maxMP, attack, defense, speed;
    public List<StatusEffect> statusEffectList;
    public Animator animator;

    protected virtual void Start()
    {
        gameObject.SetActive(true);
        animator = gameObject.GetComponent<Animator>();
    }

    public bool IsDead()
    {
        if (animator == null) animator = gameObject.GetComponent<Animator>();
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
