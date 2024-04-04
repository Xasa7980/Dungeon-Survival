using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;
public enum InventoryType
{
    AllInventory,
    B,
    EquipmentInventory
}
public class InventoryItem_UI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler
{
    public InventoryType inventoryType { get { return _inventoryType; } set { _inventoryType = value; } }
    [SerializeField] private InventoryType _inventoryType;

    public ItemInformationWindow_UI itemInformationWindow_UI;
    public Item item { get { return _item; } set { _item = value; } }
    private Item _item;
    public ItemCategory itemCategory => _itemCategory;
    [SerializeField] ItemCategory _itemCategory;

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