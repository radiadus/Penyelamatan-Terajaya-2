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
        heal = heal < 50 ? heal : 50;
        target.HP += heal;
        ((Friendly)target).SetStats();
        inventory.removeItem(instance, 1);
        return heal;
    }

    public override int UseInventory(Stats user)
    {
        Inventory inventory = GameManager.Instance.inventory;
        ItemInstance instance = inventory.FindItemInstance(this.GetType());
        if (instance == null || instance.quantity == 0) return -2;
        if (user.HP == 0) return -2;
        int heal = user.maxHP - user.HP;
        heal = heal < 50 ? heal : 50;
        user.HP += heal;
        inventory.removeItem(instance, 1);
        return heal;
    }
}
