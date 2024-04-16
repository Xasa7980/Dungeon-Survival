using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
