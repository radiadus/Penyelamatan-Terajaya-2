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
        int counterIndex = this.statusEffectList.FindIndex(e => e.GetType() == typeof(Counter));
        if (counterIndex >= 0)
        {
            ((Counter)this.statusEffectList[counterIndex]).potency += 25;
        }
    }
}
