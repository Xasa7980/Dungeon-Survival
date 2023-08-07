using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        slots = inventoryBar.GetComponentsInChildren<InventoryItem_UI_Layout>();
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
    }

    public void SelectItem(int index) => currentSelectedIndex = index;

    public void AddItem(int index, InventoryItem item)
    {
        slots[index].SetItem(item);
    }

    public InventoryItem_UI_Layout GetSlot(int index) => slots[index];
}
