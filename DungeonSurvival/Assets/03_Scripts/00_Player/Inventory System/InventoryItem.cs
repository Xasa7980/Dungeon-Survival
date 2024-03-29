using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item item { get; private set; }

    public bool stackable => item.isStackable;
    int maxStack => item.stackAmount;
    public int currentStack = 0;

    InventoryItem_UI slot;

    public bool isFull => !(currentStack < maxStack);

    public InventoryItem(Item item, InventoryItem_UI slot)
    {
        this.item = item;
        this.slot = slot;
        currentStack = 1;
    }

    public void UpdateInventorySlot( InventoryItem_UI _slot )
    {
        InventoryItem_UI_Layout tempSlot = _slot as InventoryItem_UI_Layout;
        this.slot = tempSlot;
        tempSlot.SetItemToInventory(this);
    }
    public void UpdateEquipmentSlot( InventoryItem_UI _slot )
    {
        EquipedItem_UI_Layout tempSlot = _slot as EquipedItem_UI_Layout;
        this.slot = tempSlot;
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
        tempSlot.UpdateStack(this);

        return currentStack > 0;
    }
    //public void TrySwapEquipment ( InventoryItem_UI _slot )
    //{
    //    InventoryItem_UI_Layout tempInventorySlot;
    //    EquipedItem_UI_Layout tempEquipmentInventorySlot;

    //    if(_slot as InventoryItem_UI_Layout != null)
    //    {
    //        tempInventorySlot = _slot as InventoryItem_UI_Layout;
    //        if (!tempInventorySlot.empty)
    //        {
    //            Item tempItem = tempInventorySlot.item;
    //            tempInventorySlot.SetItem(item);
    //            item = tempItem;
    //            tempInventorySlot.UpdateStack(this);
    //            return;
    //        }
    //        item = tempInventorySlot.item;
    //        tempInventorySlot.UpdateStack(this);
    //    }
    //    if (_slot as EquipedItem_UI_Layout != null)
    //    {
    //        tempEquipmentInventorySlot = _slot as EquipedItem_UI_Layout;
    //    }

    //    return;
    //}
}
