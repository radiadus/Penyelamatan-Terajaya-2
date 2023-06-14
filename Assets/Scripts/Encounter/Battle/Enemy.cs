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
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetNextAnimatorStateInfo(0).length);
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length + animator.GetNextAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }

    public abstract Enemy InitializeStats();

    public abstract int Attack(CombatUnit user, List<CombatUnit> targets);

    public override void OnKill()
    {
        return;
    }
}
