using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu]
public class WarriorStats : Stats
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

    public override void InitializeSkills()
    {
        skillList = new List<Skill>();
        if (PlayerPrefs.GetInt("warrior2", 0) == 1)
        {
            //initialize tier 2 warrior skills
        }
        if (PlayerPrefs.GetInt("warrior3", 0) == 1)
        {
            //initialize tier 3 warrior skills
        }
    }
}