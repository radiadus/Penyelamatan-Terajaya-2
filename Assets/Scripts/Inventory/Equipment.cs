using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Equipment : ScriptableObject
{
    public int id;
    public string equipmentName;
    public int baseAttack, attackStat, enhanceLevel;

    public void Reset()
    {
        this.enhanceLevel = 0;
        this.attackStat = this.baseAttack;
    }
}
