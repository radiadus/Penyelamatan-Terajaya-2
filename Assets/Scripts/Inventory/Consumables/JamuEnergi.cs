using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamuEnergi : Consumable
{
    public override int Use(CombatUnit user, List<CombatUnit> targets)
    {
        Inventory inventory = GameManager.Instance.inventory;
        ItemInstance instance = inventory.FindItemInstance(this.GetType());
        if (instance == null || instance.quantity == 0) return -2; 
        CombatUnit target = targets[0];
        if (target.IsDead()) return -2;
        int heal = target.maxMP - target.MP;
        heal = heal < 25 ? heal : 25;
        target.MP += heal;
        ((Friendly)target).SetHPMP();
        inventory.removeItem(instance, 1);
        return heal;
    }

    public override int UseInventory(Stats user)
    {
        Inventory inventory = GameManager.Instance.inventory;
        ItemInstance instance = inventory.FindItemInstance(this.GetType());
        if (instance == null || instance.quantity == 0) return -2;
        int heal = user.maxMP - user.MP;
        heal = heal < 25 ? heal : 25;
        user.MP += heal;
        inventory.removeItem(instance, 1);
        return heal;
    }
}
