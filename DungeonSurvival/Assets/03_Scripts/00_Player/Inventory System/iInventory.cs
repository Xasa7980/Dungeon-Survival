using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface iInventory
{
    public InventoryItem[] allItems { get; }
}

public static class iInventoryExtensions
{
    /// <summary>
    /// Tratar de agregar un item nuevo al inventario
    /// </summary>
    /// <param name="item">Item a agregar</param>
    /// <returns>Falso si el inventario esta lleno</returns>
    public static bool TryAddItem(this iInventory inventory, Item item, out int index, bool backpack = false)
    {
        if (item.isStackable)
        {
            InventoryItem[] stacks = inventory.allItems.Where(i => i != null && i.item == item).ToArray();
            InventoryItem stack = stacks.FirstOrDefault(i => !i.isFull);

            if (stack != null)
            {
                if (stack.TryAdd())
                {
                    index = -1;
                    return true;
                }
            }

            for (int i = 0; i < inventory.allItems.Length; i++)
            {
                if (inventory.allItems[i] == null)
                {
                    if (!backpack)
                        inventory.allItems[i] = new InventoryItem(item, PlayerInventory_UI_Manager.current.GetSlot(i));
                    else
                        inventory.allItems[i] = new InventoryItem(item, PlayerInventory_UI_Manager.current.GetBackpackSlot(i));
                    //if (updateUI)
                    //    PlayerInventory_UI_Manager.current.AddItem(i, inventory.allItems[i]);
                    index = i;
                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < inventory.allItems.Length; i++)
            {
                if (inventory.allItems[i] == null)
                {
                    if (!backpack)
                        inventory.allItems[i] = new InventoryItem(item, PlayerInventory_UI_Manager.current.GetSlot(i));
                    else
                        inventory.allItems[i] = new InventoryItem(item, PlayerInventory_UI_Manager.current.GetBackpackSlot(i));
                    //if (updateUI)
                    //    PlayerInventory_UI_Manager.current.AddItem(i, inventory.allItems[i]);
                    index = i;
                    return true;
                }
            }
        }

        index = -1;
        return false;
    }
    /// <summary>
    /// Intentar remover un item espeifico del inventario
    /// </summary>
    /// <param name="item">Item a remover</param>
    /// <returns>Falso si el item no existe en el inventario</returns>
    public static bool TryRemoveItem(this iInventory inventory, Item item, out int index)
    {
        InventoryItem instance = inventory.allItems.FirstOrDefault(i => i.item == item); // Guardo el item que sea igual al item que pasaremos como parametro 

        if (instance != null) // Si ese item es diferente a nulo
        {
            if (instance.stackable) // Si ese item es stackable
            {
                if (!instance.TryRemove()) // Si le quitamos uno y se queda con 1 o 0
                {
                    for (int i = 0; i < inventory.allItems.Length; i++)
                    {
                        if (inventory.allItems[i] == instance)
                        {
                            inventory.allItems[i] = null;
                            //if (updateUI)
                            //    PlayerInventory_UI_Manager.current.AddItem(i, null);
                            index = i;
                            return true;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < inventory.allItems.Length; i++)
                {
                    if (inventory.allItems[i] == instance)
                    {
                        inventory.allItems[i] = null;
                        //if (updateUI)
                        //    PlayerInventory_UI_Manager.current.AddItem(i, null);
                        index = i;
                        return true;
                    }
                }
            }
        }

        index = -1;
        return false;
    }
}
public static class iItemDataExtensions
{
    public static bool IsTypeOf ( this iItemData item, System.Type type )
    {
        return item.GetType() == type;
    }

    public static bool IsType ( this iItemData item, ItemTag type )
    {
        return item.itemTag == type;
    }
}