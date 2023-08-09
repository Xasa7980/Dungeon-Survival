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

    public override void FinishInteraction()
    {
        base.FinishInteraction();

        if (PlayerInventory.TryAddItem(_item))
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
