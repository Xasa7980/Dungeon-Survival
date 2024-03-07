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

    [SerializeField] private DungeonExits exit;
    
    [SerializeField] private GameObject altar;
    
    private bool altarIsEnabled;
    public DungeonExits TurnExitOn ( )
    {
        exit.SetExitType(ExitType.Exit);
        OnPlaceKey?.Invoke(this,EventArgs.Empty);
        return exit;
    }
    public void TurnEntranceOn(DungeonManager dungeonManager)
    {
        DungeonExits[] dungeonExits = dungeonManager.exits;
        dungeonExits[UnityEngine.Random.Range(0, dungeonExits.Length)].SetExitType(ExitType.Entrance);
    }
    public DungeonExits GetEnabledAltarExit ( )
    {
        return exit;
    }
    public void SetAltarState ( bool _altarIsEnabled )
    {
        altarIsEnabled = _altarIsEnabled;
    }
    public bool AltarIsEnabled ( )
    {
        return altarIsEnabled;
    }
    private void Show ( )
    {
        altar.SetActive( true );
    }
    private void Hide ( )
    {
        altar.SetActive ( false );
    }
}
