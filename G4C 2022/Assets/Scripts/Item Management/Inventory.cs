using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory
{
    public static event Action InventoryUpdated;
    public readonly int MaxItems = 5;

    public int ItemsInInventory { get; private set; }

    public SortedDictionary<Item, int> Items { get; private set; }

    public Inventory()
    {
        ItemsInInventory = 0;
        Items = new SortedDictionary<Item, int>();
    }

    public void Add(Item item)
    {
        if (ItemsInInventory == MaxItems)
        {
            return;
        }

        if (Items.ContainsKey(item))
        {
            Items[item] = Items[item] + 1;
        } 
        else
        {
            Items.Add(item, 1);
        }
       
        ItemsInInventory++;
        InventoryUpdated?.Invoke();
    }

    public void Remove(Item item)
    {
        Items[item] = Items[item] - 1;
        if (Items[item] == 0)
        {
            Items.Remove(item);
        }
        ItemsInInventory--;
        InventoryUpdated?.Invoke();
    }
}
