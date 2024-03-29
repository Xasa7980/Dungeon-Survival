using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryItem_UI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler
{
    public virtual void OnBeginDrag ( PointerEventData eventData )
    {
    }
    public virtual void OnDrag ( PointerEventData eventData )
    {
    }

    public virtual void OnEndDrag ( PointerEventData eventData )
    {
    }
    public virtual void OnPointerClick ( PointerEventData eventData )
    {
    }

    public virtual void OnPointerEnter ( PointerEventData eventData )
    {
    }
}
public class EquipedItem_UI_Layout : InventoryItem_UI
{
    [SerializeField] Image icon;

    [SerializeField] ItemCategory _itemCategory;
    [SerializeField] Vector2 tempImageSize = new Vector2(100,100);
    public ItemCategory itemCategory => _itemCategory;

    Item _item;
    public Item item => _item;
    public void SetItem(Item item ) {  _item = item; }
    public bool empty => _item == null;

    private GameObject tempItemRepresentation;
    private InventoryItem_UI _tempTargetContainer;
    private Item tempItem;

    private void Start ( )
    {
        icon.sprite = itemCategory.icon;
        UpdateIconAlpha(0.5f);
    }
    public void SetItemToEquipmentWindow ( InventoryItem item )
    {
        if (item == null)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
            icon.sprite = item.item.icon;
        }

        _item = item == null ? null : item.item;
    }

    public void Equip ( Item item )
    {
        this._item = item;
        icon.sprite = this._item.icon;
        UpdateIconAlpha(1);
    }

    public void Unequip ( )
    {
        _item = null;
        icon.sprite = itemCategory.icon;
        UpdateIconAlpha(0.5f);
    }
    private void UpdateIconAlpha ( float alpha )
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }
    public override void OnBeginDrag ( PointerEventData eventData )
    {
        if (_item != null)
        {
            tempItemRepresentation = new GameObject("TempItem");
            tempItemRepresentation.transform.SetParent(transform.parent.parent.parent, false);
            tempItemRepresentation.transform.SetAsLastSibling(); // Para visualizarlo por encima de otros UIs
            Image tempImage = tempItemRepresentation.AddComponent<Image>();
            tempImage.sprite = _item.icon;
            tempImage.rectTransform.sizeDelta = new Vector2(tempImageSize.x, tempImageSize.y); // Le damos un tamaño a la imagen con el icono temporal
            tempImage.color = icon.color;
            tempItem = _item;
        }
    }

    public override void OnDrag ( PointerEventData eventData )
    {
        if (tempItemRepresentation != null)
        {
            tempItemRepresentation.transform.position = Input.mousePosition;
        }
    }
    public override void OnEndDrag ( PointerEventData eventData )
    {
        if (tempItemRepresentation != null)
        {
            Destroy(tempItemRepresentation);

            // Aquí necesitas determinar si el mouse está sobre otro contenedor al soltar.
            // Puedes usar eventData.pointerCurrentRaycast.gameObject para obtener el GameObject actual bajo el cursor.
            GameObject dropTarget = eventData.pointerCurrentRaycast.gameObject;
            _tempTargetContainer = dropTarget.GetComponentInParent<InventoryItem_UI>();

            if (dropTarget != null && dropTarget.GetComponentInParent<InventoryItem_UI>() != null)
            {
                InventoryItem_UI _targetContainer = dropTarget.GetComponentInParent<InventoryItem_UI>();
                Debug.Log("hola hola");
                if (dropTarget.GetComponentInParent<InventoryItem_UI>() as EquipedItem_UI_Layout != null)
                {
                    EquipedItem_UI_Layout targetContainer = _targetContainer as EquipedItem_UI_Layout;
                    if (targetContainer.empty || targetContainer.itemCategory == this.itemCategory)
                    {
                        // Aquí implementas la lógica para transferir el item al contenedor objetivo.
                        // Esto podría incluir llamar a `Unequip` en este contenedor y `Equip` en el contenedor destino.
                        targetContainer.Equip(tempItem);
                    }
                }
                else if (dropTarget.GetComponentInParent<InventoryItem_UI>() as InventoryItem_UI_Layout != null)
                {
                    InventoryItem_UI_Layout targetContainer = _targetContainer as InventoryItem_UI_Layout;

                    if (targetContainer.empty)
                    {
                        // Aquí implementas la lógica para transferir el item al contenedor objetivo.
                        // Esto podría incluir llamar a `Unequip` en este contenedor y `Equip` en el contenedor destino.
                        targetContainer.SetItem(item);
                    }
                    else
                    {
                        Item _targetItem = targetContainer.item;
                        targetContainer.SetItem(tempItem);
                        _item = _targetItem;
                    }
                }

            }
        }
        tempItem = null;
    }

    public override void OnPointerClick ( PointerEventData eventData )
    {
    }
    public override void OnPointerEnter ( PointerEventData eventData )
    {
        if (eventData.pointerEnter.gameObject != null)
        {
            GameObject dropTarget = eventData.pointerEnter.gameObject;
            _tempTargetContainer = dropTarget.GetComponentInParent<InventoryItem_UI>();
        }
    }
}
