using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : SpawnerManager
{
    public enum SpawnerElement
    {
        None,
        Water,
        Fire,
        Darkness,
        Light,
        Thunder,
        Earth,
        Wind,
        Ice
    }
    [SerializeField] private SpawnerElement spawnerElement;

    public override void Spawn ( )
    {
        base.Spawn();
    }
}
