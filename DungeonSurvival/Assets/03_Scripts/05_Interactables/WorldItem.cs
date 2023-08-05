using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Variante del item que se coloca en el mundo
/// </summary>
public class WorldItem : Interactable
{
    public override void FinishInteraction()
    {
        base.FinishInteraction();
        Destroy(gameObject);
    }
}
