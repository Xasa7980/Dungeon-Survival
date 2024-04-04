using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item item { get; private set; }
    public bool stackable => item.isStackable;
    int maxStack => item.currentStack;
    public int currentStack = 0;

    public InventoryItem_UI GetSlot => slot;
    InventoryItem_UI slot;

    public bool isFull => !(currentStack < maxStack);

    public InventoryItem(Item item, InventoryItem_UI slot)
    {
        this.item = item;
        this.slot = slot;
        currentStack = 1;
    }

    public void UpdateInventorySlot( InventoryItem_UI targetSlot, EventHandler<SlotChecker> extraAction )
    {
        InventoryItem_UI_Layout tempSlot = targetSlot as InventoryItem_UI_Layout;
        this.slot = (InventoryItem_UI)tempSlot;
        if(currentStack <= 0 )
        {
        }
        tempSlot.SetItemToInventory(this);
    }
    public void UpdateEquipmentSlot( InventoryItem_UI targetSlot, EventHandler<SlotChecker> extraAction )
    {
        EquipedItem_UI_Layout tempSlot = targetSlot as EquipedItem_UI_Layout;
        this.slot = (InventoryItem_UI)tempSlot;
        extraAction?.Invoke(this, new SlotChecker
        {
            inventoryItem = this
        });
        tempSlot.SetItemToEquipmentWindow(this);
    }

    /// <summary>
    /// Añade un item al stack siempre y cuando este no este lleno
    /// </summary>
    /// <returns>False si el stack esta lleno</returns>
    public bool TryAdd()
    {
        if (currentStack < maxStack)
        {
            InventoryItem_UI_Layout tempSlot = slot as InventoryItem_UI_Layout;
            currentStack++;
            tempSlot.UpdateStack(this);
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
        InventoryItem_UI_Layout tempSlot = slot as InventoryItem_UI_Layout;
        currentStack--;
        if(currentStack <= 0)
        {
            tempSlot.RemoveItem_UI(this);
        }
        tempSlot.UpdateStack(this);

        return currentStack > 0;
    }
}
public class SlotChecker
{
    public InventoryItem inventoryItem;
}