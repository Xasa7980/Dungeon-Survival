using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dungeon Library", menuName = "Dungeon Library")]
public class DungeonLibrary : ScriptableObject
{
    [SerializeField] GameObject[] wallsBorder;
    [SerializeField] GameObject[] floor;

    public GameObject GetBorderWall() => wallsBorder[Random.Range(0, wallsBorder.Length)];

    public GameObject GetFloor() => floor[Random.Range(0, floor.Length)];
}
