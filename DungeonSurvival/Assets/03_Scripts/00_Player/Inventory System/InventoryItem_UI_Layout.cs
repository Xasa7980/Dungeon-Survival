using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;

public class InventoryItem_UI_Layout : InventoryItem_UI
{
    [SerializeField] Toggle _toggle;
    public Toggle toggle => _toggle;

    [SerializeField] Image icon;
    [SerializeField] TMP_Text amountCounter;

    [SerializeField] KeyCode _key;

    [SerializeField] Vector2 tempImageSize = new Vector2(100, 100);
    public bool empty => item == null;

    private GameObject tempItemRepresentation;
    private InventoryItem_UI _tempTargetContainer;
    private Item tempItem;
    public Item GetTempItem => tempItem;

    private void Start()
    {
        icon.gameObject.SetActive(false);
        amountCounter.transform.parent.gameObject.SetActive(false);
        inventoryType = transform.parent.GetComponent<InventoryItem_UI>().inventoryType;
        itemInformationWindow_UI = transform.parent.GetComponent<InventoryItem_UI>().itemInformationWindow_UI;
        equipmentItemInformationWindow_UI = transform.parent.GetComponent<InventoryItem_UI>().equipmentItemInformationWindow_UI;

        keyIcon.SetActive(hasKeyIcon);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            PlayerInventory_UI_Manager.current.SelectItem(transform.GetSiblingIndex());
        }
    }

    public void SetItemToInventory ( InventoryItem item )
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

        this.item = item == null ? null : item.item;
    }

    public void UpdateStack(InventoryItem item)
    {
        amountCounter.text = item.currentStack.ToString();
    }
    public void RemoveItem_UI(InventoryItem inventoryItem)
    {
        item = null; 
        inventoryItem = null;
        icon.gameObject.SetActive(false);
        amountCounter.transform.parent.gameObject.SetActive(false);
    }
    public override void OnBeginDrag ( PointerEventData eventData )
    {
        if ( item != null)
        {
            tempItemRepresentation = new GameObject("TempItem");
            tempItemRepresentation.transform.SetParent(GetComponentInParent<Canvas>().transform, false);
            tempItemRepresentation.transform.SetAsLastSibling(); // Para visualizarlo por encima de otros UIs
            Image tempImage = tempItemRepresentation.AddComponent<Image>();
            tempImage.raycastTarget = false;
            tempImage.sprite = item.icon;
            tempImage.rectTransform.sizeDelta = new Vector2(tempImageSize.x, tempImageSize.y); // Le damos un tamaño a la imagen con el icono temporal
            tempImage.color = icon.color;
            tempItem = item;
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
            AttemptItemDrop(eventData);
        }
        tempItem = null;
    }

    private void AttemptItemDrop ( PointerEventData eventData )
    {
        GameObject dropTarget = eventData.pointerEnter.gameObject;

        if (dropTarget == null) return;
        InventoryItem_UI inventoryItem_UI = dropTarget.GetComponentInParent<InventoryItem_UI>();
        _tempTargetContainer = inventoryItem_UI;

        if (_tempTargetContainer == null) return;

        if(inventoryItem_UI is InventoryItem_UI_Layout)
        {
            SwapOrTransferItem((InventoryItem_UI_Layout)_tempTargetContainer);
        }
        else if(inventoryItem_UI is EquipedItem_UI_Layout)
        {
            SwapOrTransferItem((EquipedItem_UI_Layout)_tempTargetContainer);
        }
    }
    private void SwapOrTransferItem ( InventoryItem_UI inventoryItem_UI )
    {
        bool sourceHasItem = this.item != null;
        bool targetHasItem = inventoryItem_UI.item != null;

        if (inventoryItem_UI is InventoryItem_UI_Layout)
        {
            InventoryItem_UI_Layout inventorySlot = inventoryItem_UI as InventoryItem_UI_Layout;

            if (targetHasItem)
            {
                inventorySlot.SetItem(this.item);
                this.RemoveItem_UI(null); // Asume que este método puede manejar la lógica para limpiar el slot actual
            }
            else
            {
                // Si ambos slots tienen ítems, intercámbialos
                Item tempItem = this.item;
                this.SetItem(inventoryItem_UI.item);
                inventorySlot.SetItem(tempItem);
            }

            this.UpdateUI();
            inventorySlot.UpdateUI();
        }
        else if (inventoryItem_UI is EquipedItem_UI_Layout)
        {
            EquipedItem_UI_Layout equipmentSlot = inventoryItem_UI as EquipedItem_UI_Layout;
            bool equipable = item.equipable? true : false;
            if(equipable)
            {
                if (equipmentSlot.empty)
                {
                    equipmentSlot.SetItem(this.item);
                    this.RemoveItem_UI(null);
                }
                else
                {
                    Item tempItem = this.item;
                    this.SetItem(equipmentSlot.item);
                    equipmentSlot.SetItem(tempItem);
                }
                if (equipmentSlot.item != null)
                {
                    bool canEquip = equipmentSlot.item.equipmentDataSO.equipmentStats.equipmentCategory == equipmentSlot.itemCategory.equipmentCategory;
                    if (canEquip)
                    {
                        equipmentSlot.item.Equip();
                        equipmentSlot.EquipUI(equipmentSlot.item);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                Debug.Log("Not EquipableItem");
                return;
            }
        }

    }
    // Implementación de UpdateUI para reflejar los cambios en el inventario
    public void UpdateUI ( )
    {
        if (this.item != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = this.item.icon;
            if (this.item.isStackable && this.item.currentStack > 1)
            {
                amountCounter.text = this.item.currentStack.ToString();
                amountCounter.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                amountCounter.transform.parent.gameObject.SetActive(false);
            }
        }
        else
        {
            icon.gameObject.SetActive(false);
            amountCounter.transform.parent.gameObject.SetActive(false);
        }
    }
    public void SetItem(Item _item )
    {
        item = _item;
    }
    public override void OnPointerClick ( PointerEventData eventData )
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            //Abrir ventana de usos para item
            if (item.equipable)
            {
                //Usos para Equipamiento (Equipar, desequipar, destruir, tirar al suelo)
            }
            else
            {
                //Usos para item (Uso,destruir,tirar al suelo)
            }
        }
        else if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(item.equipable)
            {
                equipmentItemInformationWindow_UI.gameObject.SetActive(true);
                equipmentItemInformationWindow_UI.SetItemInformation(item);
            }
            else
            {
                itemInformationWindow_UI.gameObject.SetActive(false);
                itemInformationWindow_UI.SetItemInformation(item);
            }
        }
    }
    public override void OnPointerEnter ( PointerEventData eventData )
    {
    }
    private void Show ( GameObject gO)
    {
        gO.SetActive(true);
    }
    private void Hide ( GameObject gO )
    {
        gO.SetActive(false);
    }
}
