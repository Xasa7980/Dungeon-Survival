using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;



[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/Item")]
public class Item : ItemBase
{
    [SerializeField] ItemCategory _category;
    [SerializeField] bool _stackable = false;
    [ShowIf("_stackable"), SerializeField] int _maxStack = 3;

    [SerializeField] WorldItem interactableModel;
    [SerializeField] GameObject visualizationModel;

    [FoldoutGroup("Equip"), SerializeField] bool _canBeEquiped;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("_canBeEquiped")] bool _autoEquip;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("_canBeEquiped")] 

    public ItemCategory category => _category;
    public bool stackable => _stackable;
    public int maxStack => _maxStack;
    public bool canBeEquiped => _canBeEquiped;
    public bool autoEquip => _autoEquip;

    /// <summary>
    /// Crea una instancia del item para modificarlo y no afectar el Asset
    /// </summary>
    /// <returns>Instancia del item</returns>
    public Item CreateInstance()
    {
        return Instantiate(this);
    }

    public WorldItem InstantiateInWorld(Vector3 position)
    {
        return Instantiate(interactableModel, position, Quaternion.Euler(Vector3.up * Random.Range(0, 360)));
    }

    public WorldItem InstantiateInWorld(Vector3 position, Item item)
    {
        WorldItem instance = Instantiate(interactableModel, position, Quaternion.Euler(Vector3.up * Random.Range(0, 360)));
        instance.ChangeItemReference(item);
        return instance;
    }

    public virtual void EquipOnPlayer(Transform holster)
    {
        Instantiate(visualizationModel, holster);
    }

    public virtual void UnequipOnPlayer(iInventory inventory)
    {
        if (inventory.TryAddItem(this, out int index))
        {

        }
    }

    public virtual void Use()
    {
        //do something
    }
}
