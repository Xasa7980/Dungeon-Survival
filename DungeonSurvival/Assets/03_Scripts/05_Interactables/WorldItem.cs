using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Variante del item que se coloca en el mundo
/// </summary>
public class WorldItem : Interactable
{
    [PropertyOrder(-10), SerializeField] Item _item;
    public Item item => _item;

    public void ChangeItemReference(Item item) => _item = item;

    public override void FinishInteraction()
    {
        base.FinishInteraction();

        if (_item.canBeEquiped && _item.autoEquip)
        {
            switch (_item)
            {
                case Item_Backpack backpack:
                    PlayerInventory.current.EquipBackpack(backpack);
                    break;
            }

            Destroy(gameObject);
        }
        else
        {
            if (PlayerInventory.current.TryAddItem(_item))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full");
                _canInteract = true;
            }
        }
    }
}
