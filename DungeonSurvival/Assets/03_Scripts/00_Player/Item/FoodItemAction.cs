using UnityEngine;

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
