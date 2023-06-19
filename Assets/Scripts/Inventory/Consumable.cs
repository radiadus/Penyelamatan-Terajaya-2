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
    public AudioClip clip;

    public abstract int Use(CombatUnit user, List<CombatUnit> target);
    public abstract int UseInventory(Stats user);
}
