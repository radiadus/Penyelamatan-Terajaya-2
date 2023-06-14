using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : Friendly
{
    public override void InitializeStats()
    {
        base.InitializeStats();
        new Sturdy(this);
    }

    public override void OnTakingDamage(int damage)
    {
        int index = this.statusEffectList.FindIndex(e => e.GetType() == typeof(Sturdy));
        if (index >= 0)
        {
            this.HP += damage;
            this.statusEffectList[index].RemoveEffect();
        }
    }
}
