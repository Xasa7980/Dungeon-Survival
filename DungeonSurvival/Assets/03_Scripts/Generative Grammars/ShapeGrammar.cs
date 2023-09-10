using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown.LevelDesign.Generation.Grammar {
    public class ShapeGrammar : MonoBehaviour
    {
        [SerializeField] float spawnInterval = 1;

        [Header("Generation")]
        [SerializeField] int seed = 1234567;
        [SerializeField] bool randomSeed = false;
        [SerializeField, Range(0, 2)] int linearity = 0;

        [Header("Branches")]
        [SerializeField] Shape branchShape;
        [SerializeField, Range(0, 1)] float branchingProbability = 0.5f;
        [SerializeField, Min(1)] int branchingDepth = 1;
        [SerializeField, Min(1)] int branchingPasses = 1;

        int _maxDepth = 0;
        int maxDepth
        {
            get => _maxDepth;
            set
            {
                if (value > _maxDepth)
                    _maxDepth = value;
            }
        }
        [SerializeField]
        Gradient depthMapping = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.blue,0),
                new GradientColorKey(Color.red,1)
            },
            alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1,0),
                new GradientAlphaKey(1,1)
            }
        };

        [Space(15)]
        [SerializeField] Sprite wallSprite;

        List<Vector3Int> takenPositions = new List<Vector3Int>();
        List<Vector3> freePositions = new List<Vector3>();
        List<Shape> allShapes = new List<Shape>();
        Dictionary<Vector3, Shape> map = new Dictionary<Vector3, Shape>();
        List<Symbol> unassignedSymbols = new List<Symbol>();

        Vector2 dungeonSize => new Vector2(Mathf.Abs(leftMost) + Mathf.Abs(rightMost), Mathf.Abs(bottomMost) + Mathf.Abs(topMost));
        Vector2 center => new Vector2(leftMost + rightMost, bottomMost + topMost) / 2;
        Vector2 bottomLeftCorner => new Vector2(leftMost, bottomMost);
        float leftMost = 0;
        float rightMost = 0;
        float bottomMost = 0;
        float topMost = 0;

        public void Generate(Symbol entrance)
        {
            StopAllCoroutines();

            if (randomSeed)
                seed = Random.Range(-10000, 10000);

            Random.InitState(seed);

            GetComponent<GrammarGenerator>().Generate();
            StartCoroutine(GenerateRoutine(entrance));
        }

        void ResetDungeon(Symbol entrance)
        {
            foreach (Shape s in allShapes)
            {
                Destroy(s.gameObject);
            }
            allShapes.Clear();
            takenPositions.Clear();
            map.Clear();
            unassignedSymbols = new List<Symbol>(entrance.transform.parent.GetComponentsInChildren<Symbol>(true));
            freePositions.Clear();

            leftMost = 0;
            rightMost = 0;
            bottomMost = 0;
            topMost = 0;
        }

        IEnumerator GenerateMain(Symbol entrance)
        {
            ResetDungeon(entrance);

            Queue<SymbolNode> next = new Queue<SymbolNode>();
            List<Symbol> processedSymbols = new List<Symbol>();

            Symbol currentSymbol = entrance;
            Shape shape = currentSymbol.GetShape();

            shape.transform.position = Vector3.zero;
            shape.transform.rotation = Quaternion.Euler(Vector3.forward * 90 * Random.Range(0, 4));

            ShapeNode currentShapeNode = new ShapeNode(currentSymbol, shape);
            takenPositions.Add(Vector3Int.zero);
            takenPositions.Add(Vector3Int.RoundToInt(shape.entrance.anchorPosition + shape.entrance.direction * 3));

            foreach (Symbol s in currentSymbol.childs)
            {
                if (s == null) continue;

                next.Enqueue(new SymbolNode(s, currentShapeNode));
            }

            allShapes.Add(shape);
            unassignedSymbols.Remove(currentSymbol);

            while (next.Count > 0)
            {
                //Creo y conecto las piezas de la dungeon
                SymbolNode currentNode = next.Dequeue();
                currentSymbol = currentNode.symbol;

                if (!processedSymbols.Contains(currentSymbol))
                    processedSymbols.Add(currentSymbol);
                else
                    continue;

                Connection outConnection = currentNode.parent.shape.GetFreeConnection(takenPositions, linearity);

                if (outConnection != null)
                {
                    unassignedSymbols.Remove(currentSymbol);
                    takenPositions.Add(Vector3Int.RoundToInt(outConnection.anchorPosition + outConnection.direction * 3));
                }
                else
                    continue;

                shape = currentSymbol.GetShape();
                currentShapeNode = new ShapeNode(currentSymbol, shape);

                outConnection.Connect(shape);

                Connection enterConnection = shape.entrance;
                enterConnection.Connect(currentNode.parent.shape);

                //Las coloco en la posicion correcta
                shape.transform.rotation = Quaternion.FromToRotation(Vector3.right, outConnection.direction);
                Vector3Int position = Vector3Int.RoundToInt(outConnection.anchorPosition + outConnection.direction * 3);
                shape.transform.position = position;
                currentSymbol.transform.position = position;
                takenPositions.Add(position);

                shape.depth = currentNode.parent.shape.depth += 1;
                maxDepth = shape.depth;
                shape.CreateSubinstances(ref takenPositions);

                //Agrego los nodos hijos del nodo actual a la lista para asignar sus piezas
                foreach (Symbol s in currentSymbol.childs)
                {
                    if (s == null) continue;

                    if (next.FirstOrDefault(n => n.symbol == s) == null)
                        next.Enqueue(new SymbolNode(s, currentShapeNode));
                }

                //if (!shape.ValidateNeigboring(takenPositions))
                //{
                //    shape.Extend(branchShape, ref takenPositions);
                //}

                allShapes.Add(shape);
                map.Add(position, shape);

                yield return new WaitForSeconds(spawnInterval);
            }
        }

        public IEnumerator GenerateRoutine(Symbol entrance)
        {
            ResetDungeon(entrance);

            while (unassignedSymbols.Count > 0)
            {
                yield return GenerateMain(entrance);
            }

            for (int p = 0; p < branchingPasses; p++)
            {
                Shape[] nonCriticalShapes = allShapes.Where(s => !s.critical).ToArray();
                foreach (Shape s in nonCriticalShapes)
                {
                    Shape currentParent = s;
                    if (Random.value <= branchingProbability)
                    {
                        for (int i = 0; i < branchingDepth - p; i++)
                        {
                            Connection c = currentParent.GetFreeConnection(takenPositions, linearity);
                            if (c == null)
                                break;

                            Shape shape = branchShape.CreateInstance(null);
                            shape.depth = currentParent.depth += 1;
                            maxDepth = shape.depth;
                            Vector3Int position = Vector3Int.RoundToInt(c.anchorPosition + c.direction * 3);
                            takenPositions.Add(position);
                            shape.transform.position = position;
                            shape.transform.rotation = Quaternion.FromToRotation(Vector3.right, c.direction);

                            c.Connect(shape);
                            shape.entrance.Connect(currentParent);

                            map.Add(position, shape);
                            allShapes.Add(shape);

                            currentParent = shape;

                            yield return new WaitForSeconds(spawnInterval);
                        }
                    }
                }
            }

            foreach (Shape s in allShapes)
            {
                Connection[] freeConnections = s.GetFreeConnections();

                if (freeConnections == null) continue;

                foreach (Connection c in freeConnections)
                {
                    if (c == null) continue;

                    SpriteRenderer renderer = c.anchor.gameObject.AddComponent<SpriteRenderer>();

                    if (renderer == null) continue;

                    renderer.sprite = wallSprite;
                    renderer.sortingOrder = 1;
                }

                s.GetComponent<SpriteRenderer>().color = depthMapping.Evaluate((float)s.depth / maxDepth);
            }

            foreach(Vector3Int position in takenPositions)
            {
                if (position.x < leftMost)
                    leftMost = position.x;

                if (position.x > rightMost)
                    rightMost = position.x;

                if (position.y < bottomMost)
                    bottomMost = position.y;

                if (position.y > topMost)
                    topMost = position.y;
            }

            for (int x = 0; x <= dungeonSize.x / 6; x++)
            {
                for (int y = 0; y <= dungeonSize.y / 6; y++)
                {
                    Vector3Int position = new Vector3Int(x * 6, y * 6, 0);
                    if (!takenPositions.Contains(Vector3Int.RoundToInt(bottomLeftCorner.ToVector3() + position)))
                        freePositions.Add(bottomLeftCorner.ToVector3() + position);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach(Vector3 v in freePositions)
            {
                Gizmos.DrawSphere(v, 1);
            }

            Gizmos.color = Color.red;
            foreach(Symbol s in unassignedSymbols)
            {
                Gizmos.DrawSphere(s.transform.position, 1);
            }

#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.green;
            foreach(Shape s in allShapes)
            {
                UnityEditor.Handles.Label(s.transform.position, s.depth.ToString());
            }
#endif
        }

        class SymbolNode
        {
            public readonly Symbol symbol;
            public readonly ShapeNode parent;

            public SymbolNode (Symbol symbol, ShapeNode parent)
            {
                this.symbol = symbol;
                this.parent = parent;
            }
        }

        class RoomNode
        {
            public readonly Vector3 position;
            public readonly bool critical;
            public ConnectionInfo[] connections;
            
            public RoomNode(Vector2 position, bool critical, Connection[] connections)
            {
                this.position = position.ToPlanarVector3();
                this.critical = critical;
                this.connections = new ConnectionInfo[connections.Length];
                for(int c = 0; c < connections.Length; c++)
                {
                    this.connections[c] = new ConnectionInfo(connections[c]);
                }
            }
        }

        class ConnectionInfo
        {
            public readonly bool locked;
            public readonly Vector3 direction;

            public ConnectionInfo(Connection connection)
            {
                if (!connection.free)
                    locked = connection.child.critical;
                else
                    locked = true;

                direction = connection.direction.ToPlanarVector3();
            }
        }
    }

    public class ShapeNode
    {
        public readonly Symbol symbol;
        public readonly Shape shape;

        public ShapeNode(Symbol symbol, Shape shape)
        {
            this.symbol = symbol;
            this.shape = shape;
        }
    }
}
