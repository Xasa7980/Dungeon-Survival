using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Tag", menuName = "Dungeon Survival/Inventory/ItemTag")]
public class ItemTag : ScriptableObject
{
    [SerializeField] string _itemCategoryTag;
    [SerializeField] string _itemTag;
    ItemTagLibrary itemTag => new ItemTagLibrary(_itemCategoryTag,this);
    public ItemTagLibrary GetItemTagLibrary => itemTag;
    public string GetCategoryTag => itemTag.GetTag();
    public string GetItemTag => _itemTag;
}
