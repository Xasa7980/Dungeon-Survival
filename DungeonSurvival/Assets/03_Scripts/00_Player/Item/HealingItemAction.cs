using UnityEngine;

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