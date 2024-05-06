using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UI_Inventory_Panel : UI_Panel
{
    public static UI_Inventory_Panel current { get; private set; }

    public static iItemData hoveredItem;

    [SerializeField] RectTransform itemActionsMenu;

    [SerializeField, DisableInPlayMode] ItemTag currentTag;

    [SerializeField] Text itemTagName;

    public UI_ItemInspector itemInspector;

    iItemData[] currentItems;

    [SerializeField, FoldoutGroup("Prefabs")] UI_InventoryItemList_Layout InventoryListItemPrefab;
    [SerializeField, FoldoutGroup("Prefabs")] UI_Stat_Layout statLayoutPrefab;

    [SerializeField, FoldoutGroup("Containers")] Transform itemsList;
    [SerializeField, FoldoutGroup("Containers")] Transform statsContainer;

    [Space(10)]

    [SerializeField, FoldoutGroup("Item Info")] Text itemName;
    [SerializeField, FoldoutGroup("Item Info")] Text itemDescription;
    [SerializeField, FoldoutGroup("Item Info")] Image itemImage;

    [Space(10)]

    [SerializeField, FoldoutGroup("Inventory Settings")] Transform inventoryHolder;
    [SerializeField, FoldoutGroup("Inventory Settings")] RuntimeAnimatorController controller;
    [SerializeField, FoldoutGroup("Inventory Settings")] AnimationClip defaultAnimation;
    GameObject characterMiniature;

    [Title("Item Slots")]
    [SerializeField, FoldoutGroup("Inventory Settings")] Image meleeWeaponSlot;
    [SerializeField, FoldoutGroup("Inventory Settings")] Image rangeWeaponSlot;
    [SerializeField, FoldoutGroup("Inventory Settings")] Image shieldSlot;
    [SerializeField, FoldoutGroup("Inventory Settings")] Image armorSlot;
    [SerializeField, FoldoutGroup("Inventory Settings")] Image[] itemSlots;

    [SerializeField, FoldoutGroup("Inventory Settings")] UI_CharacterInspector inspector;

    [Title("Empty Slot Sprites")]

    [SerializeField, FoldoutGroup("Inventory Settings")] Sprite emptyMeleeWeaponSprite;
    [SerializeField, FoldoutGroup("Inventory Settings")] Sprite emptyRangeWeaponSprite;
    [SerializeField, FoldoutGroup("Inventory Settings")] Sprite emptyShieldSprite;
    [SerializeField, FoldoutGroup("Inventory Settings")] Sprite emptyArmorSprite;
    [SerializeField, FoldoutGroup("Inventory Settings")] Sprite emptyItemSprite;

    [Title("Type References")]
    [SerializeField, FoldoutGroup("Inventory Settings")] ItemTag itemTagReference;

    void Awake()
    {
        current = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && itemActionsMenu.gameObject.activeSelf)
        {
            //PROBLEMON PROBLEMON
            //El mouse position y el size delta no coinciden
            Rect rect = new Rect(itemActionsMenu.transform.position.x, itemActionsMenu.transform.position.y, itemActionsMenu.sizeDelta.x * 0.75f, itemActionsMenu.sizeDelta.y * 0.75f);
            if (!SupportTools.RectContains(rect, Input.mousePosition, false, true))
            {
                itemActionsMenu.gameObject.SetActive(false);
            }
        }
    }

    void FilterType(ItemTag itemTag)
    {
        itemTagName.text = itemTag.GetCategoryTag;
        currentTag = itemTag;

        List<iItemData> items = new List<iItemData>();

        foreach (iItemData i in PlayerInventory.current.GetEquipedBackpack.allItems)
        {
            if (i == null) continue;

            if (i.itemTag == itemTag)
                items.Add(i);
        }

        foreach (iItemData i in PlayerInventory.current.allItems)
        {
            if (i == null) continue;

            if (i.itemTag == itemTag)
                items.Add(i);
        }

        currentItems = items.ToArray();

        UpdateList();
    }

    void UpdateList()
    {
        ClearItemsList();

        foreach (iItemData i in currentItems)
        {
            UI_InventoryItemList_Layout.CreateInstance(InventoryListItemPrefab, i, itemsList);
        }
    }

    void ClearItemsList()
    {
        UI_InventoryItemList_Layout[] items = itemsList.GetComponentsInChildren<UI_InventoryItemList_Layout>();
        foreach (UI_InventoryItemList_Layout layout in items)
        {
            Destroy(layout.gameObject);
        }
    }

    public void SelectItem( Item item )
    {
        itemName.text = item.displayName;
        itemDescription.text = item.description;
        itemImage.sprite = item.icon;

        itemInspector.Configure(item);

        ClearStats_UI();

        //foreach (Stat s in item.stats)
        //{
        //    UI_Stat_Layout.CreateInstance(statLayoutPrefab, s, statsContainer);
        //}
    }

    void ClearStats_UI()
    {
        UI_Stat_Layout[] layouts = statsContainer.GetComponentsInChildren<UI_Stat_Layout>();

        foreach (UI_Stat_Layout layout in layouts)
        {
            Destroy(layout.gameObject);
        }
    }

    public override void Open()
    {
        base.Open();

        FilterType(currentTag);

        CreateInventoryMiniature();
    }

    public override void Close()
    {
        base.Close();

        ClearMiniature();
    }

    public void CloseItemActionMenu()
    {
        itemActionsMenu.gameObject.SetActive(false);
    }

    public void OpenItemActionMenu(Vector3 position)
    {
        itemActionsMenu.transform.position = position;
        itemActionsMenu.gameObject.SetActive(true);
    }

    void CreateInventoryMiniature()
    {
        GameObject character = Instantiate(PlayerLocomotion.current.gameObject, Vector3.one * 10000, Quaternion.identity);

        Destroy(character.GetComponentInChildren<AudioListener>());

        PlayerComponents[] behaviours = character.GetComponentsInChildren<PlayerComponents>();
        foreach (PlayerComponents behaviour in behaviours)
        {
            Destroy(behaviour);
        }

        UI_CharacterInventoryMiniature miniature = character.AddComponent<UI_CharacterInventoryMiniature>();

        miniature.Init(controller);

        miniature.transform.position = inventoryHolder.position;
        miniature.transform.rotation = inventoryHolder.rotation;

        ((AnimatorOverrideController)miniature.anim.runtimeAnimatorController)["_CharacterInspector_State"] = defaultAnimation;

        characterMiniature = character;
        inspector.Init(character.transform);

        UpdateCharacterPlot();
    }

    void UpdateCharacterPlot ( ) { }
    //{
    //    if (CharacterCombat.current.meleeWeapon != null)
    //    {
    //        meleeWeaponSlot.sprite = CharacterCombat.current.meleeWeapon.icon;
    //        meleeWeaponSlot.color = new Color(meleeWeaponSlot.color.r, meleeWeaponSlot.color.g, meleeWeaponSlot.color.b, 1);
    //    }
    //    else
    //    {
    //        meleeWeaponSlot.sprite = emptyMeleeWeaponSprite;
    //        meleeWeaponSlot.color = new Color(meleeWeaponSlot.color.r, meleeWeaponSlot.color.g, meleeWeaponSlot.color.b, 40f / 255f);
    //    }

    //    if (CharacterCombat.current.rangeWeapon != null)
    //    {
    //        rangeWeaponSlot.sprite = CharacterCombat.current.rangeWeapon.icon;
    //        rangeWeaponSlot.color = new Color(rangeWeaponSlot.color.r, rangeWeaponSlot.color.g, rangeWeaponSlot.color.b, 1);
    //    }
    //    else
    //    {
    //        rangeWeaponSlot.sprite = emptyRangeWeaponSprite;
    //        rangeWeaponSlot.color = new Color(rangeWeaponSlot.color.r, rangeWeaponSlot.color.g, rangeWeaponSlot.color.b, 40f / 255f);
    //    }

    //    if (CharacterCombat.current.shield != null)
    //    {
    //        shieldSlot.sprite = CharacterCombat.current.shield.icon;
    //        shieldSlot.color = new Color(shieldSlot.color.r, shieldSlot.color.g, shieldSlot.color.b, 1);
    //    }
    //    else
    //    {
    //        shieldSlot.sprite = emptyShieldSprite;
    //        shieldSlot.color = new Color(shieldSlot.color.r, shieldSlot.color.g, shieldSlot.color.b, 40f / 255f);
    //    }

    //    if (CharacterCombat.current.armor != null)
    //    {
    //        armorSlot.sprite = CharacterCombat.current.armor.icon;
    //        armorSlot.color = new Color(armorSlot.color.r, armorSlot.color.g, armorSlot.color.b, 1);
    //    }
    //    else
    //    {
    //        armorSlot.sprite = emptyArmorSprite;
    //        armorSlot.color = new Color(armorSlot.color.r, armorSlot.color.g, armorSlot.color.b, 40f / 255f);
    //    }

    //    for (int i = 0; i < itemSlots.Length; i++)
    //    {
    //        if (CharacterCombat.current.equipedItems[i] != null)
    //        {
    //            itemSlots[i].sprite = CharacterCombat.current.equipedItems[i].icon;
    //            itemSlots[i].color = new Color(itemSlots[i].color.r, itemSlots[i].color.g, itemSlots[i].color.b, 1);
    //        }
    //        else
    //        {
    //            itemSlots[i].sprite = emptyItemSprite;
    //            itemSlots[i].color = new Color(itemSlots[i].color.r, itemSlots[i].color.g, itemSlots[i].color.b, 40f / 255f);
    //        }
    //    }
    //}

    public void ClearMiniature()
    {
        if(characterMiniature)
        Destroy(characterMiniature);
    }

    public void EquipItem()
    {
        //bool equiped = false;

        //if (hoveredItem.weaponType == WeaponType.Melee)
        //{
        //    InventoryItem currentItem = CharacterCombat.current.meleeWeapon;

        //    CharacterCombat.current.meleeWeapon = (InventoryItem)hoveredItem;

        //    if (currentItem != null)
        //        currentItem.Unequip();

        //    equiped = true;
        //}

        //if (hoveredItem.weaponType == WeaponType.Range)
        //{
        //    InventoryItem currentItem = CharacterCombat.current.rangeWeapon;
            
        //    CharacterCombat.current.rangeWeapon = (InventoryItem)hoveredItem;

        //    if (currentItem != null)
        //        currentItem.Unequip();

        //    equiped = true;
        //}

        //if (hoveredItem.weaponType == WeaponType.Shield)
        //{
        //    InventoryItem currentItem = CharacterCombat.current.shield;
            
        //    CharacterCombat.current.shield = (InventoryItem)hoveredItem;

        //    if (currentItem != null)
        //        currentItem.Unequip();

        //    equiped = true;
        //}

        //if (hoveredItem.weaponType == WeaponType.Armor)
        //{
        //    InventoryItem currentItem = CharacterCombat.current.armor;

        //    CharacterCombat.current.armor = (InventoryItem)hoveredItem;

        //    if (currentItem != null)
        //        currentItem.Unequip();

        //    equiped = true;
        //}

        //if (hoveredItem.IsType(itemTypeReference))
        //{
        //    for (int i = 0; i < CharacterCombat.current.equipedItems.Length; i++)
        //    {
        //        if (CharacterCombat.current.equipedItems[i] == null)
        //        {
        //            CharacterCombat.current.equipedItems[i] = (InventoryItem)hoveredItem;
        //            equiped = true;
        //            break;
        //        }
        //    }
        //}

        //if (equiped)
        //    hoveredItem.Equip();

        //FilterType(hoveredItem.itemType);
        //UpdateList();
        //UpdateCharacterPlot();

        //itemActionsMenu.gameObject.SetActive(false);
    }
}
