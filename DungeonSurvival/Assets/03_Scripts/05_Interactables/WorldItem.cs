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
    public void SetItem(Item item )
    {
        _item = item;
    }
    public void ChangeItemReference(Item item) => _item = item;

    [SerializeField, GUIColor(0.3f, 0.8f, 0.8f, 1f)] private bool hasDissolveEffect;
    [SerializeField,ShowIf("hasDissolveEffect")] private float appearMaxTime = 1.5f;
    [SerializeField, ShowIf("hasDissolveEffect")] private float dissolveSpeed = 3;
    [SerializeField, ShowIf("hasDissolveEffect")] private Material dissolveMaterialShader; //En el caso de querer incorporar un material de dissolve en caso de que no este puesto
    [SerializeField, ShowIf("hasDissolveEffect")] private Renderer renderer;
    public void SetRenderer ( Renderer renderer )
    {
        this.renderer = renderer;
    }
    private Shader_Dissolve shader_Dissolve;

    public override void Start ( )
    {
        base.Start();
        shader_Dissolve = new Shader_Dissolve();
        if(hasDissolveEffect )
        {
            renderer.material = dissolveMaterialShader;
        }
    }
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

            if(!hasDissolveEffect) gameObject.SetActive(false); // hay objetos 2D y 3D no es bueno usar un dissolve para manipular el desactivamiento
            else
            {
                HideCanvasImmediately();
                DissolveGameObject();
            }
        }
        else
        {
            if (PlayerInventory.current.TryAddItem(_item))
            {
                if (!hasDissolveEffect) gameObject.SetActive(false);
                else
                {
                    HideCanvasImmediately();
                    DissolveGameObject();
                }
            }
            else
            {
                Debug.Log("Inventory is full");
                _canInteract = true;
            }
        }
    }
    private void DissolveGameObject ( )
    {
        shader_Dissolve.DissolveGameObject(dissolveSpeed, 0, renderer, gameObject, this);
    }
}
