using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/itemDroppingGroup")]
public class ItemDroppingGroup : ScriptableObject
{
    [SerializeField] Item[] _itemsToDrop;
    public Item[] itemsToDrop => _itemsToDrop;
}
