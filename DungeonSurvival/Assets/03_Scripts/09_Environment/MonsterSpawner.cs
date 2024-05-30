using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : SpawnerManager
{
    public event EventHandler OnRechargedSpawning;
    private void Update ( )
    {
        if(TryDetectPlayer(out GameObject player))
        {
            Spawn();
        }
        else if(!spawnAble)
        {
            OnRechargedSpawning?.Invoke ( this, EventArgs.Empty );
            StartCoroutine(SpawnDelay(spawnDelay));
        }
    }

}
