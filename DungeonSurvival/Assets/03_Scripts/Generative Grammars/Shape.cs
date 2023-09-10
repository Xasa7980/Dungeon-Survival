using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown.LevelDesign.Generation.Grammar
{
    public class Shape : MonoBehaviour
    {
        public Symbol symbol { get; private set; }
        [SerializeField] int repeat = 1;
        public Connection entrance;
        [SerializeField] List<Connection> connections;

        public int depth = 1;

        [SerializeField] bool _critical;
        public bool critical => _critical;

        public bool isChild { get; private set; }

        public Shape CreateInstance(Symbol symbol)
        {
            Shape instance = Instantiate(this);
            instance.symbol = symbol;
            return instance;
        }

        public void CreateSubinstances(ref List<Vector3Int> map)
        {
            List<Shape> subShapes = new List<Shape>();
            List<Connection> newConnections = new List<Connection>(connections);

            for (int i = 1; i < repeat; i++)
            {
                Shape subInstance = Instantiate(this);
                subInstance.isChild = true;
                Connection[] freeConnections = GetFreeConnections(newConnections.ToArray(), map);
                Connection connection = freeConnections[Random.Range(0, freeConnections.Length)];
                //newConnections.Remove(connection);

                subInstance.symbol = this.symbol;
                connection.Connect(subInstance);
                subInstance.entrance.Connect(this);

                Vector3Int position = Vector3Int.RoundToInt(connection.anchorPosition + connection.direction * 3);
                subInstance.transform.rotation = Quaternion.FromToRotation(Vector3.right, connection.direction);
                //subInstance.transform.parent = this.transform;
                subInstance.transform.position = position;
                map.Add(position);

                newConnections.AddRange(subInstance.connections);
                //Destroy(connection.anchor.gameObject);
                //Destroy(subInstance);
                subInstance.connections.Clear();
                subShapes.Add(subInstance);
            }

            connections = new List<Connection>(newConnections);
            subShapes.ForEach(s => s.transform.parent = this.transform);
        }

        public Shape Extend (Shape extensionShape, ref List<Vector3Int> map)
        {
            List<Connection> newConnections = new List<Connection>(connections);

            Shape extension = Instantiate(extensionShape);
            extension.isChild = true;
            Connection[] freeConnections = newConnections.Where(c => c.free).ToArray();
            Connection connection = freeConnections[Random.Range(0, freeConnections.Length)];
            //newConnections.Remove(connection);

            extension.symbol = this.symbol;
            connection.Connect(extension);
            extension.entrance.Connect(this);

            Vector3Int position = Vector3Int.RoundToInt(connection.anchorPosition + connection.direction * 3);
            extension.transform.rotation = Quaternion.FromToRotation(Vector3.right, connection.direction);
            //subInstance.transform.parent = this.transform;
            extension.transform.position = position;
            map.Add(position);

            newConnections.AddRange(extension.connections);
            //Destroy(connection.anchor.gameObject);
            //Destroy(subInstance);
            extension.connections.Clear();

            return extension;

            //subInstance.transform.parent = this.transform;
        }

        public Connection GetFreeConnection()
        {
            Connection[] freeConnections = connections.Where(c => c.free).ToArray();
            if (freeConnections.Length > 0)
                return freeConnections[Random.Range(0, freeConnections.Length)];
            else
                return null;
        }

        public Connection GetFreeConnection(List<Vector3Int> map, int linearity = 0)
        {
            List<Connection> freeConnections = new List<Connection>();
            foreach(Connection c in connections)
            {
                if (!c.free) continue;

                if (map.Contains(Vector3Int.RoundToInt(c.anchorPosition + c.direction * 3))) continue;

                if (!ValidateSurroundings(Vector3Int.RoundToInt(c.anchorPosition + c.direction * 3), map, linearity))
                    continue;

                freeConnections.Add(c);
            }
            
            if (freeConnections.Count > 0)
                return freeConnections[Random.Range(0, freeConnections.Count)];
            else
                return null;
        }

        public Connection GetFreeConnection(Connection[] connections, List<Vector3Int> map, int linearity = 0)
        {
            List<Connection> freeConnections = new List<Connection>();
            foreach (Connection c in connections)
            {
                if (!c.free) continue;

                if (map.Contains(Vector3Int.RoundToInt(c.anchorPosition + c.direction * 3))) continue;

                if (!ValidateSurroundings(Vector3Int.RoundToInt(c.anchorPosition + c.direction * 3), map, linearity))
                    continue;

                freeConnections.Add(c);
            }

            if (freeConnections.Count > 0)
                return freeConnections[Random.Range(0, freeConnections.Count)];
            else
                return null;
        }

        public Connection[] GetFreeConnections(List<Vector3Int> map, int linearity = 0)
        {
            List<Connection> freeConnections = new List<Connection>();
            foreach (Connection c in connections)
            {
                if (!c.free) continue;

                if (map.Contains(Vector3Int.RoundToInt(c.anchorPosition + c.direction * 3))) continue;

                if (!ValidateSurroundings(Vector3Int.RoundToInt(c.anchorPosition + c.direction * 3), map, linearity))
                    continue;

                freeConnections.Add(c);
            }

            if (freeConnections.Count > 0)
                return freeConnections.ToArray();
            else
                return null;
        }

        public Connection[] GetFreeConnections(Connection[] connections, List<Vector3Int> map, int linearity = 0)
        {
            List<Connection> freeConnections = new List<Connection>();
            foreach (Connection c in connections)
            {
                if (!c.free) continue;

                if (map.Contains(Vector3Int.RoundToInt(c.anchorPosition + c.direction * 3))) continue;

                if (!ValidateSurroundings(Vector3Int.RoundToInt(c.anchorPosition + c.direction * 3), map, linearity))
                    continue;

                freeConnections.Add(c);
            }

            if (freeConnections.Count > 0)
                return freeConnections.ToArray();
            else
                return null;
        }

        public Connection[] GetFreeConnections() => connections.Where(c => c.free).ToArray();

        private void OnDrawGizmos()
        {
            foreach(Connection c in connections)
                c.DrawGizmos();

            //Gizmos.color = Color.red;
            //Gizmos.DrawLine(transform.position, symbol.transform.position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            foreach (Connection c in connections)
            {
                Gizmos.DrawSphere(c.anchorPosition, 1);
            }
        }

        bool ValidateSurroundings(Vector3 position, List<Vector3Int> map, int linearity = 0)
        {
            int freeCount = 0;

            if (!map.Contains(Vector3Int.RoundToInt(position + Vector3.right * 6)))
                freeCount++;

            if (!map.Contains(Vector3Int.RoundToInt(position - Vector3.right * 6)))
                freeCount++;

            if (!map.Contains(Vector3Int.RoundToInt(position + Vector3.up * 6)))
                freeCount++;

            if (!map.Contains(Vector3Int.RoundToInt(position + Vector3.up * 6)))
                freeCount++;

            if (freeCount > linearity)
                return true;
            else
                return false;
        }

        public bool ValidateNeigboring(List<Vector3Int> map, int safeThreshold = 0)
        {
            Connection[] freeConnections = GetFreeConnections(map);
            if (freeConnections == null) return false;

            return freeConnections.Length >= symbol.childs.Count + safeThreshold;
        }
    }

    [System.Serializable]
    public class Connection
    {
        public Transform anchor;

        public Vector3 anchorPosition => anchor.position;
        public Quaternion anchorRotation => anchor.rotation;

        public Vector3 direction => anchor.right;

        public Shape child { get; private set; }
        public bool free => !child;

        public void Connect(Shape shape)
        {
            child = shape;
        }

        public void DrawGizmos()
        {
            if(child != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(anchorPosition, child.transform.position);
            }
        }

        public Shape GetNeighbour(Dictionary<Vector3, Shape> map)
        {
            Vector3 position = anchorPosition + direction * 3;
            if (map.ContainsKey(position))
                return map[position];

            return null;
        }
    }
}