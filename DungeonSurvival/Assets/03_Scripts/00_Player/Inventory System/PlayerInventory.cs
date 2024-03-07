using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, iInventory
{
    public static PlayerInventory current { get; private set; }

    [SerializeField] int maxItems = 9;
    public InventoryItem[] allItems { get; private set; }

    Item_Backpack backpack;
    Item item;
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
        this.backpack = backpackInstance;
        backpackInstance.Init();
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
                if (_item.itemAction.itemFunction.itemTag == tag)
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
        item = itemToUse;
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
            else
                return false;
        }

        if (index >= 0)
            PlayerInventory_UI_Manager.current.AddItem(index, allItems[index]);

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
            PlayerInventory_UI_Manager.current.AddItem(index, null);

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
    public string[] foodTags = new string[] { "Meat", "Wheat", "Rice" };
    public string[] healHPTags = new string[] { "Meat", "Wheat", "Rice" };
    public string[] healMPTags = new string[] { "Meat", "Wheat", "Rice" };
    public string[] repairTags = new string[] { "Meat", "Wheat", "Rice" };
    public string[] specialTags = new string[] { "Key", "Wheat", "Rice" };
}