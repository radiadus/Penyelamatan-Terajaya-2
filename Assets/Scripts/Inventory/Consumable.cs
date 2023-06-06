using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : Item
{
    private enum Target
    {
        ALLY,
        ALL_ALLIES,
        ENEMY,
        ALL_ENEMIES
    }
    [SerializeField] private Target itemTarget;

    public abstract void Use(CombatUnit user, List<CombatUnit> target);
}
