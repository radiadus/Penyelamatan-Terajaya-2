using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamuEnergi : Consumable
{
    public override int Use(CombatUnit user, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        int heal = target.maxMP - target.MP;
        heal = heal < 50 ? heal : 50;
        target.MP += heal;
        Inventory inventory = GameManager.Instance.inventory;
        inventory.removeItem(inventory.items.Find(instance => instance.item == this), 1);
        return heal;
    }
}
