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
            new Penyembuhan()
        };
    }

    public override void InitializeSkills()
    {
        this.Reset();
        skillList = new List<Skill>
        {
            new BolaApi(),
            new Debus(),
            new Kilat(),
            new Penyembuhan()
        };
        if (PlayerPrefs.GetInt("mage2", 0) == 1)
        {
            skillList.Add(new SemburApi());
            skillList.Add(new Jimat());
            skillList.Add(new Petir());
        }
        if (PlayerPrefs.GetInt("mage3", 0) == 1)
        {
            skillList.Add(new BadaiApi());
            skillList.Add(new SakratulMaut());
            skillList.Add(new Bangkit());
        }
        int savedLevel = PlayerPrefs.GetInt("mage", 1);
        Debug.Log(savedLevel);
        Debug.Log(maxHP);
        for (int i = 2; i <= savedLevel; i++)
        {
            friendly.LevelUp();
        }
        Debug.Log(maxHP);
        HP = PlayerPrefs.GetInt("mageHP");
        MP = PlayerPrefs.GetInt("mageMP");
        friendly.GainExp(PlayerPrefs.GetInt("mageExp"));
        equipment.Reset();
        int equipmentLevel = PlayerPrefs.GetInt("tongkat");
        equipment.enhanceLevel = equipmentLevel;
        for (int i = 1; i <= equipmentLevel; i++)
        {
            equipment.attackStat += 2;
            equipment.enhancePrice += 1000;
        }
    }
}
