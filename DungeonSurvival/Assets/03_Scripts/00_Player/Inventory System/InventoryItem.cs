using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item item { get; private set; }

    public bool stackable => item.stackable;
    int maxStack => item.maxStack;
    public int currentStack = 0;

    InventoryItem_UI_Layout slot;

    public bool isFull => !(currentStack < maxStack);

    public InventoryItem(Item item, InventoryItem_UI_Layout slot)
    {
        this.item = item;
        this.slot = slot;
        currentStack = 1;
    }

    /// <summary>
    /// Añade un item al stack siempre y cuando este no este lleno
    /// </summary>
    /// <returns>False si el stack esta lleno</returns>
    public bool TryAdd()
    {
        if (currentStack < maxStack)
        {
            currentStack++;
            slot.UpdateStack(this);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Remueve in item del stack y indica si este quedo vacío
    /// </summary>
    /// <returns>False si el stack esta vacio</returns>
    public bool TryRemove()
    {
        currentStack--;
        slot.UpdateStack(this);

        return currentStack > 0;
    }
}
