using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerInventory_UI_Manager : MonoBehaviour
{
    public static PlayerInventory_UI_Manager current { get; private set; }

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

    [FoldoutGroup("Quick Equip Slots"), SerializeField] EquipedItem_UI_Layout primaryWeapon;
    [FoldoutGroup("Quick Equip Slots"), SerializeField] EquipedItem_UI_Layout secundaryWeapon;
    [FoldoutGroup("Quick Equip Slots"), SerializeField] EquipedItem_UI_Layout backpack;

    [FoldoutGroup("Backpack"), SerializeField] InventoryItem_UI_Layout slotPrefab;
    [FoldoutGroup("Backpack"), SerializeField] Transform backpackContainer;
    InventoryItem_UI_Layout[] backpackSlots;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        quickAccessSlots = inventoryQuickAccessBar.GetComponentsInChildren<InventoryItem_UI_Layout>();
        backpackSlots = backpackContainer.GetComponentsInChildren<InventoryItem_UI_Layout>();
        equipmentItem_UI_Slots = equipmentWindowsInventory.GetComponentsInChildren<EquipedItem_UI_Layout>();
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
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
                switch (quickAccessSlots[currentSelectedIndex].item)
                {
                    case Item_Backpack backpack: /* Si el item es de tipo Item_Backpack procede */
                        
                        PlayerInventory.current.EquipBackpack(backpack);
                        break;
                    case Item usableItem when usableItem.itemTag.tag == "Key" :

                        PlayerInventory.current.UseItem(usableItem); 
                        break;
                }
            }
        }
    }

    public void SelectItem(int index) => currentSelectedIndex = index;

    public void AddItem(int index, InventoryItem item)
    {
        quickAccessSlots[index].SetItemToInventory(item);
    }

    public void EquipBackpack(Item_Backpack backpack)
    {
        if (backpackSlots != null)
        {
            foreach(InventoryItem_UI_Layout s in backpackSlots)
            {
                if (s != null)
                    Destroy(s.gameObject);
            }
        }

        this.backpack.Equip(backpack);
        backpackSlots = new InventoryItem_UI_Layout[backpack.maxItems];

        for (int i = 0; i < backpackSlots.Length; i++)
        {
            InventoryItem iItem = backpack.allItems[i];

            InventoryItem_UI_Layout slot = Instantiate(slotPrefab, backpackContainer);
            slot.transform.Find("Key").gameObject.SetActive(false);
            backpackSlots[i] = slot;

            if (iItem != null)
                iItem.UpdateInventorySlot(slot);
        }
    }

    public void AddItemToBackpack(int index, InventoryItem item)
    {
        backpackSlots[index].SetItemToInventory(item);
    }

    public void EquipItemToEquipmentWindows ( Item item )
    {
        for (int i = 0; i < equipmentItem_UI_Slots.Length; i++)
        {
            if (equipmentItem_UI_Slots[i].itemCategory.equipmentCategory == item.equipmentDataSO.equipmentStats.equipmentCategory)
            {
                equipmentItem_UI_Slots[i].Equip(item);
            }
        }
    }
    public InventoryItem_UI_Layout GetSlot(int index) => quickAccessSlots[index];
    public InventoryItem_UI_Layout GetBackpackSlot(int index) => backpackSlots[index];
}
