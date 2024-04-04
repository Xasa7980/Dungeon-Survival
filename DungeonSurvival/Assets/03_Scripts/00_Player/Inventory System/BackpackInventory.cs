using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackInventory : MonoBehaviour, iInventory
{
    public static BackpackInventory current { get; private set; }

    public event EventHandler<iInventory> InventoryChanged; //Por ahora no necesario
    public event EventHandler<iInventory> InventoryCleared; //Por ahora no necesario

    [SerializeField] int maxItems = 0;
    public InventoryItem[] allItems { get; private set; }
    public int keys { get; private set; }
    private void Awake ( )
    {
        current = this;
    }
    private void Start ( )
    {
        allItems = new InventoryItem[maxItems];
        PlayerInventory.current.OnEquipBackpack += PlayerInventory_OnEquipBackpack; ;
        PlayerInventory.current.OnUnequipBackpack += PlayerInventory_OnUnequipBackpack;
    }

    private void PlayerInventory_OnUnequipBackpack ( object sender, EventArgs e )
    {
        if(PlayerInventory.current.GetEquipedBackpack == null)
        {
            maxItems = 0;
            return;
        }
        maxItems = PlayerInventory.current.GetEquipedBackpack.maxItems;
    }

    private void PlayerInventory_OnEquipBackpack ( object sender, EventArgs e )
    {
        maxItems = PlayerInventory.current.GetEquipedBackpack.maxItems;
    }

    public void AddKey ( )
    {
        keys++;
    }

    public bool TryUseKey ( )
    {
        if (keys > 0)
        {
            keys--;
            return true;
        }

        return false;
    }

    void DropItem ( Item item )
    {
        Vector3 position = UnityEngine.Random.insideUnitSphere * 2;
        position.y = 0;

        item.InstantiateInWorld(PlayerLocomotion.current.transform.position + position);
    }
    public void UseItem ( Item _item )
    {

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
    public bool TryAddItem ( Item item ) //Por usar
    {
        if (!((iInventory)this).TryAddItem(item, out int index))
        {
            return TryAddItemToBackPack(item);
        }

        if (index >= 0)
            PlayerInventory_UI_Manager.current.AddItemToQuickAccess(index, allItems[index]);

        return true;
    }

    public bool TryRemoveItem ( Item item ) // Por usar
    {
        if (!((iInventory)this).TryRemoveItem(item, out int index))
        {
            return TryRemoveItemFromBackpack(item);
        }

        if (index >= 0)
            PlayerInventory_UI_Manager.current.AddItemToQuickAccess(index, null);

        return true;
    }

    bool TryAddItemToBackPack ( Item item )
    {
        if (((iInventory)this).TryAddItem(item, out int index, true))
        {
            //Update backpack UI here
            if (index >= 0)
            {
                PlayerInventory_UI_Manager.current.AddItemToBackpack(index, this.allItems[index]);
            }

            return true;
        }
        else
            return false;
    }

    bool TryRemoveItemFromBackpack ( Item item )
    {
        if (((iInventory)this).TryRemoveItem(item, out int index))
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
