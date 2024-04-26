using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_InventoryItemList_Layout : MonoBehaviour, IPointerUpHandler
{
    public iItemData item { get; private set; }

    [SerializeField] Text itemNameText;
    [SerializeField] Text amount;

    public GameObject equipedLabel;
    public bool equiped { get => item.equiped; }

    public Toggle toggle { get; private set; }

    public static UI_InventoryItemList_Layout CreateInstance(UI_InventoryItemList_Layout reference, iItemData item, Transform parent = null)
    {
        UI_InventoryItemList_Layout instance = Instantiate(reference, parent);
        instance.item = item;

        instance.itemNameText.text = item.displayName;
        instance.amount.text = "x" + item.currentAmount;

        instance.toggle = instance.GetComponent<Toggle>();
        instance.toggle.group = parent.GetComponent<ToggleGroup>();
        instance.toggle.onValueChanged.AddListener(instance.Select);

        instance.equipedLabel.SetActive(item.equiped);

        return instance;
    }

    protected virtual void Select(bool value)
    {
        if (value)
        {
            UI_Inventory_Panel.current.SelectItem((Item)item);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(1))
        {
            UI_Inventory_Panel.hoveredItem = item;
            UI_Inventory_Panel.current.OpenItemActionMenu(Input.mousePosition);
        }
    }

    public void Equip()
    {
        item.EquipStats();
        equipedLabel.SetActive(true);
    }

    public void Unequip()
    {
        item.UnequipStats();
        equipedLabel.SetActive(false);
    }
}