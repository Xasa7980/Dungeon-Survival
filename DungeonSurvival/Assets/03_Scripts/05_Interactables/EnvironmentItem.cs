using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentItem : Interactable
{
    [SerializeField] private bool hasItemDroppingChance = false;
    [SerializeField] private ItemDroppingGroup itemDroppingGroup;
    [SerializeField, ShowIf("hasItemDroppingChance")] private float itemDroppingChance;


    [SerializeField] private float dropRadius = 2;
    [SerializeField] private int[] droppingRate;

    [SerializeField] private float appearMaxTime = 1.5f;
    [SerializeField] private float dissolveSpeed = 1;
    [SerializeField] private Renderer renderer;
    public void  SetRenderer(Renderer renderer )
    {
        this.renderer = renderer;
    }
    private Shader_Dissolve shader_Dissolve;

    public override void Start ( )
    {
        base.Start();
        if (hasItemDroppingChance && !_lockInteraction) _lockInteraction = itemDroppingChance > Random.Range(0, 101);

        shader_Dissolve = new Shader_Dissolve();
        if(renderer == null)
        {
            renderer = transform.GetComponentInChildren<Renderer>() ;
        }
        CalculateDroppingRates();
    }
    public override void StartInteraction ( )
    {
        base.StartInteraction();
        ApplyHit();
    }
    public override void FinishInteraction ( )
    {
        base.FinishInteraction();
        DropItemsBasedOnProbability();
    }

    private void ApplyHit ( )
    {
        interactionHits--;
        if(interactionHits <= 0)
        {
            HideCanvasImmediately();
            DissolveGameObject();
        }
    }
    private void CalculateDroppingRates ( )
    {
        if (itemDroppingGroup == null) return;
        if (itemDroppingGroup.itemsToDrop.Length == 0) return;

        int totalItems = itemDroppingGroup.itemsToDrop.Length;
        droppingRate = new int[totalItems];

        for (int i = 0; i < totalItems; i++)
        {
            int basePercent = 100 - ((i + 1) * (100 / totalItems * i - 1)); // Decrecimiento equitativo basado en el índice
            int qualityAdjustment = Random.Range(1, 6); // Factor aleatorio que afectará la caída basado en la calidad

            // Ajustar la probabilidad en base a la calidad del ítem. Mayor calidad reduce más el porcentaje.
            int finalPercent = Mathf.Max(basePercent - (qualityAdjustment * itemDroppingGroup.itemsToDrop[i].itemQualityLevel), 0);

            droppingRate[i] = finalPercent;

        }
    }

    private void DropItemsBasedOnProbability ( )
    {
        if (interactionHits <= 0) return;

        int dropCount = Random.Range(1, 4); // De 1 a 3 items

        for (int i = 0; i < dropCount; i++)
        {
            // Selecciona y suelta el item basado en la probabilidad
            Item itemToDrop = SelectItemToDrop();
            if (itemToDrop != null)
            {
                DropItem(itemToDrop);
            }
        }
    }

    private Item SelectItemToDrop ( )
    {
        int totalRate = 0;
        foreach (var rate in droppingRate)
        {
            totalRate += rate;
        }

        int randomPoint = Random.Range(0, totalRate);

        // Determina que item soltar basado probabilidad
        for (int i = 0; i < droppingRate.Length; i++)
        {
            if (randomPoint <= droppingRate[i])
            {
                return itemDroppingGroup.itemsToDrop[i];
            }
            else
            {
                randomPoint -= droppingRate[i];
            }
        }

        return null;
    }
    private void DissolveGameObject ( )
    {
        shader_Dissolve.DissolveGameObject(dissolveSpeed,0, renderer, gameObject, this);
    }
    private void DropItem ( Item item )
    {
        Item itemDropped = item.CreateInstance();
        Vector2 circle = Random.insideUnitCircle;
        float factor = Mathf.Sqrt(circle.x * circle.x + circle.y * circle.y); // Magnitud del vector
        circle = circle.normalized * Mathf.Lerp(0.1f * dropRadius, dropRadius, factor); // Reescalando usando Lerp

        Vector3 dropPosition = new Vector3(circle.x, 0, circle.y) + transform.position;
        dropPosition.y = 0;
        itemDropped.InstantiateInWorld(dropPosition, this);
    }
}
