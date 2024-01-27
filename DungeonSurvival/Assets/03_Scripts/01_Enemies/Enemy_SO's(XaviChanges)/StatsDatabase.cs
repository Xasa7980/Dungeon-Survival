using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StatsDatabase 
{
    readonly public int Get_HpOnBaseVit ( int vitPoints )
    {
        return 25 * vitPoints;
    }
    readonly public int Get_MpOnBaseInt ( int intPoints )
    {
        return Mathf.RoundToInt(7.5f * intPoints);
    }
    readonly public int Get_AtkOnBasePower ( int strPoints )
    {
        return Mathf.RoundToInt(3f * strPoints);
    }
    readonly public int Get_DefenseOnBaseRes ( int resPoints )
    {
        return Mathf.RoundToInt(3f * resPoints);
    }
    readonly public float Get_CritRateOnBaseDexAndAgi ( int dexPoints, int agiPoints )
    {
        return Mathf.RoundToInt((((0.05f * dexPoints) * 0.2F) + ((0.05F * agiPoints) * 0.2f)));
    }
    readonly public float Get_CritDmgOnBaseDex ( int dexPoints )
    {
        return (1.5f * 100 + (dexPoints / 10)) / 100;
    }
    readonly public int Get_ElemRESOnBaseRESAndINT ( int resPoints, int intPoints )
    {
        return Mathf.RoundToInt((1.5f * resPoints * 0.75f) + (1.5f * intPoints * 0.75f));
    }
}
