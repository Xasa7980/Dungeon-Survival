using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomWall : MonoBehaviour
{
    public float amountToCreate;
    public GameObject[] walls;
    public float[] rating;
    private void OnEnable()
    {
        GetWall();
    }
    void GetWall()//Creo gameObject , lo meto en lista los hijos a instanciar.
    {
        GameObject obj = new GameObject();
        Transform spawner = transform.GetChild(0);
        for (int i = 0; i < amountToCreate; i++)
        {
            List<GameObject> gOs = new();
            gOs.Add(Instantiate(walls[(int)RarityRating()], Vector3.zero, spawner.transform.rotation, obj.transform));
            foreach (var item in gOs)
            {
                item.transform.position = spawner.position + transform.right * i * 5;
                item.SetActive(true);
            }
        }
    }
    float RarityRating()
    {
        float total = 0;
        foreach (var item in rating)
        {
            total += item;
        }
        float randNum = Random.Range(0,total);

        for (int i = 0; i < rating.Length; i++)
        {
            if(randNum < rating[i])
            {
                return i;
            }
            else
            {
                randNum -= rating[i];
            }
        }
        return rating.Length - 1;

    }
}
