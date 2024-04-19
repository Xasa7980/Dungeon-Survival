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

        if (_item.equipable && _item.autoEquip)
        {
            switch (_item)
            {
                case Item_Backpack backpack:
                    PlayerInventory.current.EquipBackpack(backpack);
                    break;
            }

            gameObject.SetActive(false); // hay objetos 2D y 3D no es bueno usar un dissolve para manipular el desactivamiento
        }
        else
        {
            if (PlayerInventory.current.TryAddItem(_item))
            {
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Inventory is full");
                _canInteract = true;
            }
        }
    }
}
