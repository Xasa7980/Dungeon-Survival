using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<RuntimeDungeonEditor_RoomCell> cells;
    public Vector2 size { get; private set; }

    List<GameObject> parts = new List<GameObject>();

    public static Room CreateInstance (Vector2 size, List<GameObject> cells)
    {
        GameObject parentGO = new GameObject("Room_" + Random.Range(0, 100));
        Room room = parentGO.AddComponent<Room>();

        room.size = size;
        room.cells = new List<RuntimeDungeonEditor_RoomCell>();

        parentGO.transform.position = cells[0].transform.position + new Vector3(size.x, 0, size.y) / 2;

        List<GameObject> deleteCells = new List<GameObject>();
        List<Room> affectedRooms = new List<Room>();

        foreach (GameObject g in cells)
        {
            Collider[] colls = Physics.OverlapBox(g.transform.position, new Vector3(1f, 0.5f, 1f), Quaternion.identity);
            
            bool hasCell = false;
            
            foreach (Collider coll in colls)
            {
                if (coll.TryGetComponent(out RuntimeDungeonEditor_RoomCell c))
                {
                    if (!affectedRooms.Contains(c.room))
                        affectedRooms.Add(c.room);

                    c.room.cells.Remove(c);
                    c.room = room;
                    hasCell = true;

                    c.transform.parent = room.transform;
                    room.cells.Add(c);

                    deleteCells.Add(g);
                    break;
                }
            }

            if (!hasCell)
            {
                RuntimeDungeonEditor_RoomCell cell = g.AddComponent<RuntimeDungeonEditor_RoomCell>();
                cell.room = room;
                g.AddComponent<BoxCollider>();

                g.transform.parent = room.transform;

                room.cells.Add(cell);

                Destroy(g.GetComponent<Renderer>());
                Destroy(g.GetComponent<MeshFilter>());
            }
        }

        foreach(GameObject g in deleteCells)
        {
            Destroy(g);
        }

        room.Draw();
        foreach (Room r in affectedRooms)
        {
            r.Draw();
        }

        return room;
    }

    void Draw()
    {
        ClearParts();

        foreach (RuntimeDungeonEditor_RoomCell c in cells)
        {
            GameObject floor = Instantiate(RuntimeDungeonEditor.current.library.GetFloor(), c.transform.position + new Vector3(2.5f,0,-2.5f), Quaternion.identity);
            floor.transform.parent = c.transform;

            for (CellDirections direction = CellDirections.N; direction <= CellDirections.W; direction++)
            {
                Vector3 dir = direction.AsVector3() * 5;

                Collider[] colls = Physics.OverlapBox(c.transform.position + dir, new Vector3(1f, 0.5f, 1f));
                RuntimeDungeonEditor_RoomCell sideCell = null;

                foreach (Collider coll in colls)
                {
                    sideCell = coll.GetComponent<RuntimeDungeonEditor_RoomCell>();

                    if (sideCell) break;
                }

                if (!sideCell)
                {
                    Vector3 position = c.transform.position + direction.AsVector3() * 2.5f;
                    Quaternion rotation = Quaternion.LookRotation(-direction.AsVector3());

                    GameObject borderWall = Object.Instantiate(RuntimeDungeonEditor.current.library.GetBorderWall(),
                        position, rotation);

                    borderWall.transform.position += borderWall.transform.right * 2.5f;
                    borderWall.transform.position += borderWall.transform.forward * 0.1f;

                    borderWall.transform.parent = c.transform;

                    parts.Add(borderWall);
                }
                else if (sideCell.room != c.room)
                {
                    Vector3 position = c.transform.position + direction.AsVector3() * 2.5f;
                    Quaternion rotation = Quaternion.LookRotation(-direction.AsVector3());

                    GameObject borderWall = Object.Instantiate(RuntimeDungeonEditor.current.library.GetBorderWall(),
                        position, rotation);

                    borderWall.transform.position += borderWall.transform.right * 2.5f;
                    borderWall.transform.position += borderWall.transform.forward * 0.2f;

                    borderWall.transform.parent = c.transform;

                    parts.Add(borderWall);
                }
            }
        }
    }

    void ClearParts()
    {
        foreach (GameObject p in parts)
        {
            GameObject.Destroy(p);
        }
        parts.Clear();
    }

    public void Expand(List<GameObject> cells)
    {
        List<Room> affectedRooms = new List<Room>();
        List<GameObject> deleteCells = new List<GameObject>();

        foreach (GameObject g in cells)
        {
            g.transform.parent = transform;

            bool hasCell = false;
            Collider[] colls = Physics.OverlapBox(g.transform.position, new Vector3(2f, 0.5f, 2f), Quaternion.identity);
            foreach (Collider coll in colls)
            {
                if (coll.TryGetComponent(out RuntimeDungeonEditor_RoomCell c))
                {
                    hasCell = true;
                    deleteCells.Add(g);

                    if (!affectedRooms.Contains(c.room) && c.room != this)
                        affectedRooms.Add(c.room);

                    if (c.room != this)
                    {
                        c.room.cells.Remove(c);
                        //GameObject.Destroy(c.gameObject);
                        c.room = this;
                        c.room.cells.Add(c);
                        c.transform.parent = transform;
                    }

                    break;
                }
            }

            if (!hasCell)
            {
                RuntimeDungeonEditor_RoomCell cell = g.AddComponent<RuntimeDungeonEditor_RoomCell>();
                cell.room = this;
                g.AddComponent<BoxCollider>();

                this.cells.Add(cell);

                Destroy(g.GetComponent<Renderer>());
                Destroy(g.GetComponent<MeshFilter>());
            }
        }

        foreach(GameObject g in deleteCells)
        {
            Destroy(g);
        }

        Draw();
        foreach(Room r in affectedRooms)
        {
            r.Draw();
        }
    }
}
