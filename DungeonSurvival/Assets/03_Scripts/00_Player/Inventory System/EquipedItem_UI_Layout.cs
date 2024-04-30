using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventoryItem_UI_Layout;
using static UnityEditor.Progress;

public class EquipedItem_UI_Layout : InventoryItem_UI
{
    [SerializeField] Image icon;

    [SerializeField] Vector2 tempImageSize = new Vector2(100,100);

    public Transform targetHolster => _targetHolster;
    [SerializeField] private Transform _targetHolster;

    private GameObject tempItemRepresentation;
    private InventoryItem_UI _tempTargetContainer;
    private Item tempItem;

    private void Start ( )
    {
        if(itemCategory != null) icon.sprite = itemCategory.icon;
        inventoryType = transform.GetComponentInParent<InventoryItem_UI>().inventoryType;
    }
    public void SetItemToEquipmentWindow ( InventoryItem item )
    {
        if (item == null)
        {
            if(itemCategory != null) icon.sprite = itemCategory.icon;
            else icon.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
            icon.sprite = item.item.icon;
        }

        this.item = item == null ? null : (Item)item.item;
    }

    public void EquipUI ( Item _item )
    {
        this.item = _item;

        if (this.item.equipable && itemCategory != null && item.equipmentDataSO != null)
        {
            if (itemCategory.itemCategories != _item.equipmentDataSO.equipmentStats.equipmentCategory) return;
        }
        icon.sprite = this.item.icon;
        UpdateIconAlpha(1);
    }

