using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/ItemAction/SpecialItemAction")]
public class SpecialItemAction : ItemAction
{
    public float healPoints;
    public float healingTime;
    public override ActionResult<Action> ExecuteAction ( Action action )
    {
        if (itemFunction.functionType == FunctionType.SpecialFunction)
        {
            action.Invoke();
            return new ActionResult<Action>(true, action, "realizada la funcion");
        }
        Debug.Log("FunctionType not done correctly, it has no FunctionType");
        return new ActionResult<Action>(false, default, "realizada la funcion");
    }

    public override ActionResult<T> ExecuteFunction<T> ( Item item )
    {
        throw new NotImplementedException();
    }
}
