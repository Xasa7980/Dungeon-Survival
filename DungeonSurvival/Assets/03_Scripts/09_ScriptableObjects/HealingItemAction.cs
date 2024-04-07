using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Item;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/ItemAction/HealingAction")]
public class HealingItemAction : ItemAction
{
    public float healPoints;
    public float healingFillTime;
    public override ActionResult<T> ExecuteFunction<T> ( Item item )
    {
        if(typeof(T) == typeof(float))
        {
            if (itemFunction.functionType == FunctionType.Healing_HP)
            {
                return new ActionResult<T>(true,(T)(object)healPoints);
            }
            else if (itemFunction.functionType == FunctionType.Healing_MP)
            {
                return new ActionResult<T>(true, (T)(object)healPoints);
            }
        }
        Debug.Log("FunctionType not done correctly, it has no FunctionType");
        return new ActionResult<T>(false, default(T), "No realizada la funcion");
    }
}
[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/ItemAction/EatAction")]
public class FoodItemAction : ItemAction
{
    public float hungerPoints;
    public float eatFillTime;
    public override ActionResult<T> ExecuteFunction<T> ( Item item )
    {
        if (typeof(T) == typeof(float))
        {
            if (itemFunction.functionType == FunctionType.Food)
            {
                return new ActionResult<T>(true, (T)(object)hungerPoints);
            }
            else
            {
                return new ActionResult<T>(true, (T)(object)hungerPoints);
            }
        }
        Debug.Log("FunctionType not done correctly, it has no FunctionType");
        return new ActionResult<T>(false, default(T), "No realizada la funcion");
    }
}
[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/ItemAction/BoostStats")]
public class BoostItemAction : ItemAction
{
    public float statPoints;
    public ItemTag tag;
    public override ActionResult<T> ExecuteFunction<T> ( Item item )
    {
        if (typeof(T) == typeof(float))
        {
            if (itemFunction.functionType == FunctionType.Boosting_Stats)
            {
                return new ActionResult<T>(true, (T)(object)statPoints);
            }
            else
            {
                return new ActionResult<T>(true, (T)(object)statPoints);
            }
        }
        Debug.Log("FunctionType not done correctly, it has no FunctionType");
        return new ActionResult<T>(false, default(T), "No realizada la funcion");
    }
}
[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/ItemAction/RepairAction")]
public class RepairItemAction : ItemAction
{
    public float repairingPoints;
    public float repairingFillTime;
    public override ActionResult<T> ExecuteFunction<T> ( Item item )
    {
        if (typeof(T) == typeof(float))
        {
            if (itemFunction.functionType == FunctionType.Repairing)
            {
                return new ActionResult<T>(true, (T)(object)repairingPoints);
            }
            else
            {
                return new ActionResult<T>(true, (T)(object)repairingPoints);
            }
        }
        Debug.Log("FunctionType not done correctly, it has no FunctionType");
        return new ActionResult<T>(false, default(T), "No realizada la funcion");
    }
}

//public class MagicItemAction : ItemAction
//{
//    public float healPoints;
//    public float healingTime;
//    public override ActionResult<T> ExecuteFunction<T> ( Item item, T value )
//    {
//        if (typeof(T) == typeof(float))
//        {
//            if (itemFunction.functionType == FunctionType.Healing_HP)
//            {
//                float initialValue = (float)(object)value;
//                float increment = healPoints;
//                float fullFillPercent = Mathf.Lerp(initialValue, initialValue + increment, healingTime);
//                return new ActionResult<T>(true, (T)(object)fullFillPercent);
//            }
//            else if (itemFunction.functionType == FunctionType.Healing_MP)
//            {
//                float initialFillBar = (float)(object)value;
//                float increment = healPoints;
//                float fullFillPercent = Mathf.Lerp(initialFillBar, initialFillBar += increment, healingTime);
//                return new ActionResult<T>(true, (T)(object)fullFillPercent);
//            }
//        }
//        Debug.Log("FunctionType not done correctly, it has no FunctionType");
//        return new ActionResult<T>(false, default(T), "No realizada la funcion");
//    }
//}
public class ActionResult<T>
{
    public bool Success { get; set; }
    public T Value { get; set; }
    public string Message { get; set; } // opcional

    public ActionResult ( bool success, T value, string message = "" )
    {
        Success = success;
        Value = value;
        Message = message;
    }
}