    public void UnequipUI ( )
    {
        //item = null;
        RemoveItem_UI(null);
        if (itemCategory != null) icon.sprite = itemCategory.icon;
        else icon.gameObject.SetActive(false);
    }
    private void UpdateIconAlpha ( float alpha )
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }
    public void RemoveItem_UI ( InventoryItem inventoryItem )
    {
        item = null;
        inventoryItem = null;
        if (itemCategory != null) icon.sprite = itemCategory.icon;
        else icon.gameObject.SetActive(false);
    }
    public override void OnBeginDrag ( PointerEventData eventData )
    {
        if (item != null)
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
        if (eventData.pointerEnter.gameObject == null)
        {
            tempItem.InstantiateInWorld(eventData.position, tempItem);
            tempItem.UnequipStats();
        }

        if (dropTarget == null) return;
        InventoryItem_UI inventoryItem_UI = dropTarget.GetComponentInParent<InventoryItem_UI>();
        _tempTargetContainer = inventoryItem_UI;

        if (_tempTargetContainer == null) return;

        if (inventoryItem_UI is InventoryItem_UI_Layout)
        {
            SwapOrTransferItem((InventoryItem_UI_Layout)_tempTargetContainer);
        }
        else if (inventoryItem_UI is EquipedItem_UI_Layout)
        {
            SwapOrTransferItem((EquipedItem_UI_Layout)_tempTargetContainer);
        }
    }
    private void SwapOrTransferItem ( InventoryItem_UI inventoryItem_UI )
    {
        bool sourceHasItem = this.item != null;
        bool targetHasItem = inventoryItem_UI.item != null;

        if (inventoryItem_UI is InventoryItem_UI_Layout) //Si del equipo pasa a inventario
        {
            InventoryItem_UI_Layout inventorySlot = inventoryItem_UI as InventoryItem_UI_Layout;

            if (!targetHasItem) //Si ese inventario no tiene un item
            {
                inventorySlot.SetItem(this.item);
                PlayerInventory.current.UnequipItem(tempItem, this);
                this.RemoveItem_UI(null); // Asume que este método puede manejar la lógica para limpiar el slot actual)
            }
            else //Si tiene items
            {
                Item tempItem = this.item;
                if (inventorySlot.item.equipmentDataSO.equipmentStats.equipmentCategory == itemCategory.itemCategories)
                {
                    PlayerInventory.current.UnequipItem(item, this);
                    SetItem(inventorySlot.item);
                }
                else return;
                inventorySlot.SetItem(tempItem);
            }

            this.UpdateUI();
            inventorySlot.UpdateUI();
        }
        else if (inventoryItem_UI is EquipedItem_UI_Layout) //Si paso de este slot a otro slot de equipo
        {
            EquipedItem_UI_Layout equipmentSlot = inventoryItem_UI as EquipedItem_UI_Layout;
            bool equipable = item.equipable ? true : false;

            if (equipable)
            {
                if (equipmentSlot.empty) //Si esta vacio
                {
                    if (tempItem.equipmentDataSO != null)
                    {
                        ItemCategories tempItemCategory = tempItem.equipmentDataSO.equipmentStats.equipmentCategory;
                        if (tempItemCategory == equipmentSlot.itemCategory.itemCategories)
                        {
                            equipmentSlot.SetItem(tempItem);
                            PlayerInventory.current.EquipItem(equipmentSlot.item, equipmentSlot);
                            UnequipUI();
                        }
                        else
                        {
                            Debug.Log("Not matched for this slot");
                            return;
                        }
                    }
                    else
                    {
                        Debug.LogError("Not found equipmentDataSO");
                        return;
                    }
                }
                else
                {
                    bool matchedCategory = tempItem.equipmentDataSO.equipmentStats.equipmentCategory == equipmentSlot.itemCategory.itemCategories &&
                                           tempItem.equipmentDataSO.weaponHandlerType == equipmentSlot.item.equipmentDataSO.weaponHandlerType;

                    if (matchedCategory) //Si el equipo es un arma y es de la misma mano que la otra
                    {
                        Debug.Log("Matched2" + item.equipmentDataSO.equipmentStats.equipmentCategory.ToString());
                        Item tempItem = this.item;

                        this.SetItem(equipmentSlot.item);

                        PlayerInventory.current.EquipItem(equipmentSlot.item,this);
                        PlayerInventory.current.EquipItem(tempItem,equipmentSlot);
                        
                        equipmentSlot.SetItem(tempItem); //Añado el item al slot
                    }
                    else
                    {
                        Debug.Log("Not matched to equipment");
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
    public void TransferItemToAnotherSlot ( EquipedItem_UI_Layout destinationSlot )
    {
        if (item == null)
        {
            Debug.LogError("Transfer failed. Item is null.");
            return;
        }

        if (!destinationSlot.CanReceiveItem(item))
        {
            Debug.LogError("Transfer failed. Destination slot cannot receive the item.");
            return;
        }

        // Transferir el ítem al slot destino
        destinationSlot.SetItem(item);

        // Actualizar UI del slot destino
        destinationSlot.UpdateUI();

        // Limpiar el ítem actual del slot origen
        Debug.Log(item);
        UnequipUI();
        item = null;
        Debug.Log(item);
        // Actualizar UI del slot origen
        UpdateUI();
        Debug.Log("Item successfully transferred to another slot.");
    }

    public bool CanReceiveItem ( Item _item )
    {
        bool canReceive = item == null || item.equipmentDataSO.equipmentStats.equipmentCategory == _item.equipmentDataSO.equipmentStats.equipmentCategory;
        Debug.Log($"CanReceiveItem called: item is null: {item == null}, categories match: {item != null && item.equipmentDataSO.equipmentStats.equipmentCategory == _item.equipmentDataSO.equipmentStats.equipmentCategory}");
        return canReceive;
    }
    // Implementación de UpdateUI para reflejar los cambios en el inventario
    public void UpdateUI ( )
    {
        if (item != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = item.icon;
            if (item.isStackable && currentStack > 1)
            {
                item.currentAmount = currentStack;
            }
        }
        else
        {
            if (itemCategory != null) icon.sprite = itemCategory.icon;
            else icon.gameObject.SetActive(false);

        }
    }
    public void SetItem ( Item _item )
    {
        if(itemCategory.itemCategories == _item.equipmentDataSO.equipmentStats.equipmentCategory)
        {
            item = _item;
        }
        else
        {
            Debug.LogError("Este arma no puede ir aqui");
        }
    }
    public override void OnPointerClick ( PointerEventData eventData )
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("right clicked");
            //Abrir ventana de usos para item
            if (eventData.pointerClick.gameObject.TryGetComponent<InventoryItem_UI>(out InventoryItem_UI inventoryItem_UI))
            {
                if (inventoryItem_UI is InventoryItem_UI_Layout)
                {
                    InventoryItem_UI_Layout inventoryItem_UI_Layout = (InventoryItem_UI_Layout)inventoryItem_UI;
                    if (inventoryItem_UI_Layout.item != null)
                    {
                        UI_InventoryMenuManager.instance.HandleClickToItem(inventoryItem_UI_Layout.item);
                    }
                }
                else if (inventoryItem_UI is EquipedItem_UI_Layout)
                {
                    EquipedItem_UI_Layout equipedItem_UI_Layout = (EquipedItem_UI_Layout)inventoryItem_UI;
                    if (equipedItem_UI_Layout.item != null)
                    {
                        UI_InventoryMenuManager.instance.HandleClickToItem(equipedItem_UI_Layout.item);
                    }
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (eventData.pointerClick.gameObject.TryGetComponent<InventoryItem_UI>(out InventoryItem_UI inventoryItem_UI))
            {
                if (inventoryItem_UI is InventoryItem_UI_Layout)
                {
                    InventoryItem_UI_Layout inventoryItem_UI_Layout = (InventoryItem_UI_Layout)inventoryItem_UI;
                    if (inventoryItem_UI_Layout.item != null)
                    {
                        UI_InventoryMenuManager.instance.HandleClickToItem(inventoryItem_UI_Layout.item);
                    }
                }
                else if (inventoryItem_UI is EquipedItem_UI_Layout)
                {
                    EquipedItem_UI_Layout equipedItem_UI_Layout = (EquipedItem_UI_Layout)inventoryItem_UI;
                    if (equipedItem_UI_Layout.item != null)
                    {
                        UI_InventoryMenuManager.instance.HandleClickToItem(equipedItem_UI_Layout.item);
                    }
                }
            }
        }
    }
    public override void OnPointerEnter ( PointerEventData eventData )
    {
    }
}
