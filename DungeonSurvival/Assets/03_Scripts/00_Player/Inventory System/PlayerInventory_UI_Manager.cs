using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System;

public class PlayerInventory_UI_Manager : MonoBehaviour
{
    public static PlayerInventory_UI_Manager current { get; private set; }
    public event EventHandler<SlotChecker> OnAnySlotChanged;

    public KeyCode itemUseKey = KeyCode.Return;

    [SerializeField] GameObject inventoryQuickAccessBar;

    InventoryItem_UI_Layout[] quickAccessSlots;
    EquipedItem_UI_Layout[] equipmentItem_UI_Slots;

    int _currentSelectedIndex = 0;
    public int currentSelectedIndex
    {
        get => _currentSelectedIndex;
        private set
        {
            _currentSelectedIndex = value;
            quickAccessSlots[_currentSelectedIndex].toggle.isOn = true;
        }
    }
    [FoldoutGroup("Equipment Windows"), SerializeField] GameObject equipmentWindowsInventory;

    [FoldoutGroup("Quick EquipUI Slots"), SerializeField] EquipedItem_UI_Layout primaryWeapon;
    [FoldoutGroup("Quick EquipUI Slots"), SerializeField] EquipedItem_UI_Layout secundaryWeapon;
    [FoldoutGroup("Quick EquipUI Slots"), SerializeField] EquipedItem_UI_Layout backpack;

    [FoldoutGroup("Backpack"), SerializeField] InventoryItem_UI_Layout slotPrefab;
    [FoldoutGroup("Backpack"), SerializeField] Transform backpackContainer;
    InventoryItem_UI_Layout[] backpackSlots;

    private void Awake ( )
    {
        current = this;
    }

    private void Start ( )
    {
        quickAccessSlots = inventoryQuickAccessBar.GetComponentsInChildren<InventoryItem_UI_Layout>();
        backpackSlots = backpackContainer.GetComponentsInChildren<InventoryItem_UI_Layout>();
        equipmentItem_UI_Slots = equipmentWindowsInventory.GetComponentsInChildren<EquipedItem_UI_Layout>();
        OnAnySlotChanged += PlayerInventory_UI_Manager_OnAnySlotChanged;
    }

    private void PlayerInventory_UI_Manager_OnAnySlotChanged ( object sender, SlotChecker e )
    {
        InventoryItem inventoryItem = sender as InventoryItem;

        if (inventoryItem.currentStack <= 0)
        {
            PlayerInventory.current.TryRemoveItem((Item)e.inventoryItem.item, 0);
        }
    }

    private void Update ( )
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (currentSelectedIndex < quickAccessSlots.Length - 1)
                currentSelectedIndex++;
            else
                currentSelectedIndex = 0;
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (currentSelectedIndex > 0)
                currentSelectedIndex--;
            else
                currentSelectedIndex = quickAccessSlots.Length - 1;
        }

        if (Input.GetKeyDown(itemUseKey))
        {
            if (!quickAccessSlots[currentSelectedIndex].empty)
            {
                Debug.Log("Current slot is not empty");
                switch (quickAccessSlots[currentSelectedIndex].item)
                {
                    case Item_Backpack backpack: /* Si el item es de tipo Item_Backpack procede */

                        Debug.Log("Accessed to item");
                        PlayerInventory.current.EquipBackpack(backpack);
                        break;
                    case Item usableItem when usableItem.itemTag.GetTag == "Key":

                        Debug.Log("Accessed to item");
                        PlayerInventory.current.UseItem(usableItem);
                        break;
                    case Item usableItem when usableItem.itemTag.GetTag == "Food":

                        Debug.Log("Accessed to item");
                        PlayerInventory.current.UseItem(usableItem);
                        break;
                    case Item usableItem when usableItem.itemTag.GetTag == usableItem.itemTag.GetItemTagLibrary.statBoostTags[0]:

                        //Debug.Log("Accessed to item");
                        PlayerInventory.current.UseItem(usableItem);
                        break;
                }
            }
        }
    }

    public void SelectItem ( int index ) => currentSelectedIndex = index;

    public void AddItemToQuickAccess ( int index, InventoryItem item )
    {
        quickAccessSlots[index].SetItemToInventory(item);
    }
    public void AddItemToBackpack ( int index, InventoryItem item )
    {
        backpackSlots[index].SetItemToInventory(item);
    }
    public void EquipItemToEquipmentWindows ( Item item )
    {
        for (int i = 0; i < equipmentItem_UI_Slots.Length; i++)
        {
            if (equipmentItem_UI_Slots[i].itemCategory.itemCategories == item.equipmentDataSO.equipmentStats.equipmentCategory)
            {
                equipmentItem_UI_Slots[i].EquipUI(item);
            }
        }
    }

    public void EquipBackpack ( Item_Backpack backpack )
    {
        if (backpackSlots != null)
        {
            foreach (InventoryItem_UI_Layout s in backpackSlots)
            {
                if (s != null)
                    Destroy(s.gameObject);
            }
        }

        this.backpack.EquipUI(backpack);
        backpackSlots = new InventoryItem_UI_Layout[backpack.maxItems];

        for (int i = 0; i < backpackSlots.Length; i++)
        {
            InventoryItem iItem = backpack.allItems[i];

            InventoryItem_UI_Layout slot = Instantiate(slotPrefab, backpackContainer);
            //slot.transform.Find("Key").gameObject.SetActive(false);
            backpackSlots[i] = slot;

            if (iItem != null)
                iItem.UpdateInventorySlot(slot, OnAnySlotChanged);
        }
    }

    public InventoryItem_UI_Layout GetSlot ( int index ) => quickAccessSlots[index];
    public InventoryItem_UI_Layout GetBackpackSlot ( int index ) => backpackSlots[index];
    public InventoryItem_UI_Layout GetFirstBackpackEmptySlot ( InventoryItem_UI inventoryItem_UI )
    {
        return backpackSlots.First(i => i.empty);
    }
    public int GetFirstBackpackEmptySlotIndex ( InventoryItem_UI inventoryItem_UI )
    {
        int index = Array.FindIndex(backpackSlots, i => i.empty);

        return index;
    }
}
[System.Serializable]
public class ItemTagLibrary
{
    public string itemTagString;
    public ItemTag itemTag;

    public string[] foodTags = new string[] { "Food" };
    public string[] healingTags = new string[] { "Healing_HP", "Healing_MP" };
    public string[] repairTags = new string[] { "Repair" };
    public string[] statBoostTags = new string[] { "StatBoost" };
    public string[] specialTags = new string[] { "SpecialItem" };
    public string[] equipmentTags = new string[] { "Equipment" };

    public ItemTagLibrary(string _itemTagString, ItemTag _itemTag )
    {
        itemTagString = _itemTagString;
        itemTag = _itemTag;
    }
}
public static class ItemTagLibraryExtensions
{
    public static string SetTag ( this ItemTagLibrary itemTagLibrary, string itemTag )
    {
        return itemTagLibrary.itemTagString = itemTag;
    }
    public static string GetTag ( this ItemTagLibrary itemTagLibrary )
    {
        if( string.IsNullOrWhiteSpace(itemTagLibrary.itemTagString) )
        {
            Debug.LogError("There is not SetTag in this itemTagLibrary");
            return null;
        }
        return itemTagLibrary.itemTagString;
    }
}