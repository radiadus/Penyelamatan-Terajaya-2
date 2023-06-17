using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public List<ItemInstance> items = new();
    private int maxQuantity = 99;
    private int newItemId = 1;
    public int money = 100;
    
    public bool addItem(Item item, int quantity)
    {
        foreach (ItemInstance itemInstance in items)
        {
            if (itemInstance.item.id == item.id)
            {
                if (itemInstance.quantity+quantity > maxQuantity)
                {
                    quantity -= maxQuantity - itemInstance.quantity;
                    itemInstance.quantity = maxQuantity;
                    continue;
                }
                itemInstance.quantity += quantity;
                return true;
            }
        }
        while (quantity > maxQuantity)
        {
            addToList(item, maxQuantity);
            quantity -= maxQuantity;
        }
        addToList(item, quantity);
        return true;
    }

    private void addToList(Item item, int quantity)
    {
        ItemInstance newItem = new ItemInstance();
        newItem.id = newItemId;
        newItemId++;
        newItem.item = item;
        newItem.quantity = quantity;
        items.Add(newItem);
    }

    public ItemInstance FindItemInstance(Type itemType)
    {
        return this.items.Find(i => i.GetType() == itemType);
    }

    public bool removeItem(ItemInstance item, int quantity)
    {
        foreach (ItemInstance itemInstance in items)
        {
            if (itemInstance.id == item.id)
            {
                if (itemInstance.quantity < quantity)
                {
                    return false;
                }
                itemInstance.quantity -= quantity;
                if (itemInstance.quantity == 0)
                {
                    items.Remove(itemInstance);
                }
                return true;
            }
        }
        return false;
    }

    public void emptyInventory()
    {
        items = new List<ItemInstance>();
        newItemId = 1;
        money = 100;
    }
}
