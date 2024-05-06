using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UI_InventoryMenuManager : MonoBehaviour
{
    public static UI_InventoryMenuManager instance;
    [Header ("Menu's")]
    [SerializeField] private CanvasGroup backpackInventory;
    [SerializeField] private CanvasGroup equipmentMenu;

    [SerializeField] private EquipmentItemInformationWindow_UI equipmentItemInformationWindow_UI;
    [SerializeField] private ItemInformationWindow_UI itemInformationWindow_UI;

    [Header ("UI Elements")]
    [SerializeField] private Button equipmentMenuSwitcherButton;

    [Header("EffectsProperties")]
    [SerializeField] private float speed = 10;

    private CanvasGroup equipmentItemInfoWindowCanvasGroup;
    private CanvasGroup itemInfoWindowCanvasGroup;
    private Animator equipmentMenuAnimator;
    private Animator backpackInventoryAnimator;
    private Animator informationEquipmentWindowAnimator;
    private Animator informationItemWindowAnimator;
    private bool equipmentMenuIsOn;
    private bool backpackInventoryIsOn;
    private bool equipmentInformationWindowIsOn;
    private bool itemInformationWindowIsOn;
    private void Awake ( )
    {
        instance = this;
    }

    private void Start ( )
    {
        equipmentMenuAnimator = equipmentMenu.GetComponent<Animator>();
        backpackInventoryAnimator = backpackInventory.GetComponent<Animator>();
        informationEquipmentWindowAnimator = equipmentItemInformationWindow_UI.GetComponent<Animator>();
        informationItemWindowAnimator = itemInformationWindow_UI.GetComponent<Animator>();
        equipmentItemInfoWindowCanvasGroup = equipmentItemInformationWindow_UI.GetComponent<CanvasGroup>();
        itemInfoWindowCanvasGroup = itemInformationWindow_UI.GetComponent<CanvasGroup>();

        backpackInventory.alpha = 0;
        equipmentMenu.alpha = 0;
        equipmentItemInfoWindowCanvasGroup.alpha = 0;
        itemInfoWindowCanvasGroup.alpha = 0;

        equipmentItemInfoWindowCanvasGroup.blocksRaycasts = false;
        itemInfoWindowCanvasGroup.blocksRaycasts = false;
        backpackInventory.blocksRaycasts = false;
        equipmentMenu.blocksRaycasts = false;

        equipmentMenuSwitcherButton.onClick.AddListener(EquipmentMenuSwitcher);
    }
    private void EquipmentMenuSwitcher ( )
    {
        equipmentMenuIsOn = !equipmentMenuIsOn;
        if(equipmentMenuIsOn )
        {
            equipmentMenuAnimator.SetTrigger("PopUp");
            StartCoroutine(FadeIn(equipmentMenu));
        }
        else
        {
            equipmentMenuAnimator.SetTrigger("PopOut");
            StartCoroutine(FadeOut(equipmentMenu));
        }
    }
    public void HandleClickToItem ( Item item )
    {
        if (item.equipable)
        {
            if (item.itemTag.GetCategoryTag == "Backpack")
            {
                SwitcherInventoryState();
            }
            else
            {
                SwitcherEquipmentItemInfoWindow(item);
            }
        }
        else
        {
            SwitcherItemInformationWindow(item);
        }
    }
    public void SwitcherInventoryState ( )
    {

        if( backpackInventory != null )
        {
            backpackInventoryIsOn = !backpackInventoryIsOn;
            if (backpackInventoryIsOn)
            {
                backpackInventoryAnimator.SetTrigger("PopUp");
                StartCoroutine(FadeIn(backpackInventory));
            }
            else
            {
                backpackInventoryAnimator.SetTrigger("PopOut");
                StartCoroutine(FadeOut(backpackInventory));
            }
        }
    }
    private void SwitcherEquipmentItemInfoWindow ( Item _item )
    {
        equipmentInformationWindowIsOn = !equipmentInformationWindowIsOn;

        if (equipmentInformationWindowIsOn)
        {
            informationEquipmentWindowAnimator.SetTrigger("PopUp");
            StartCoroutine(FadeIn(equipmentItemInfoWindowCanvasGroup));
            equipmentItemInformationWindow_UI.SetItemInformation(_item);
        }
        else
        {
            informationEquipmentWindowAnimator.SetTrigger("PopOut");
            StartCoroutine(FadeOut(equipmentItemInfoWindowCanvasGroup));
        }
    }
    private void SwitcherItemInformationWindow (Item _item )
    {
        itemInformationWindowIsOn = !itemInformationWindowIsOn;

        if (itemInformationWindowIsOn)
        {
            informationItemWindowAnimator.SetTrigger("PopUp");
            StartCoroutine(FadeIn(itemInfoWindowCanvasGroup));
            itemInformationWindow_UI.SetItemInformation(_item);
        }
        else
        {
            informationItemWindowAnimator.SetTrigger("PopOut");
            StartCoroutine(FadeOut(itemInfoWindowCanvasGroup));
        }
    }
    private IEnumerator FadeIn ( CanvasGroup _canvasGroup )
    {
        _canvasGroup.blocksRaycasts = true;
        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1f, Time.deltaTime * speed);
            yield return null;
        }
        _canvasGroup.alpha = 1f;  // Asegura que el alpha llegue a 1.
    }
    private IEnumerator FadeOut ( CanvasGroup _canvasGroup )
    {
        _canvasGroup.blocksRaycasts = false;
        while (_canvasGroup.alpha > 0f)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0f, Time.deltaTime * speed);
            yield return null;
        }
        _canvasGroup.alpha = 0f;  // Asegura que el alpha llegue a 0.
    }
}
