using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabPool;
    [SerializeField] private int poolSize = 10;
    private List<GameObject> _poolingObjectsList = new List<GameObject>();
    public List<GameObject> poolingObjectsList {  get { return _poolingObjectsList; } }
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
            GameObject newObject = Instantiate(prefabPool, this.transform);
            _poolingObjectsList.Add( newObject );
            newObject.SetActive(false);
        }
    }
    public GameObject RequestGameObject ( )
    {
        foreach (var pooledObject in _poolingObjectsList)
        {
            if (!pooledObject.activeSelf)
            {
                pooledObject.SetActive(true);
                return pooledObject;
            }
        }
        AddObjectsToPool(1);
        GameObject newPooledObject = _poolingObjectsList[_poolingObjectsList.Count - 1];
        newPooledObject.SetActive(true);
        return newPooledObject;
    }
}
