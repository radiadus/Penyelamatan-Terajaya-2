using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class SwordsmanStats : Stats
{
    public override void Reset()
    {
        base.Reset();
        maxHP = 120;
        HP = maxHP;
        maxMP = 20;
        MP = maxMP;
        attack = 10;
        defense = 20;
        speed = 10;
        skillList = new List<Skill>
        {

        };
    }
}
