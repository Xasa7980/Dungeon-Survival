using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipedItem_UI_Layout : MonoBehaviour
{
    [SerializeField] Image icon;

    [SerializeField] ItemCategory _category;
    public ItemCategory category => _category;

    Item equipedItem;
    public bool empty => equipedItem == null;

    private void Start()
    {
        icon.sprite = category.icon;
        Color color = icon.color;
        color.a = 0.5f;
        icon.color = color;
    }

    public void Equip(Item item)
    {
        equipedItem = item;
        icon.sprite = equipedItem.icon;
        Color color = icon.color;
        color.a = 1;
        icon.color = color;
    }

    public void Unequip()
    {
        equipedItem = null;
        icon.sprite = category.icon;
        Color color = icon.color;
        color.a = 0.5f;
        icon.color = color;
    }
}
