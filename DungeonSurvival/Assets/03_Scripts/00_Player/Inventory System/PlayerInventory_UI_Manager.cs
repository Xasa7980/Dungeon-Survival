using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerInventory_UI_Manager : MonoBehaviour
{
    public static PlayerInventory_UI_Manager current { get; private set; }

    [SerializeField] GameObject inventoryBar;

    InventoryItem_UI_Layout[] slots;
    int _currentSelectedIndex = 0;
    public int currentSelectedIndex
    {
        get => _currentSelectedIndex;
        private set
        {
            _currentSelectedIndex = value;
            slots[_currentSelectedIndex].toggle.isOn = true;
        }
    }

    [FoldoutGroup("Equip Slots"), SerializeField] EquipedItem_UI_Layout primaryWeapon;
    [FoldoutGroup("Equip Slots"), SerializeField] EquipedItem_UI_Layout secundaryWeapon;
    [FoldoutGroup("Equip Slots"), SerializeField] EquipedItem_UI_Layout backpack;

    [FoldoutGroup("Backpack"), SerializeField] InventoryItem_UI_Layout slotPrefab;
    [FoldoutGroup("Backpack"), SerializeField] Transform backpackContainer;
    InventoryItem_UI_Layout[] backpackSlots;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        slots = inventoryBar.GetComponentsInChildren<InventoryItem_UI_Layout>();
        backpackSlots = backpackContainer.GetComponentsInChildren<InventoryItem_UI_Layout>();
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (currentSelectedIndex < slots.Length - 1)
                currentSelectedIndex++;
            else
                currentSelectedIndex = 0;
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (currentSelectedIndex > 0)
                currentSelectedIndex--;
            else
                currentSelectedIndex = slots.Length - 1;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!slots[currentSelectedIndex].empty)
            {
                switch (slots[currentSelectedIndex].item)
                {
                    case Item_Backpack backpack:
                        PlayerInventory.current.EquipBackpack(backpack);
                        break;
                }
            }
        }
    }

    public void SelectItem(int index) => currentSelectedIndex = index;

    public void AddItem(int index, InventoryItem item)
    {
        slots[index].SetItem(item);
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
                iItem.UpdateSlot(slot);
        }
    }

    public void AddItemToBackpack(int index, InventoryItem item)
    {
        backpackSlots[index].SetItem(item);
    }

    public InventoryItem_UI_Layout GetSlot(int index) => slots[index];
    public InventoryItem_UI_Layout GetBackpackSlot(int index) => backpackSlots[index];
}
