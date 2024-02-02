using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabPool;
    [SerializeField] private int poolSize = 10;
    private List<GameObject> poolingObjectsList = new List<GameObject>();
    public List<GameObject> PoolingObjectsList {  get { return poolingObjectsList; } }
    private void Start ( )
    {
        if (prefabPool != null)
        {
            AddObjectsToPool(poolSize);
        }
        else
        {
            Debug.LogError("PrefabPool is null.");
        }
    }
    private void AddObjectsToPool(int amount )
    {
        for (int i = 0; i < amount; i++)
        {
            if (prefabPool == null) return;

            GameObject newObject = Instantiate(prefabPool, this.transform);
            poolingObjectsList.Add( newObject );
        }
    }
    public GameObject RequestGameObject ( )
    {
        foreach (var pooledObject in poolingObjectsList)
        {
            if (!pooledObject.activeSelf)
            {
                pooledObject.SetActive(true);
                return pooledObject;
            }
        }
        AddObjectsToPool(1);
        GameObject newPooledObject = poolingObjectsList[poolingObjectsList.Count - 1];
        newPooledObject.SetActive(true);
        return newPooledObject;
    }
}
