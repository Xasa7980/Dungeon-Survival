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
        backpackInventory.alpha = 0;
        equipmentMenu.alpha = 0;

        equipmentMenuAnimator = equipmentMenu.GetComponent<Animator>();
        backpackInventoryAnimator = backpackInventory.GetComponent<Animator>();
        informationEquipmentWindowAnimator = equipmentItemInformationWindow_UI.GetComponent<Animator>();
        informationItemWindowAnimator = itemInformationWindow_UI.GetComponent<Animator>();

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
            if (item.itemTag.GetTag == "Backpack")
            {
                Debug.Log("Found a backpack");
                SwitcherInventoryState();
            }
            else
            {
                SwitcherEquipmentItemInfoWindow(item);
            }
        }
        else
        {
            Debug.Log("no equipable");
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
        CanvasGroup canvasGroup = equipmentItemInformationWindow_UI.GetComponent<CanvasGroup>();
        equipmentInformationWindowIsOn = !equipmentInformationWindowIsOn;

        if (equipmentInformationWindowIsOn)
        {
            informationEquipmentWindowAnimator.SetTrigger("PopUp");
            StartCoroutine(FadeIn(canvasGroup));
            equipmentItemInformationWindow_UI.SetItemInformation(_item);
        }
        else
        {
            informationEquipmentWindowAnimator.SetTrigger("PopOut");
            StartCoroutine(FadeOut(canvasGroup));
        }
    }
    private void SwitcherItemInformationWindow (Item _item )
    {
        CanvasGroup canvasGroup = itemInformationWindow_UI.GetComponent<CanvasGroup>();
        itemInformationWindowIsOn = !itemInformationWindowIsOn;

        if (itemInformationWindowIsOn)
        {
            informationItemWindowAnimator.SetTrigger("PopUp");
            StartCoroutine(FadeIn(canvasGroup));
            itemInformationWindow_UI.SetItemInformation(_item);
        }
        else
        {
            informationItemWindowAnimator.SetTrigger("PopOut");
            StartCoroutine(FadeOut(canvasGroup));
        }
    }
    private IEnumerator FadeIn ( CanvasGroup _canvasGroup )
    {
        while (_canvasGroup.alpha < 1f)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 1f, Time.deltaTime * speed);
            yield return null;
        }
        _canvasGroup.alpha = 1f;  // Asegura que el alpha llegue a 1.
    }
    private IEnumerator FadeOut ( CanvasGroup _canvasGroup )
    {
        while (_canvasGroup.alpha > 0f)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, 0f, Time.deltaTime * speed);
            yield return null;
        }
        _canvasGroup.alpha = 0f;  // Asegura que el alpha llegue a 0.
    }
}
