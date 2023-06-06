using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Stats : ScriptableObject
{
    public int HP, maxHP;
    public int MP, maxMP;
    public int attack;
    public int defense;
    public int speed;
    public int level, exp, requiredExp;
    public List<Skill> skillList;
    public List<StatusEffect> statusEffectList;
    public bool isDead;

    public virtual void Reset()
    {
        level = 1;
        exp = 0;
        requiredExp = 100;
    }
}
