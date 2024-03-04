using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public DungeonExits[] exits;

    public DungeonExits currentExit;
    public DungeonExits currentEntrance;

    private ActivableAltar activableAltar;

    private bool hasEntrance;
    private void Start ( )
    {
        activableAltar = PlayerInventory.current.activableAltar;
        activableAltar.OnPlaceKey += ActivableAltar_OnPlaceKey;
        LoadSceneManager.instance.OnSceneLoaded += LoadSceneManager_OnSceneLoaded;
    }

    private void LoadSceneManager_OnSceneLoaded ( object sender, System.EventArgs e )
    {
        throw new System.NotImplementedException();
    }

    public DungeonExits GetActivedExit ( )
    {
        foreach ( DungeonExits ex in exits)
        {
            if (ex.IsExit())
            {
                currentExit = ex;
                return ex;
            }
        }
        Debug.LogError("Not Found exit");
        return null;
    }
    public DungeonExits GetActivedEntrance ( )
    {
        foreach (DungeonExits ex in exits)
        {
            if (ex.IsEntrance())
            {
                currentEntrance = ex;
                return ex;
            }
        }
        Debug.LogError("NotFound entrance");
        return null;
    }
    private void ActivableAltar_OnPlaceKey ( object sender, System.EventArgs e )
    {
        activableAltar.TurnEntranceOn(this);
    }

    public DungeonExits SetEntrance ( )
    {
        activableAltar.TurnEntranceOn(this);
        return exits.Where(x => x.IsEntrance()).FirstOrDefault();
    }
    public DungeonExits SetExit ( )
    {
        DungeonExits exit = exits.Where(ex => ex.transform.GetComponentInChildren<DungeonExits>()).First();
        activableAltar.TurnExitOn(exit);

        return exits.Where(x => x.IsEntrance()).FirstOrDefault();
    }
    public bool HasEntrance ( )
    {
        return hasEntrance;
    }
    public void SetPosition ( Vector3 position )
    {
        gameObject.transform.position = position;
    }
    public Vector3 GetPosition ( )
    {
        return gameObject.transform.position;
    }
}
