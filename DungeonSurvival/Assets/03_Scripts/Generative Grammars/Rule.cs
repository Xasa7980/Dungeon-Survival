using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown.LevelDesign.Generation.Grammar
{
    public class Rule : MonoBehaviour
    {
        public Symbol startSymbol;
        public Symbol endSymbol;

        [Space(10)]

        public Symbol[] order;
    }
}
