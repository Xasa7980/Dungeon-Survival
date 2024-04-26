using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategories
{
    Weapon,
    Armor,
    Accesory,
    Recipe,
    CraftPiece,
    CraftRune
}
[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/item Category")]
public class ItemCategory : ItemBase
{
    public bool isSecondaryWeapon;
    public ItemCategories itemCategories; 
}
