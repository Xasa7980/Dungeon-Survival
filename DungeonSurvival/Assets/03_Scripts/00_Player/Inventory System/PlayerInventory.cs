using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    static PlayerInventory current;

    [SerializeField] int maxItems = 9;
    [SerializeField] InventoryItem[] allItems;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        allItems = new InventoryItem[maxItems];
    }

    /// <summary>
    /// Tratar de agregar un item nuevo al inventario
    /// </summary>
    /// <param name="item">Item a agregar</param>
    /// <returns>Falso si el inventario esta lleno</returns>
    public static bool TryAddItem(Item item)
    {
        if (item.stackable)
        {
            InventoryItem[] stacks = current.allItems.Where(i => i != null && i.item == item).ToArray();
            InventoryItem stack = stacks.FirstOrDefault(i => !i.isFull);

            if(stack != null)
            {
                if (stack.TryAdd())
                {
                    return true;
                }
            }

            for (int i = 0; i < current.allItems.Length; i++)
            {
                if (current.allItems[i] == null)
                {
                    current.allItems[i] = new InventoryItem(item, PlayerInventory_UI_Manager.current.GetSlot(i));
                    PlayerInventory_UI_Manager.current.AddItem(i, current.allItems[i]);
                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < current.allItems.Length; i++)
            {
                if (current.allItems[i] == null)
                {
                    current.allItems[i] = new InventoryItem(item, PlayerInventory_UI_Manager.current.GetSlot(i));
                    PlayerInventory_UI_Manager.current.AddItem(i, current.allItems[i]);
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Intentar remover un item espeifico del inventario
    /// </summary>
    /// <param name="item">Item a remover</param>
    /// <returns>Falso si el item no existe en el inventario</returns>
    public static bool TryRemoveItem(Item item)
    {
        InventoryItem instance = current.allItems.FirstOrDefault(i => i.item == item);

        if(instance != null)
        {
            if (instance.stackable)
            {
                if (!instance.TryRemove())
                {
                    for (int i = 0; i < current.allItems.Length; i++)
                    {
                        if (current.allItems[i] == instance)
                        {
                            current.allItems[i] = null;
                            PlayerInventory_UI_Manager.current.AddItem(i, null);
                            return true;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < current.allItems.Length; i++)
                {
                    if (current.allItems[i] == instance)
                    {
                        current.allItems[i] = null;
                        PlayerInventory_UI_Manager.current.AddItem(i, null);
                        return true;
                    }
                }
            }

            return true;
        }

        return false;
    }
}
