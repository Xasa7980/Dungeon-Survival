using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public event EventHandler OnGetSpawnLimit;
    public event EventHandler OnSpawn;

    public float spawnRadius;
    public float spawnOffset;
    
    public bool hasSpawnLimit = false;
    public bool hasRandomAmountToSpawn = false;
    public ObjectPool objectPool;
    

    [SerializeField, ShowIf("@hasSpawnLimit")] private int spawnLimit = 1;
    [SerializeField, ShowIf("@hasRandomAmountToSpawn")] private int minAmountToSpawn = 1;
    [SerializeField, ShowIf("@hasRandomAmountToSpawn")] private int maxAmountToSpawn = 1;

    [SerializeField] private float spawnDelay;
    private bool spawnAble;

    private int spawnedAmount;
    private int amountToSpawn => SetRandomAmountToSpawn(minAmountToSpawn,maxAmountToSpawn);
    public virtual void Spawn ( )
    {
        if (hasSpawnLimit && spawnedAmount >= spawnLimit)
        {
            Debug.Log("arrived to spawn limit");
            OnGetSpawnLimit?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (spawnAble)
        {
            for (int i = 0; i <= amountToSpawn; i++)
            {
                GameObject gO = objectPool.RequestGameObject();
                OnSpawn?.Invoke(this, EventArgs.Empty);
                spawnedAmount += 1;

                Vector3 randomSpawnPosition = transform.position - UnityEngine.Random.onUnitSphere * spawnRadius;
                randomSpawnPosition.y = 0;
                randomSpawnPosition.x += spawnOffset;
                randomSpawnPosition.z += spawnOffset;
                gO.transform.position = randomSpawnPosition;
            }
            spawnAble = false;
            StartCoroutine(SpawnDelay(spawnDelay));
        }
    }
    private int SetRandomAmountToSpawn ( int min, int max )
    {
        if (hasRandomAmountToSpawn)
        {
            min = Mathf.Clamp(min, 1, max);
            return UnityEngine.Random.Range(min, max + 1);
        }
        else
        {
            return 1;
        }
    }
    public IEnumerator SpawnDelay ( float time )
    {
        yield return new WaitForSeconds(spawnDelay);
        spawnAble = true;
    }
}