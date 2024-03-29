using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Tag", menuName = "Dungeon Survival/Inventory/ItemTag")]
public class ItemTag : ScriptableObject
{
    [SerializeField] string _itemTag;
    ItemTagLibrary _itemTagLibrary = new ItemTagLibrary();
    public string tag => _itemTagLibrary.SetTag(_itemTag);
    public string typeName => _itemTag;
}
