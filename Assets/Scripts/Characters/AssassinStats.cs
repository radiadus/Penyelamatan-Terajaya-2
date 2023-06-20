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
            new BomAsap()
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
        int savedLevel = PlayerPrefs.GetInt("assassin", 1);
        for (int i = 2; i <= savedLevel; i++)
        {
            friendly.LevelUp();
        }
        friendly.HP = PlayerPrefs.GetInt("assassinHP");
        friendly.MP = PlayerPrefs.GetInt("assassinMP");
        friendly.GainExp(PlayerPrefs.GetInt("assassinExp"));
        int equipmentLevel = PlayerPrefs.GetInt("keris");
        equipment.enhanceLevel = equipmentLevel;
        for(int i = 1; i <= equipmentLevel; i++)
        {
            equipment.attackStat += 2;
            equipment.enhancePrice += 1000;
        }
    }
}
