using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public interface iItemPerformingAction
{
    public void PerformSpecialAction ( PlayerInventory playerInventory, Item _item );
}
public class GenericAction : iItemPerformingAction
{
    private Action<PlayerInventory, Item, int, int> _action;
    private int _value;
    private int _duration;

    public GenericAction ( Action<PlayerInventory, Item, int, int> action, int value, int duration )
    {
        _action = action;
        _value = value;
        _duration = duration;
    }

    public void PerformSpecialAction ( PlayerInventory playerInventory, Item item )
    {
        _action(playerInventory, item, _value, _duration);
    }
}
public class ItemActionFactory
{
    private static ItemActionFactory _instance;
    public static ItemActionFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ItemActionFactory();
            }
            return _instance;
        }
    }
    private Dictionary<string, Func<PlayerInventory, Item, int, int, iItemPerformingAction>> _allActionsDictionary;
    public Dictionary<string, Func<PlayerInventory, Item, int, int, iItemPerformingAction>> allActionsDictionary => _allActionsDictionary;

    public ItemActionFactory ( )
    {
        _allActionsDictionary = new Dictionary<string, Func<PlayerInventory, Item, int, int, iItemPerformingAction>>();
        InitializeActions();
    }

    private void InitializeActions ( )
    {
        _allActionsDictionary.Add("Heal", ( inventory, item, value, duration ) =>
            new GenericAction(( inventory, item, value, duration ) =>
            {
                //Aqui va la funcion
                Debug.Log($"Healing {item.name} for {value} HP over {duration} seconds.");
            }, 100, 10));

        _allActionsDictionary.Add("BoostHealth", ( inventory, item, value, duration ) =>
            new GenericAction(( inventory, item, value, duration ) =>
            {
                //Aqui va la funcion
                PlayerComponents.instance.playerStats.BoostMaxHealth(value, item.itemQualityLevel);
                Debug.Log($"Boosting {item.name} for {value} points of speed for {duration} seconds.");
            }, 20, 5));
        _allActionsDictionary.Add("Key", ( inventory, item, value, duration ) =>
            new GenericAction(( inventory, item, value, duration ) =>
            {
                Item itemToUse = item.CreateInstance() as Item;

                if (inventory.activableAltar == null)
                {
                    return;
                }
                if (inventory.activableAltar.AltarIsEnabled()) return;

                LoadSceneManager.instance.LoadSceneAsync();
                inventory.activableAltar.SetAltarState(true);
                inventory.activableAltar.TurnExitOn();

                Debug.Log($"Opening door/new map");
            }, 20, 5));
    }

}
public static class ItemActionFactoryExtensions
{
    public static iItemPerformingAction GetActionByItemTag ( this ItemActionFactory actionFactory, string tag, PlayerInventory inventory, Item item, int value, int duration )
    {
        if (actionFactory.allActionsDictionary.TryGetValue(tag, out Func<PlayerInventory,Item,int,int, iItemPerformingAction> action))
        {
            return action(inventory, item, value, duration);
        }

        Debug.LogError($"No action found for tag: {tag}");
        return null;
    }
}
#region SpecialItemActions
//public class KeyAction : iItemPerformingAction
//{
//    public void PerformSpecialAction ( PlayerInventory playerInventory, Item _item )
//    {
//        Item itemToUse = _item.CreateInstance() as Item;

//        if (playerInventory.activableAltar == null)
//        {
//            return;
//        }
//        if (playerInventory.activableAltar.AltarIsEnabled()) return;

//        LoadSceneManager.instance.LoadSceneAsync();
//        playerInventory.activableAltar.SetAltarState(true);
//        playerInventory.activableAltar.TurnExitOn();
//    }
//}
#endregion
//public class FoodAction : iItemPerformingAction
//{
//    public int amountToFeed;
//    public int durationTime;

//    public FoodAction ( int amountValue, int _durationValue )
//    {
//        amountToFeed = amountValue;
//        durationTime = _durationValue;
//    }
//    public void PerformSpecialAction ( PlayerInventory playerInventory, Item _item )
//    {
//        throw new NotImplementedException();
//    }
//}
//public class HealingAction : iItemPerformingAction
//{
//    public int amountToHeal; 
//    public int durationTime;

//    public HealingAction ( int amountValue, int _durationValue )
//    {
//        amountToHeal = amountValue;
//        durationTime = _durationValue;
//    }
//    public void PerformSpecialAction ( PlayerInventory playerInventory, Item _item )
//    {
//        throw new NotImplementedException();
//    }
//}
//public class RepairAction : iItemPerformingAction
//{
//    public int amountToRepair;
//    public int durationTime;
//    public RepairAction ( int amountValue, int _durationValue )
//    {
//        amountToRepair = amountValue;
//        durationTime = _durationValue;
//    }
//    public void PerformSpecialAction ( PlayerInventory playerInventory, Item _item )
//    {
//        throw new NotImplementedException();
//    }
//}
//public class BoostStatAction : iItemPerformingAction
//{
//    public int amountToBoost;
//    public int durationTime;
//    public BoostStatAction ( int amountValue, int _durationValue)
//    {
//        amountToBoost = amountValue;
//        durationTime = _durationValue;
//    }
//    public void PerformSpecialAction ( PlayerInventory playerInventory, Item _item )
//    {
//        throw new NotImplementedException();
//    }
