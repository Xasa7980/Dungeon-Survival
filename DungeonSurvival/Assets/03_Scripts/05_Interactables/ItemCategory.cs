using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentCategory
{
    Weapon,
    Armor,
    Accesory
}
[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/Item Category")]
public class ItemCategory : ItemBase
{
    public EquipmentCategory equipmentCategory;
}
