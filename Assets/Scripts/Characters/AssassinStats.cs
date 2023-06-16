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
        maxMP = 20;
        MP = maxMP;
        attack = 20;
        defense = 10;
        speed = 15;
        skillList = new List<Skill>
        {
            new Sembunyi(),
            new Tusuk(),
            new BomAsap(),
            new PisauBeracun(),
            new SeribuTusukan(),
            new BomBeracun(),
            new TusukanMematikan(),
            new BomMolotov()
        };
    }

    public override void InitializeSkills()
    {
        skillList = new List<Skill>
        {
            new Sembunyi(),
            new Tusuk(),
            new BomAsap()
        };

        if (PlayerPrefs.GetInt("assassin2", 0) == 1)
        {
            //initialize tier 2 assassin skills
            skillList.Add(new PisauBeracun());
            skillList.Add(new SeribuTusukan());
            skillList.Add(new BomBeracun());
        }
        if (PlayerPrefs.GetInt("assassin3", 0) == 1)
        {
            //initialize tier 3 assassin skills
            skillList.Add(new TusukanMematikan());
            skillList.Add(new BomMolotov());
        }
    }
}
