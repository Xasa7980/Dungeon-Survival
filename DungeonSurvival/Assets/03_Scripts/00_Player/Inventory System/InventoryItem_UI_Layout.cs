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

    private GameObject tempItemRepresentation;
    private InventoryItem_UI _tempTargetContainer;
    public Item GetTempItem => tempItem;
    private Item tempItem;
    private void Start()
    {
        icon.gameObject.SetActive(false);
        amountCounter.transform.parent.gameObject.SetActive(false);
        inventoryType = transform.parent.GetComponent<InventoryItem_UI>().inventoryType;
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

        this.item = item == null ? null : (Item)item.item;
    }

    public void UpdateStack(InventoryItem inventoryItem)
    {
        amountCounter.text = inventoryItem.currentStack.ToString();
        currentStack = inventoryItem.currentStack;
    }
    public void RemoveItem_UI()
    {
        item = null; 
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
        if(eventData.pointerEnter.gameObject == null)
        {
            tempItem.InstantiateInWorld(eventData.position, tempItem);
            tempItem.UnequipStats();
        }

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
        bool sourceHasItem = item != null;
        bool targetHasItem = inventoryItem_UI.item != null;

        if (inventoryItem_UI is InventoryItem_UI_Layout)
        {
            InventoryItem_UI_Layout inventorySlot = inventoryItem_UI as InventoryItem_UI_Layout;
            int targetCurrentStack = inventorySlot.currentStack;
            inventorySlot.currentStack = currentStack;
            currentStack = targetCurrentStack;

            if (!targetHasItem)
            {
                inventorySlot.SetItem(tempItem);
                this.RemoveItem_UI(); // Asume que este método puede manejar la lógica para limpiar el slot actual
            }
            else
            {
                // Si ambos slots tienen ítems, intercámbialos
                Item tempItem = item;
                this.SetItem(inventoryItem_UI.item);
                inventorySlot.SetItem(tempItem);
            }

            this.UpdateUI();
            inventorySlot.UpdateUI();
        }
        else if (inventoryItem_UI is EquipedItem_UI_Layout)
        {
            EquipedItem_UI_Layout equipmentSlot = inventoryItem_UI as EquipedItem_UI_Layout;
            bool equipable = tempItem.equipable ? true : false;
            int targetCurrentStack = equipmentSlot.currentStack;
            equipmentSlot.currentStack = currentStack;
            currentStack = targetCurrentStack;

            if (equipable)
            {
                if (equipmentSlot.empty) //Si esta vacio
                {
                    if(tempItem.equipmentDataSO != null)
                    {
                        ItemCategories tempItemCategory = tempItem.equipmentDataSO.equipmentStats.equipmentCategory; 
                        if(tempItemCategory == equipmentSlot.itemCategory.itemCategories)
                        {
                            equipmentSlot.SetItem(tempItem);
                            PlayerInventory.current.EquipItem(equipmentSlot.item, equipmentSlot);
                            RemoveItem_UI();
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
                    if (tempItem.equipmentDataSO == null) return;
                    bool matchedCategory = tempItem.equipmentDataSO.equipmentStats.equipmentCategory == equipmentSlot.itemCategory.itemCategories;

                    if (matchedCategory) //Si el equipo es un arma y es de la misma mano que la otra
                    {
                        Debug.Log("Matched2" + item.equipmentDataSO.equipmentStats.equipmentCategory.ToString());
                        Item tempItem = this.item;

                        this.SetItem(equipmentSlot.item);


                        equipmentSlot.SetItem(tempItem); //Añado el item al slot
                        PlayerInventory.current.EquipItem(equipmentSlot.item, equipmentSlot);
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
            UpdateUI();
            equipmentSlot.UpdateUI();
        }
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
                amountCounter.text = currentStack.ToString();
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
        RemoveItem_UI();

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
    private void Show ( GameObject gO)
    {
        gO.SetActive(true);
    }
    private void Hide ( GameObject gO )
    {
        gO.SetActive(false);
    }
}
