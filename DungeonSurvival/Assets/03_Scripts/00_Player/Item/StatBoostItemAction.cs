using UnityEngine;

[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/ItemAction/BoostStats")]
public class StatBoostItemAction : ItemAction
{
    public enum StatToBoost
    {
        Boost_Health,
        Boost_Mana,
        Boost_Attack,
        Boost_AttackSpeed,
        Boost_Defense,
        Boost_CriticalRate,
        Boost_CriticalDamage
    }
    public float statPoints;
    public float duration => itemFunction.duration;
    public ItemTag itemType;
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
