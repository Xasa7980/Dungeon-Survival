using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New_MonsterLibrary", menuName = ("Dungeon Survival/Enemy"))]
public class MonstersLibraries : ScriptableObject
{
    [SerializeField] MonsterDataSO[] allMonsterDatas;

    public MonsterDataSO[] NoneElementMonsterDatas => allMonsterDatas
     .Where(mns => mns.monsterElement == MonsterElement.None)
     .ToArray();

    public MonsterDataSO[] WaterElementMonsterDatas => allMonsterDatas
        .Where(mns => mns.monsterElement == MonsterElement.Water)
        .ToArray();

    public MonsterDataSO[] FireElementMonsterDatas => allMonsterDatas
        .Where(mns => mns.monsterElement == MonsterElement.Fire)
        .ToArray();

    public MonsterDataSO[] DarknessElementMonsterDatas => allMonsterDatas
        .Where(mns => mns.monsterElement == MonsterElement.Darkness)
        .ToArray();

    public MonsterDataSO[] LightElementMonsterDatas => allMonsterDatas
        .Where(mns => mns.monsterElement == MonsterElement.Light)
        .ToArray();

    public MonsterDataSO[] ThunderElementMonsterDatas => allMonsterDatas
        .Where(mns => mns.monsterElement == MonsterElement.Thunder)
        .ToArray();

    public MonsterDataSO[] EarthElementMonsterDatas => allMonsterDatas
        .Where(mns => mns.monsterElement == MonsterElement.Earth)
        .ToArray();

    public MonsterDataSO[] WindElementMonsterDatas => allMonsterDatas
        .Where(mns => mns.monsterElement == MonsterElement.Wind)
        .ToArray();

    public MonsterDataSO[] IceElementMonsterDatas => allMonsterDatas
        .Where(mns => mns.monsterElement == MonsterElement.Ice)
        .ToArray();
}
