using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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


    Item equippedMainWeapon;
    Item equippedSecondaryWeapon;
    Item equippedHelmet;
    Item equippedChest;
    Item equippedGloves;
    Item equippedPants;
    Item equippedBoots;
    Item equippedNecklace;
    Item equippedRing;
    public Item[] GetEquippedItems => new Item[]
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
    public void EquipItem(Item item)
    {
        int index = 0;

        for (int i = 0; i < GetEquippedItems.Length; i++)
        {
            if (GetEquippedItems[i] == null) continue;

            if (item.itemTag == GetEquippedItems[i].itemTag)
            {
                Debug.Log("Already has an item, let's unequip");
                UnequipItem(GetEquippedItems[i]);
                index = i;
            }
        }

        Item equipItemInstance = item.CreateInstance() as Item;
        //this.TryRemoveItem(item,1);
        GetEquippedItems[index] = equipItemInstance;
        PlayerInventory_UI_Manager.current.EquipItem(equipItemInstance);
        for (int i = 0; i < PlayerHolsterHandler.current.allActualHolsters.Length; i++)
        {
            if (PlayerHolsterHandler.current.allActualHolsters[i] != null)
            {
                equipItemInstance.EquipVisuals(PlayerHolsterHandler.current.allActualHolsters[i]);
            }
        }
        Debug.Log("hea2");
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
    public void UnequipItem ( Item item )
    {
        //if (!this.TryAddItem(backpack))
        //{
        //backpack.InstantiateInWorld(PlayerLocomotion.current.transform.position, backpack);
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
        Destroy(PlayerHolsterHandler.current.allActualHolsters.Where(r => r.GetChild(0).GetComponent<EquipmentDataHolder>().IsType(item)).First());

        item = null;
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
                Debug.Log("added to backpack");
                return TryAddItemToBackPack(item);
            }

            Debug.Log("added to quickAcces");
            return false;
        }

        if (index >= 0)
        {
            Debug.Log("added to quickAcces by default");
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
                Debug.Log("added to backpack");
                return TryAddItemToBackPack(item);
            }

            Debug.Log("added to quickAcces");
            return false;
        }

        if (index >= 0)
        {
            Debug.Log("added to quickAcces by default");
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
