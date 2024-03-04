using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivableAltar : MonoBehaviour
{
    //Salta cinematica
    //Genera room
    //Activa Efectos
    //Camera Shake
    public event EventHandler OnPlaceKey;
    private bool altarIsEnabled;

    public void SetAltarState(bool _altarIsEnabled )
    {
        altarIsEnabled = _altarIsEnabled;
    }
    public bool AltarIsEnabled( )
    {
        return altarIsEnabled;
    }
    public void TurnExitOn ( DungeonExits exit)
    {
        if (altarIsEnabled) return;
        exit.SetExitType(ExitType.Exit);
        OnPlaceKey?.Invoke(this,EventArgs.Empty);
    }
    public void TurnEntranceOn(DungeonManager dungeonManager)
    {
        if (dungeonManager.HasEntrance()) return;
        if(altarIsEnabled) return;

        DungeonExits[] dungeonExits = dungeonManager.exits;
        dungeonExits[UnityEngine.Random.Range(0, dungeonExits.Length)].SetExitType(ExitType.Entrance);
    }
}
