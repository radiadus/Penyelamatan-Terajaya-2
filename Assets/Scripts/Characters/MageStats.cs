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
            new BolaApi(),
            new BolaApi()
        };
    }

    public override void InitializeSkills()
    {
        skillList = new List<Skill>();
        skillList.Add(new BolaApi());
        skillList.Add(new BolaApi());
        if (PlayerPrefs.GetInt("mage2", 0) == 1)
        {
            //initialize tier 2 mage skills
        }
        if (PlayerPrefs.GetInt("mage3", 0) == 1)
        {
            //initialize tier 3 mage skills
        }
    }
}
