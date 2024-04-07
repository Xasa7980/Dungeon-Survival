using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item Tag", menuName = "Dungeon Survival/Inventory/ItemTag")]
public class ItemTag : ScriptableObject
{
    [SerializeField] string _itemTag;
    ItemTagLibrary itemTag => new ItemTagLibrary(_itemTag,this);
    public string StTag => itemTag.SetTag(_itemTag);
    public ItemTagLibrary GetItemTagLibrary => itemTag;
    public string GetTag => itemTag.GetTag();
}
