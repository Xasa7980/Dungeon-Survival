using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public DungeonExits[] exits;

    public DungeonExits currentExit;
    public DungeonExits currentEntrance;

    private bool hasEntrance;
    private bool hasExit;

    private void Start ( )
    {
        currentEntrance = SetEntrance();
        LoadSceneManager.instance.OnSceneLoaded += LoadSceneManager_OnSceneLoaded;
        LoadSceneManager.instance.CalculateRoomTransform(currentEntrance,gameObject);
    }
    private void Update ( )
    {
        if(exits.Any(sD => sD.IsExit()))
        {
            currentExit = exits.First(sD => sD.IsExit());
        }
    }
    private void LoadSceneManager_OnSceneLoaded ( object sender, System.EventArgs e )
    {
        throw new System.NotImplementedException();
    }

    public DungeonExits SetEntrance ( ) // Next room entrance
    {
        if (currentEntrance != null)
        {
            hasEntrance = true;
            return currentEntrance;
        }
        currentEntrance = exits[UnityEngine.Random.Range(0, exits.Length)];

        currentEntrance.GetComponentInParent<ActivableAltar>().TurnEntranceOn(this);
        hasEntrance = true;

        return currentEntrance;
    }
    public DungeonExits GetActivedExit ( )
    {
        foreach (DungeonExits ex in exits)
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
    public bool HasEntrance ( )
    {
        return hasEntrance;
    }
    public bool HasExit ( )
    {
        return hasExit;
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
