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
        maxMP = 30;
        MP = maxMP;
        attack = 25;
        defense = 5;
        speed = 10;
        skillList = new List<Skill>
        {
            new BolaApi(),
            new Debus(),
            new Kilat(),
            new Penyembuhan(),
            new SemburApi(),
            new Jimat(),
            new Petir(),
            new BadaiApi(),
            new SakratulMaut(),
            new Bangkit()
        };
    }

    public override void InitializeSkills()
    {
        skillList = new List<Skill>
        {
            new BolaApi(),
            new Debus(),
            new Kilat(),
            new Penyembuhan()
        };

        if (PlayerPrefs.GetInt("mage2", 0) == 1)
        {
            //initialize tier 2 mage skills
            skillList.Add(new SemburApi());
            skillList.Add(new Jimat());
            skillList.Add(new Petir());
        }
        if (PlayerPrefs.GetInt("mage3", 0) == 1)
        {
            //initialize tier 3 mage skills
            skillList.Add(new BadaiApi());
            skillList.Add(new SakratulMaut());
            skillList.Add(new Bangkit());
        }
    }
}
