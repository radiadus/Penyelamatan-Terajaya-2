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
        maxHP = 135;
        HP = maxHP;
        maxMP = 15;
        MP = maxMP;
        attack = 10;
        defense = 25;
        speed = 7;
        skillList = new List<Skill>
        {
            new P3K(),
            new Potong(),
            new Provokasi()
        };
    }

    public override void InitializeSkills()
    {
        Reset();
        skillList = new List<Skill>
        {
            new P3K(),
            new Potong(),
            new Provokasi()
        };

        if (PlayerPrefs.GetInt("warrior2", 0) == 1)
        {
            //initialize tier 2 warrior skills
            skillList.Add(new SemangatPemuda());
            skillList.Add(new TebasanLebar());
            skillList.Add(new PosisiJaga());
        }
        if (PlayerPrefs.GetInt("warrior3", 0) == 1)
        {
            //initialize tier 3 warrior skills
            skillList.Add(new SeranganBalik());
            skillList.Add(new AmukanPahlawan());
        }
        int savedLevel = PlayerPrefs.GetInt("warrior", 1);
        for (int i = 2; i <= savedLevel; i++)
        {
            friendly.LevelUp();
        }
        friendly.HP = PlayerPrefs.GetInt("warriorHP");
        friendly.MP = PlayerPrefs.GetInt("warriorMP");
        friendly.GainExp(PlayerPrefs.GetInt("warriorExp"));
        int equipmentLevel = PlayerPrefs.GetInt("pedang");
        equipment.enhanceLevel = equipmentLevel;
        for (int i = 1; i <= equipmentLevel; i++)
        {
            equipment.attackStat += 2;
            equipment.enhancePrice += 1000;
        }
    }
}