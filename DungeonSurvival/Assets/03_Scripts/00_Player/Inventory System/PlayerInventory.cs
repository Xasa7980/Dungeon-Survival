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

    Item_Backpack backpack;
    public Item_Backpack GetEquipedBackpack => backpack;
    public int keys { get; private set; }

    [SerializeField] private PlayerInteraction playerInteraction;
    public event EventHandler OnPlaceKey;
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        allItems = new InventoryItem[maxItems];
        allEquipableItems = new InventoryItem[maxEquipableItems];
        playerInteraction.OnInteractAnyObject += PlayerInteraction_OnInteractAnyObject;
    }
    public ActivableAltar activableAltar;
    private void PlayerInteraction_OnInteractAnyObject ( object sender, System.EventArgs e )
    {
        //if(e.objectInteracted.TryGetComponent<ActivableAltar>(out ActivableAltar activableAltar))
        PlayerInteraction playerInteraction = sender as PlayerInteraction;
        this.activableAltar = playerInteraction.possibleInteraction.GetComponent<ActivableAltar>();
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
        backpackInstance.EquipOnPlayer(PlayerHolsterHandler.current.backpack);
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

    void DropItem(Item item)
    {
        Vector3 position = UnityEngine.Random.insideUnitSphere * 2;
        position.y = 0;

        item.InstantiateInWorld(PlayerLocomotion.current.transform.position + position);
    }
    public void UseItem(Item _item )
    {
        ItemAction itemAction = _item.itemAction;
        ItemTagLibrary itemTagLibrary = new();
        foreach (string tag in itemTagLibrary.specialTags)
        {
            if (itemAction != null)
            {
                if(_item.itemTag.tag == tag)
                {
                    ActionResult<Action> actionResult = itemAction.ExecuteAction(( ) => UseKey(_item));
                }
            }
            else
            {
                Debug.LogError("itemAction es null.");
            }
        }

        //switch (_item.itemAction.itemFunction.functionType)
        //{
        //    case ItemAction.FunctionType.Food when _item.itemAction.itemFunction.itemTag == "Meat":
        //        {
        //            ActionResult<float> amount = itemAction.ExecuteFunction<float>(_item);
        //            GetComponent<PlayerStats>().Healing(ItemAction.FunctionType.Healing_HP, amount.Value);
        //            break;
        //        }
        //    case ItemAction.FunctionType.Healing_HP when _item.itemAction.itemFunction.itemTag == "Healing_HP":
        //        {
        //            ActionResult<float> amount = itemAction.ExecuteFunction<float>(_item);
        //            GetComponent<PlayerStats>().Healing(ItemAction.FunctionType.Healing_HP, amount.Value);
        //            break;
        //        }
        //    case ItemAction.FunctionType.Healing_MP when _item.itemAction.itemFunction.itemTag == "Healing_MP":
        //        {
        //            ActionResult<float> amount = itemAction.ExecuteFunction<float>(_item);
        //            GetComponent<PlayerStats>().Healing(ItemAction.FunctionType.Healing_HP, amount.Value);
        //            break;

        //        }
        //    case ItemAction.FunctionType.Boosting_Stats when _item.itemAction.itemFunction.itemTag == "Boost_Atk":
        //        {
        //            ActionResult<float> amount = itemAction.ExecuteFunction<float>(_item);
        //            GetComponent<PlayerStats>().Healing(ItemAction.FunctionType.Healing_HP, amount.Value);
        //            break;
        //        }
        //    case ItemAction.FunctionType.Repairing when _item.itemAction.itemFunction.itemTag == "Repair":
        //        {
        //            ActionResult<float> amount = itemAction.ExecuteFunction<float>(_item);
        //            GetComponent<PlayerStats>().Healing(ItemAction.FunctionType.Healing_HP, amount.Value);
        //            break;
        //        }
        //    case ItemAction.FunctionType.SpecialFunction when _item.itemAction.itemFunction.itemTag == "Key":
        //        {
        //            foreach (string tag in itemTagLibrary.specialTags)
        //            {
        //                if(itemAction != null)
        //                {
        //                    if (_item.itemAction.itemFunction.itemTag == tag)
        //                    {
        //                        ActionResult<Action> actionResult = itemAction.ExecuteAction(( ) => UseKey(_item));
        //                    }
        //                }
        //                else
        //                {
        //                    Debug.LogError("itemAction es null.");
        //                }
        //            }
        //            break;
        //        }
        //    case ItemAction.FunctionType.Magic when _item.itemAction.itemFunction.itemTag == "Meat":
        //        {
        //            ActionResult<float> amount = itemAction.ExecuteFunction<float>(_item);
        //            GetComponent<PlayerStats>().Healing(ItemAction.FunctionType.Healing_HP, amount.Value);
        //            break;
        //        }
        //    default:
        //        break;
        //}
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
            if(targetSlot.inventoryType != InventoryType.B)
            {
                PlayerInventory_UI_Manager.current.AddItemToQuickAccess(index, allItems[index]);
            }
            PlayerInventory_UI_Manager.current.AddItemToBackpack(index, allItems[index]);
        }

        return true;
    }
    public bool TryRemoveItem(Item item) // Por usar
    {
        if (!((iInventory)this).TryRemoveItem(item, out int index))
        {
            if (backpack != null)
            {
                return TryRemoveItemFromBackpack(item);
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

    bool TryRemoveItemFromBackpack(Item item)
    {
        if (((iInventory)backpack).TryRemoveItem(item, out int index))
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
public class ItemTagLibrary
{
    public string itemTag;
    public string[] foodTags = new string[] { "Meat", "Wheat", "Rice" };
    public string[] healHPTags = new string[] { "Meat", "Wheat", "Rice" };
    public string[] healMPTags = new string[] { "Meat", "Wheat", "Rice" };
    public string[] repairTags = new string[] { "Meat", "Wheat", "Rice" };
    public string[] specialTags = new string[] { "Key", "Wheat", "Rice" };
    public string[] equipmentTags = new string[] { "Key", "Wheat", "Rice" };
    public string[] othersTags = new string[] { "Key", "Wheat", "Rice" };
}
public static class ItemTagLibraryExtensions
{
    public static string SetTag ( this ItemTagLibrary itemTagLibrary, string itemTag )
    {
        return itemTagLibrary.itemTag = itemTag;
    }
    public static string GetTag ( this ItemTagLibrary itemTagLibrary )
    {
        if( string.IsNullOrWhiteSpace(itemTagLibrary.itemTag) )
        {
            Debug.LogError("There is not tag in this itemTagLibrary");
            return null;
        }
        return itemTagLibrary.itemTag;
    }
}