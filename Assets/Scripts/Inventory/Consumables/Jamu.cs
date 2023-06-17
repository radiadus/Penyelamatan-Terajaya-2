using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jamu : Consumable
{
    public override int Use(CombatUnit user, List<CombatUnit> targets)
    {
        Inventory inventory = GameManager.Instance.inventory;
        ItemInstance instance = inventory.FindItemInstance(this.GetType());
        if (instance == null || instance.quantity == 0) return -2;
        CombatUnit target = targets[0];
        if (target.IsDead()) return -2;
        int heal = target.maxHP - target.HP;
        heal = heal < 100 ? heal : 100;
        target.HP += heal;
        ((Friendly)target).SetStats();
        inventory.removeItem(instance, 1);
        return heal;
    }

}
