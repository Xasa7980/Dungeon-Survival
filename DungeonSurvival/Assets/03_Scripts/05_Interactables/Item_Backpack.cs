using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/Backpack")]
public class Item_Backpack : Item, iInventory
{
    [SerializeField] int _maxItems = 4;
    public int maxItems => _maxItems;

    public InventoryItem[] allItems { get; private set; }

    public void Init()
    {
        if (allItems == null)
            allItems = new InventoryItem[maxItems];
    }
}
