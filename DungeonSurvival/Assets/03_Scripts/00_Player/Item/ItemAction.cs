using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public class ItemAction : ScriptableObject
{
    public enum FunctionType
    {
        Food,
        Healing_HP,
        Healing_MP,
        Boosting_Stats,
        Repairing,
        SpecialFunction,
        Magic,
    }
    public virtual ActionResult<T> ExecuteFunction<T>( Item item )
    {
        throw new NotImplementedException("ExecuteFunction must be implemented by the derived class.");
    }
    public virtual ActionResult<Action> ExecuteAction ( Action action )
    {
        return new ActionResult<Action>(true, action, "Implementación predeterminada");
    }
    public ItemFunctions itemFunction;

    [System.Serializable]
    public struct ItemFunctions
    {
        public FunctionType functionType;
        public float duration;
    }

}
