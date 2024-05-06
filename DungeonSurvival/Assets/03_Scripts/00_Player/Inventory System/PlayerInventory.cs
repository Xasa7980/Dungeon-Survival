using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour, iInventory
{
    public static PlayerInventory current { get; private set; }
    public event EventHandler OnEquipBackpack;
    public event EventHandler OnUnequipBackpack;

    [SerializeField] int maxItems = 9;
    [SerializeField] int maxEquipableItems = 9;
    public InventoryItem[] allItems { get; private set; }
    public InventoryItem[] allEquipableItems { get; private set; }

    public Item_Backpack GetEquipedBackpack => backpack;
    Item_Backpack backpack;

    public int keys { get; private set; }

    [SerializeField] private PlayerInteraction playerInteraction;

    public PlayerStats playerStats => playerStats;
    private PlayerStats _playerStats;
    public PlayerLocomotion playerLocomotion => playerLocomotion;
    private PlayerLocomotion _playerLocomotion;

    [SerializeField] private EquipedItem_UI_Layout mainWeaponSlot;
    [SerializeField] private EquipedItem_UI_Layout secondaryWeaponSlot;
    [SerializeField] private EquipedItem_UI_Layout helmetSlot;
    [SerializeField] private EquipedItem_UI_Layout chestSlot;
    [SerializeField] private EquipedItem_UI_Layout glovesSlot;
    [SerializeField] private EquipedItem_UI_Layout pantsSlot;
    [SerializeField] private EquipedItem_UI_Layout bootsSlot;
    [SerializeField] private EquipedItem_UI_Layout necklaceSlot;
    [SerializeField] private EquipedItem_UI_Layout ringSlot;

    private Dictionary<EquipedItem_UI_Layout, Transform> equipSlots;

    private void Awake()
    {
        current = this;
        allItems = new InventoryItem[maxItems];
        allEquipableItems = new InventoryItem[maxEquipableItems];
        InitializeEquipSlots();
    }

    private void InitializeEquipSlots ( )
    {
        equipSlots = new Dictionary<EquipedItem_UI_Layout, Transform>
        {
            {mainWeaponSlot, mainWeaponSlot.targetHolster},
            {secondaryWeaponSlot, secondaryWeaponSlot.targetHolster},
            {helmetSlot, helmetSlot.targetHolster},
            {chestSlot, chestSlot.targetHolster},
            {glovesSlot, glovesSlot.targetHolster},
            {pantsSlot, pantsSlot.targetHolster},
            {bootsSlot, bootsSlot.targetHolster},
            {necklaceSlot, necklaceSlot.targetHolster},
            {ringSlot, ringSlot.targetHolster}
        };
    }
    private void Start()
    {
        PlayerInteraction.current.OnInteractAnyObject += PlayerInteraction_OnInteractAnyObject;
        _playerStats = PlayerInteraction.current.gameObject.GetComponent<PlayerStats>();
        _playerLocomotion = PlayerInteraction.current.gameObject.GetComponent<PlayerLocomotion>();
    }
    public ActivableAltar activableAltar;
    private void PlayerInteraction_OnInteractAnyObject ( object sender, System.EventArgs e )
    {
        //if(e.objectInteracted.TryGetComponent<ActivableAltar>(out ActivableAltar activableAltar))
        PlayerInteraction playerInteraction = sender as PlayerInteraction;
        if(playerInteraction.possibleInteraction.TryGetComponent<ActivableAltar>(out activableAltar))
        {
            Debug.Log("Found an activable altar");
        }
    }
    public void AddKey()
    {
        keys++;
    }

    public bool TryUseKey()
    {
        if (keys > 0)
        {
            keys--;
            return true;
        }

        return false;
    }

    public void EquipBackpack(Item_Backpack backpack)
    {
        if (this.backpack != null)
            UnequipBackpack();

        //this.TryRemoveItem(backpack);
        Item_Backpack backpackInstance = backpack.CreateInstance() as Item_Backpack;
        backpackInstance.Init();
        this.backpack = backpackInstance;
        OnEquipBackpack?.Invoke(this, EventArgs.Empty);
        PlayerInventory_UI_Manager.current.EquipBackpack(backpackInstance);
        backpackInstance.EquipVisuals(PlayerHolsterHandler.current.backpack);
    }
    public void UnequipBackpack()
    {
        //if (!this.TryAddItem(backpack))
        //{
        backpack.InstantiateInWorld(PlayerLocomotion.current.transform.position, backpack);
        //}
        //else
        //{
        //    foreach (InventoryItem i in backpack.allItems)
        //    {
        //        if (i != null)
        //        {
        //            DropItem(i.item);
        //        }
        //    }
        //}

        Destroy(PlayerHolsterHandler.current.backpack.GetChild(0).gameObject);

        backpack = null;
        OnUnequipBackpack?.Invoke(this, EventArgs.Empty);
    }
    public void EquipItem ( Item item, EquipedItem_UI_Layout equipedItem_UI_Layout )
    {
        if (item != null && item.equipable && item.equipmentDataSO != null)
        {
            Transform holsterToUse;
            if (item.equipmentDataSO.equipmentCategory == EquipmentCategory.Armor && item.equipmentDataSO.armorIndex >= 0)
            {
                holsterToUse = equipedItem_UI_Layout.targetHolster;
                ActivateArmor(holsterToUse, item.equipmentDataSO.armorIndex);
            }
            else if (item.equipmentDataSO.weaponHandlerType == WeaponHandler.Hand_2)
            {
                // Desequipar ambas manos antes de equipar un arma de dos manos
                UnequipWeaponsIfNeeded(equipedItem_UI_Layout);
                holsterToUse = mainWeaponSlot.targetHolster;
                EquipWeapon(item, mainWeaponSlot, holsterToUse);
                if(equipedItem_UI_Layout == secondaryWeaponSlot)secondaryWeaponSlot.TransferItemToAnotherSlot(mainWeaponSlot);
            }
            else if (item.equipmentDataSO.weaponHandlerType == WeaponHandler.Hand_1)
            {
                if (equipedItem_UI_Layout == secondaryWeaponSlot && mainWeaponSlot.targetHolster.childCount == 0)
                {
                    // Si el slot principal está vacío y se intenta equipar en el secundario, equipar en el principal
                    holsterToUse = mainWeaponSlot.targetHolster;
                    EquipWeapon(item, mainWeaponSlot, holsterToUse);
                    secondaryWeaponSlot.TransferItemToAnotherSlot(mainWeaponSlot);
                }
                else if (equipedItem_UI_Layout == secondaryWeaponSlot && mainWeaponSlot.targetHolster.childCount > 0 && mainWeaponSlot.item.equipmentDataSO.weaponHandlerType == WeaponHandler.Hand_2)
                {
                    TryAddItem(mainWeaponSlot.item);
                    UnequipItem(mainWeaponSlot.item, mainWeaponSlot); // Asegúrate de desequipar el ítem antes de transferir otro ítem al slot.
                    holsterToUse = mainWeaponSlot.targetHolster;
                    EquipWeapon(item, mainWeaponSlot, holsterToUse);
                    secondaryWeaponSlot.TransferItemToAnotherSlot(mainWeaponSlot);
                }
                else
                {
                    // Equipar en el holster designado
                    if(equipedItem_UI_Layout.item != null)
                    {
                        UnequipItem(equipedItem_UI_Layout.item, equipedItem_UI_Layout); // Asegúrate de desequipar el ítem antes de transferir otro ítem al slot.
                    }
                    holsterToUse = equipedItem_UI_Layout.targetHolster;
                    EquipWeapon(item, equipedItem_UI_Layout, holsterToUse);
                }
            }
            else
            {
                equipSlots.TryGetValue(equipedItem_UI_Layout, out holsterToUse);
                EquipWeapon(item, equipedItem_UI_Layout, holsterToUse);
            }
        }
        else
        {
            Debug.LogError("Item is null, not equipable, or EquipmentDataSO is missing.");
            return;
        }
    }
    private void EquipWeapon ( Item item, EquipedItem_UI_Layout layout, Transform holster )
    {
        Item equipItemInstance = item.CreateInstance() as Item;

        layout.EquipUI(equipItemInstance);
        equipItemInstance.EquipVisuals(holster);
    }

    private void ActivateArmor ( Transform armorParent, int index )
    {
        for (int i = 0; i < armorParent.childCount; i++)
        {
            armorParent.GetChild(i).gameObject.SetActive(i == index);
        }
    }

    public void UnequipItem ( Item item, EquipedItem_UI_Layout equipedItem_UI_Layout )
    {
        if (equipSlots.TryGetValue(equipedItem_UI_Layout, out Transform holster) && holster.childCount > 0)
        {
            if (item.equipmentDataSO.equipmentCategory == EquipmentCategory.Armor)
            {
                for (int i = 0; i < holster.childCount; i++)
                {
                    holster.GetChild(i).gameObject.SetActive(i == 0);
                }
                Debug.Log("Deactivated armor and activated default armor at position 0.");
            }
            else
            {
                Transform childTransform = holster.GetChild(0);
                if (childTransform != null && childTransform.gameObject != null)
                {
                    equipedItem_UI_Layout.UnequipUI();
                    Destroy(childTransform.gameObject);
                }
                else
                {
                    Debug.LogError("Attempted to destroy a non-existent gameObject");
                }
            }
        }
    }

    private void UnequipWeaponsIfNeeded ( EquipedItem_UI_Layout layout )
    {
        if(layout == secondaryWeaponSlot)
        {
            if(mainWeaponSlot.item != null)
            {
                TryAddItem(mainWeaponSlot.item);
            }
        }
        else if(layout == mainWeaponSlot)
        {
            if (secondaryWeaponSlot.item != null)
            {
                TryAddItem(secondaryWeaponSlot.item);
            }
        }
        if (mainWeaponSlot.targetHolster.childCount > 0)
        {
            UnequipItem(mainWeaponSlot.item, mainWeaponSlot);
        }
        if (secondaryWeaponSlot.targetHolster.childCount > 0)
        {
            UnequipItem(secondaryWeaponSlot.item, secondaryWeaponSlot);
        }
    }
    void DropItem(Item item)
    {
        Vector3 position = UnityEngine.Random.insideUnitSphere * 2;
        position.y = 0;

        item.InstantiateInWorld(PlayerLocomotion.current.transform.position + position,this);
    }
    public void UseItem ( Item _item )
    {
        if (_item == null || string.IsNullOrWhiteSpace(_item.itemTag.GetItemTag))
        {
            Debug.LogError("Invalid item or item tag.");
            return;
        }

        iItemPerformingAction action = ItemActionFactory.Instance.GetActionByItemTag(_item.itemTag.GetItemTag, this, _item, _item.consumable ? _item.effectAmount : 10, _item.hasDuration ? _item.effectDuration : 0);
        if (action != null)
        {
            action.PerformSpecialAction(this, _item);
        }

    }
    public bool TryAddItem(Item item) //Por usar
    {
        if (!((iInventory)this).TryAddItem(item, out int index))
        {
            if (backpack != null)
            {
                return TryAddItemToBackPack(item);
            }

            return false;
        }

        if (index >= 0)
        {
            PlayerInventory_UI_Manager.current.AddItemToQuickAccess(index, allItems[index]);
        }

        return true;
    }
    public bool TryAddItemTo(InventoryItem_UI inputSlot, InventoryItem_UI targetSlot,Item item) //Por usar
    {
        if (inputSlot == targetSlot) return false;


        if (!((iInventory)this).TryAddItem(item, out int index))
        {
            if (backpack != null)
            {
                return TryAddItemToBackPack(item);
            }
            return false;
        }

        if (index >= 0)
        {
            if(targetSlot.inventoryType != InventoryType.BackpackInventory)
            {
                PlayerInventory_UI_Manager.current.AddItemToQuickAccess(index, allItems[index]);
            }
            PlayerInventory_UI_Manager.current.AddItemToBackpack(index, allItems[index]);
        }

        return true;
    }
    public bool TryRemoveItem(Item item, int quatityItemsToBeRemoved ) // Por usar
    {
        if (!((iInventory)this).TryRemoveItem(item, out int index, quatityItemsToBeRemoved))
        {
            if (backpack != null)
            {
                return TryRemoveItemFromBackpack(item, quatityItemsToBeRemoved);
            }
            else
                return false;
        }

        if (index >= 0)
            PlayerInventory_UI_Manager.current.AddItemToQuickAccess(index, null);

        return true;
    }

    bool TryAddItemToBackPack(Item item)
    {
        if (((iInventory)backpack).TryAddItem(item, out int index, true))
        {
            //Update backpack UI here
            if(index >= 0)
            {
                PlayerInventory_UI_Manager.current.AddItemToBackpack(index, backpack.allItems[index]);
            }

            return true;
        }
        else
            return false;
    }

    bool TryRemoveItemFromBackpack(Item item, int quatityItemsToBeRemoved )
    {
        if (((iInventory)backpack).TryRemoveItem(item, out int index, quatityItemsToBeRemoved))
        {
            //Update backpack UI here
            if (index >= 0)
            {
                PlayerInventory_UI_Manager.current.AddItemToBackpack(index, null);
            }

            return true;
        }
        else
            return false;
    }
}
