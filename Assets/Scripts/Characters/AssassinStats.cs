using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class AssassinStats : Stats
{
    public override void Reset()
    {
        base.Reset();
        maxHP = 100;
        HP = maxHP;
        maxMP = 40;
        MP = maxMP;
        attack = 15;
        defense = 10;
        speed = 15;
        skillList = new List<Skill>
        {

        };
    }

    public override void InitializeSkills()
    {
        skillList = new List<Skill>();
        if (PlayerPrefs.GetInt("assassin2", 0) == 1)
        {
            //initialize tier 2 assassin skills
        }
        if (PlayerPrefs.GetInt("assassin3", 0) == 1)
        {
            //initialize tier 3 assassin skills
        }
    }
}
