using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatUnit : MonoBehaviour
{
    public int HP, maxHP, MP, maxMP, attack, defense, speed;
    public List<StatusEffect> statusEffectList;
    public Animator animator;
    public bool targetable;
    public new string name;
    public int accuracy, evasion;

    protected virtual void Start()
    {
        gameObject.SetActive(true);
        animator = gameObject.GetComponent<Animator>();
        targetable = true;
    }

    public bool IsDead()
    {
        return this.HP <= 0;
    }

    public virtual void TakeDamage(CombatUnit attacker, int damage)
    {
        animator.SetTrigger("hit");
        this.HP -= damage;
        this.OnTakingDamage(damage);
        if (this.IsDead())
        {
            this.HP = 0;
            this.PlayDeadAnimation();
            attacker.OnKill();
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

    public virtual void OnKill()
    {
        return;
    }
    public virtual void OnTakingDamage(int damage)
    {
        return;
    }

    public static int CalculateDamage(CombatUnit attacker, CombatUnit target, int baseDamage)
    {
        bool accCheck = Random.Range(0, 100) < attacker.accuracy;
        bool evadeCheck = Random.Range(0, 100) >= target.evasion;
        if (accCheck && evadeCheck)
        {
            int damage = (int)(attacker.GetAttack() * (float)baseDamage / 100 * Random.Range(0.95f, 1.05f));
            int index = target.statusEffectList.FindIndex(e => e.GetType() == typeof(Sturdy));
            if (index >= 0)
            {
                damage = 0;
                target.statusEffectList.RemoveAt(index);
            }
            return damage;
        }
        return -1;
    }
}
