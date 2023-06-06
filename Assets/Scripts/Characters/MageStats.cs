using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class MageStats : Stats
{
    public override void Reset()
    {
        base.Reset();
        maxHP = 80;
        HP = maxHP;
        maxMP = 50;
        MP = maxMP;
        attack = 20;
        defense = 10;
        speed = 10;
        skillList = new List<Skill>
        {
            new BolaApi()
        };
    }
}
