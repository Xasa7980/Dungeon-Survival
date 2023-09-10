using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown.LevelDesign.Generation.Grammar
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] Symbol entrance;

        private void Start()
        {
            GetComponent<ShapeGrammar>().Generate(entrance);
        }
    }
}