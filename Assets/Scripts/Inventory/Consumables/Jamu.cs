using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jamu : Consumable
{
    public override int Use(CombatUnit user, List<CombatUnit> targets)
    {
        CombatUnit target = targets[0];
        int heal = target.maxHP - target.HP;
        heal = heal < 100 ? heal : 100;
        target.HP += heal;
        Inventory inventory = GameManager.Instance.inventory;
        inventory.removeItem(inventory.items.Find(instance => instance.item == this), 1);
        return heal;
    }

}
