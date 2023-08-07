using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/Item")]
public class Item : ItemBase
{
    [SerializeField] ItemCategory _category;
    public ItemCategory category => _category;

    [SerializeField] bool _stackable = false;
    public bool stackable => _stackable;
    [ShowIf("_stackable"), SerializeField] int _maxStack = 3;
    public int maxStack => _maxStack;

    /// <summary>
    /// Crea una instancia del item para modificarlo y no afectar el Asset
    /// </summary>
    /// <returns>Instancia del item</returns>
    public Item CreateInstance()
    {
        return Instantiate(this);
    }
}
