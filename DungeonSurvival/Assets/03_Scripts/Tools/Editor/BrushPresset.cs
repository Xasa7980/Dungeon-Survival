using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushPresset : ScriptableObject
{
    [HideInInspector][SerializeField]
    public List<GameObject> objects;
}
