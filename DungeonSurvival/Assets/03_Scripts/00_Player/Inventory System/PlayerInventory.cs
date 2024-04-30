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
    [SerializeField] private EquipedItem_UI_Layout mainWeaponSlot;
    [SerializeField] private EquipedItem_UI_Layout secondaryWeaponSlot;

    Item equippedMainWeapon;
    Item equippedSecondaryWeapon;
    Item equippedHelmet;
    Item equippedChest;
    Item equippedGloves;
    Item equippedPants;
    Item equippedBoots;
    Item equippedNecklace;
    Item equippedRing;
    public Item[] getEquippedItems;

    public int keys { get; private set; }

    [SerializeField] private PlayerInteraction playerInteraction;
    public event EventHandler OnPlaceKey;

    public PlayerStats playerStats => playerStats;
    private PlayerStats _playerStats;
    public PlayerLocomotion playerLocomotion => playerLocomotion;
    private PlayerLocomotion _playerLocomotion;

    private void Awake()
    {
        current = this;
        allItems = new InventoryItem[maxItems];
        allEquipableItems = new InventoryItem[maxEquipableItems];
        getEquippedItems = new Item[9]
        {
            equippedMainWeapon,
            equippedSecondaryWeapon,
            equippedHelmet,
            equippedChest,
            equippedGloves,
            equippedPants,
            equippedBoots,
            equippedNecklace,
            equippedRing
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
    public void EquipItem(Item item, EquipedItem_UI_Layout equipedItem_UI_Layout )
    {
        Transform selectedHolster = SelectHolster(item, equipedItem_UI_Layout);


        if (selectedHolster != null && item != null && item.equipable)
        {
            Item equipItemInstance = item.CreateInstance() as Item;
            Debug.Log("Item equipped in " + selectedHolster.name);
            DetermineWhichEquipmentToUnequip(equipItemInstance, equipedItem_UI_Layout);
            equipedItem_UI_Layout.EquipUI(equipItemInstance);
            equipItemInstance.EquipVisuals(selectedHolster);
        }
    }
    private void DetermineWhichEquipmentToUnequip ( Item item, EquipedItem_UI_Layout equipedItem_UI_Layout )
    {
        bool mainWeaponSlotHasWeapon = mainWeaponSlot.targetHolster.childCount > 0 && mainWeaponSlot.item != null;
        bool secondaryWeaponSlotHasWeapon = secondaryWeaponSlot.targetHolster.childCount > 0 && secondaryWeaponSlot.item != null;
        // Simplifica la lógica comprobando primero el tipo de arma
        if (item.equipmentDataSO.weaponHandlerType == WeaponHandler.Hand_2)
        {
            // Si se equipa un arma de dos manos, desequipa todo
            if (mainWeaponSlotHasWeapon)
            {
                TryAddItem(mainWeaponSlot.item);
                UnequipItem(mainWeaponSlot.item, mainWeaponSlot);
            }
            if (secondaryWeaponSlotHasWeapon)
            {
                TryAddItem(secondaryWeaponSlot.item);
                UnequipItem(secondaryWeaponSlot.item, secondaryWeaponSlot);
            }

            if(equipedItem_UI_Layout == secondaryWeaponSlot && !secondaryWeaponSlot.empty) equipedItem_UI_Layout.TransferItemToAnotherSlot(mainWeaponSlot);
        }
        else if (item.equipmentDataSO.weaponHandlerType == WeaponHandler.Hand_1)
        {
            if (equipedItem_UI_Layout == secondaryWeaponSlot && !mainWeaponSlotHasWeapon && mainWeaponSlot.empty)
            {
                secondaryWeaponSlot.TransferItemToAnotherSlot(mainWeaponSlot);
                mainWeaponSlot.EquipUI(item);
                UnequipItem(secondaryWeaponSlot.item, secondaryWeaponSlot);
                
            }
        }
    }
    private Transform SelectHolster ( Item item, EquipedItem_UI_Layout layout )
    {
        bool mainWeaponSlotHasWeapon = mainWeaponSlot.targetHolster.childCount > 0;
        bool secondaryWeaponSlotHasWeapon = secondaryWeaponSlot.targetHolster.childCount > 0;

        if (item.equipmentDataSO.weaponHandlerType == WeaponHandler.Hand_1)
        {
            if (layout != mainWeaponSlot && !mainWeaponSlotHasWeapon)
            {
                Debug.Log("Item equipped in " + mainWeaponSlot.targetHolster);
                mainWeaponSlot.EquipUI(item);
                if (secondaryWeaponSlotHasWeapon)
                {
                    secondaryWeaponSlot.UnequipUI();
                }
                return mainWeaponSlot.targetHolster;
            }
            else
            {
                if (layout == secondaryWeaponSlot
                    )
                {
                    Debug.Log("Item equipped in 23333 " + secondaryWeaponSlot.targetHolster);
                    secondaryWeaponSlot.TransferItemToAnotherSlot(mainWeaponSlot);
                }
                layout.EquipUI(item);
                return layout.targetHolster;
            }
        }
        else if (item.equipmentDataSO.weaponHandlerType == WeaponHandler.Hand_2)
        {
            if (layout != mainWeaponSlot)
            {
                Debug.Log("Item equipped in " + mainWeaponSlot.targetHolster);
                secondaryWeaponSlot.TransferItemToAnotherSlot(mainWeaponSlot);
                mainWeaponSlot.EquipUI(item);
                return mainWeaponSlot.targetHolster;
            }
        }
        Debug.Log("by default in 4455: " + mainWeaponSlot.targetHolster);
        layout.EquipUI(item);
        return layout.targetHolster;
    }

    public void UnequipItem ( Item item, EquipedItem_UI_Layout equipedItem_UI_Layout )
    {
        if (equipedItem_UI_Layout.targetHolster.childCount > 0)
        {
            Transform childTransform = equipedItem_UI_Layout.targetHolster.GetChild(0);
            if (childTransform != null && childTransform.gameObject != null)
            {
                equipedItem_UI_Layout.UnequipUI();
                equipedItem_UI_Layout.RemoveItem_UI(null);
                Destroy(childTransform.gameObject);
            }
            else
            {
                Debug.LogError("Attempted to destroy a non-existent gameObject");
            }
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
        if (_item.TryGetAction(out ItemAction itemAction))
        {
            ItemTagLibrary itemTagLibrary = _item.itemTag.GetItemTagLibrary;
            if (itemAction is HealingItemAction)
            {
                HealingItemAction healingItemAction = itemAction as HealingItemAction;
                foreach (string tag in itemTagLibrary.healingTags)
                {
                    if (healingItemAction != null)
                    {
                        if (_item.itemTag.GetTag == tag)
                        {
                            ActionResult<float> amount = healingItemAction.ExecuteFunction<float>(_item);
                            float healPoints = healingItemAction.healPoints;
                            if (healingItemAction.itemFunction.functionType == ItemAction.FunctionType.Healing_HP)
                            {
                                playerStats.Healing(ItemAction.FunctionType.Healing_HP, amount.Value);
                            }
                            else if (healingItemAction.itemFunction.functionType == ItemAction.FunctionType.Healing_MP)
                            {
                                playerStats.Healing(ItemAction.FunctionType.Healing_MP, amount.Value);
                            }
                        }
                        else
                        {
                            Debug.LogError("healingItemAction es null.");
                        }
                    }
                }
            }
            else if (itemAction is FoodItemAction)
            {
                FoodItemAction foodItemAction = itemAction as FoodItemAction;
                foreach (string tag in itemTagLibrary.foodTags)
                {
                    if (foodItemAction != null)
                    {
                        if (_item.itemTag.GetTag == tag)
                        {
                            ActionResult<float> amount = foodItemAction.ExecuteFunction<float>(_item);
                            float foodFillTime = foodItemAction.eatFillTime;
                            Debug.Log("Feeding");
                            //Introducir metodo de llenado de hambre
                        }

                    }
                    else
                    {
                        Debug.LogError("specialItemAction es null.");
                    }
                }
            }
            else if (itemAction is StatBoostItemAction)
            {
                StatBoostItemAction statBoostItemAction = itemAction as StatBoostItemAction;
                foreach (string tag in itemTagLibrary.statBoostTags)
                {
                    if (statBoostItemAction != null)
                    {
                        if (_item.itemTag.GetTag == tag)
                        {
                            ActionResult<float> amount = statBoostItemAction.ExecuteFunction<float>(_item);
                            float statBoostedTime = statBoostItemAction.duration;
                            //Introducir metodo de reparacion de equipamiento
                        }

                    }
                    else
                    {
                        Debug.LogError("specialItemAction es null.");
                    }
                }
            }
            else if (itemAction is RepairItemAction)
            {
                RepairItemAction repairItemAction = itemAction as RepairItemAction;
                foreach (string tag in itemTagLibrary.repairTags)
                {
                    if (repairItemAction != null)
                    {
                        if (_item.itemTag.GetTag == tag)
                        {
                            ActionResult<float> amount = repairItemAction.ExecuteFunction<float>(_item);
                            float repairTime = repairItemAction.repairingFillTime;
                            //Introducir metodo de reparacion de equipamiento
                        }

                    }
                    else
                    {
                        Debug.LogError("specialItemAction es null.");
                    }
                }
            }
            else if (itemAction is SpecialItemAction)
            {
                SpecialItemAction specialItemAction = itemAction as SpecialItemAction;
                foreach (string tag in itemTagLibrary.specialTags)
                {
                    if (specialItemAction != null)
                    {
                        if (_item.itemTag.GetTag == tag)
                        {
                            ActionResult<Action> actionResult = specialItemAction.ExecuteAction(( ) => UseKey(_item));
                        }

                    }
                    else
                    {
                        Debug.LogError("specialItemAction es null.");
                    }
                }
            }
        }
    }
    private void UseKey(Item _item )
    {
        Item itemToUse = _item.CreateInstance() as Item;

        if (activableAltar == null)
        {
            return;
        }
        if (activableAltar.AltarIsEnabled()) return;

        OnPlaceKey?.Invoke(this, EventArgs.Empty);
        LoadSceneManager.instance.LoadSceneAsync();
        activableAltar.SetAltarState(true);
        activableAltar.TurnExitOn();
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
