using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New_MonsterLibrary", menuName = ("Dungeon Survival/Enemy"))]
public class MonstersLibraries : ScriptableObject
{
    public struct MonstersData
    {
        public MonsterRank monsterRank => monsterDataSO.monsterRank;
        public MonsterDataSO monsterDataSO;
    }

    [SerializeField] private MonstersData[] allMonsterDatas;

    // Métodos para obtener tuplas de MonstersData y niveles del mundo
    public (MonstersData, int)[] GetMonstersByElement ( Element element, WorldLevel worldLevel ) //Array de tuplas
    {
        return allMonsterDatas
            .Where(mns => mns.monsterDataSO.monsterElement == element && IsRankAllowed(worldLevel.worldLevel, mns.monsterRank))
            .Select(mns => (mns, worldLevel.worldLevel))
            .ToArray(); 
        /* Devolveremos todas las tuplas en los que su elemento coincida y el nivel del mundo permita al rango */
    }

    private bool IsRankAllowed ( int worldLevel, MonsterRank rank )
    {
        switch (rank)
        {
            case MonsterRank.Minion:
                return true;
            case MonsterRank.Soldier:
                return worldLevel > 3;
            case MonsterRank.General:
                return worldLevel > 5;
            case MonsterRank.Guardian:
                return worldLevel > 7;
            case MonsterRank.Boss:
                return worldLevel > 10;
            case MonsterRank.EliteBoss:
                return worldLevel > 25;
            default:
                return false;
        }
    }
}

// Clase WorldLevel
public class WorldLevel : MonoBehaviour
{
    public int worldLevel;
}