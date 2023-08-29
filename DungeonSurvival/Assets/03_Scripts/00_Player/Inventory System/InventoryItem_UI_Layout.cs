using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem_UI_Layout : MonoBehaviour
{
    [SerializeField] Toggle _toggle;
    public Toggle toggle => _toggle;

    [SerializeField] Image _icon;
    [SerializeField] TMP_Text amountCounter;

    [SerializeField] KeyCode _key;

    public Item item { get; private set; }
    public bool empty => item == null;

    private void Start()
    {
        _icon.gameObject.SetActive(false);
        amountCounter.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            PlayerInventory_UI_Manager.current.SelectItem(transform.GetSiblingIndex());
        }
    }

    public void SetItem(InventoryItem item)
    {
        if (item == null)
        {
            _icon.gameObject.SetActive(false);
            amountCounter.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _icon.gameObject.SetActive(true);
            _icon.sprite = item.item.icon;

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
}
