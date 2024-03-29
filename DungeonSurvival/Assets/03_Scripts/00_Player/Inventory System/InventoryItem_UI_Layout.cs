using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.GraphView;

public class InventoryItem_UI_Layout : InventoryItem_UI
{
    [SerializeField] Toggle _toggle;
    public Toggle toggle => _toggle;

    [SerializeField] Image icon;
    [SerializeField] TMP_Text amountCounter;

    [SerializeField] KeyCode _key;

    [SerializeField] Vector2 tempImageSize = new Vector2(100, 100);

    Item _item;
    public Item item => _item;
    public void SetItem ( Item item ) { _item = item; }
    public bool empty => item == null;

    private GameObject tempItemRepresentation;
    private InventoryItem_UI _tempTargetContainer;
    private Item tempItem;

    private void Start()
    {
        icon.gameObject.SetActive(false);
        amountCounter.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            PlayerInventory_UI_Manager.current.SelectItem(transform.GetSiblingIndex());
        }
    }

    public void SetItemToInventory(InventoryItem item)
    {
        if (item == null)
        {
            icon.gameObject.SetActive(false);
            amountCounter.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
            icon.sprite = item.item.icon;

            if (item.stackable)
            {
                amountCounter.transform.parent.gameObject.SetActive(true);
                amountCounter.text = item.currentStack.ToString();
            }
        }

        _item = item == null ? null : item.item;
    }

    public void UpdateStack(InventoryItem item)
    {
        amountCounter.text = item.currentStack.ToString();
    }
    public override void OnBeginDrag ( PointerEventData eventData )
    {
        if (_item != null)
        {
            tempItemRepresentation = new GameObject("TempItem");
            tempItemRepresentation.transform.SetParent(transform.parent.parent.parent, false);
            tempItemRepresentation.transform.SetAsLastSibling(); // Para visualizarlo por encima de otros UIs
            Image tempImage = tempItemRepresentation.AddComponent<Image>();
            tempImage.raycastTarget = false;
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

            if (_tempTargetContainer != null)
            {
                if (_tempTargetContainer as EquipedItem_UI_Layout != null)
                {
                    Debug.Log("hi1");

                    EquipedItem_UI_Layout targetContainer = _tempTargetContainer as EquipedItem_UI_Layout;
                    if (targetContainer.empty || targetContainer.itemCategory.equipmentCategory == tempItem.equipmentDataSO.equipmentStats.equipmentCategory)
                    {
                        // Aquí implementas la lógica para transferir el item al contenedor objetivo.
                        // Esto podría incluir llamar a `Unequip` en este contenedor y `Equip` en el contenedor destino.
                        targetContainer.Equip(tempItem);
                    }
                }
                else if (_tempTargetContainer as InventoryItem_UI_Layout != null)
                {

                    InventoryItem_UI_Layout targetContainer = _tempTargetContainer as InventoryItem_UI_Layout;

                    if (targetContainer.empty)
                    {
                        // Aquí implementas la lógica para transferir el item al contenedor objetivo.
                        // Esto podría incluir llamar a `Unequip` en este contenedor y `Equip` en el contenedor destino.
                        targetContainer.SetItem(tempItem);
                        Debug.Log("hi3");
                    }
                    else
                    {
                        Item _targetItem = targetContainer.item;
                        targetContainer.SetItem(tempItem);
                        _item = _targetItem;
                        Debug.Log("hi4");

                    }
                }

            }
        }
        tempItem = null;
    }

public override void OnPointerClick ( PointerEventData eventData )
    {
        // Implementar la lógica para la informacion del item.
    }
    public override void OnPointerEnter ( PointerEventData eventData )
    {
    }
}
