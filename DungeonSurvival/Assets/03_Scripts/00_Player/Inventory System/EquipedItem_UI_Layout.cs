using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class EquipedItem_UI_Layout : InventoryItem_UI
{
    [SerializeField] Image icon;

    [SerializeField] Vector2 tempImageSize = new Vector2(100,100);

    public bool empty => item == null;

    private GameObject tempItemRepresentation;
    private InventoryItem_UI _tempTargetContainer;
    private Item tempItem;

    private void Start ( )
    {
        icon.sprite = itemCategory.icon;
        UpdateIconAlpha(0.5f);
        itemInformationWindow_UI = transform.parent.GetComponent<InventoryItem_UI>().itemInformationWindow_UI;
        equipmentItemInformationWindow_UI = transform.parent.GetComponent<InventoryItem_UI>().equipmentItemInformationWindow_UI;
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

        this.item = item == null ? null : item.item;
    }

    public void EquipUI ( Item item )
    {
        this.item = item;
        icon.sprite = this.item.icon;
        UpdateIconAlpha(1);
    }

    public void UnequipUI ( )
    {
        item = null;
        icon.sprite = itemCategory.icon;
        UpdateIconAlpha(0.5f);
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
        icon.gameObject.SetActive(false);
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
            bool equipable = item.equipable ? true : false;
            if (equipable)
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
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }
    public void SetItem ( Item _item )
    {
        item = _item;
    }
    public override void OnPointerClick ( PointerEventData eventData )
    {
        // Implementar la lógica para la informacion del item.
    }
    public override void OnPointerEnter ( PointerEventData eventData )
    {
    }
}
