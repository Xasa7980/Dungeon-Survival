using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuntimeDungeonEditor : MonoBehaviour
{
    public static RuntimeDungeonEditor current { get; private set; }

    public DungeonLibrary library;

    float gridSize = 5;

    [SerializeField] Vector3 mouseGridPosition;
    [SerializeField] GameObject pointerGhost;

    [SerializeField] bool dragging = false;

    List<GameObject> dragGrid = new List<GameObject>();
    Vector3 startDragPosition;
    [SerializeField] Vector2 dragGridSize;

    List<Room> rooms = new List<Room>();

    Room currentRoom = null;

    private void Start()
    {
        current = this;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDst;
        if (groundPlane.Raycast(ray, out rayDst))
        {
            Vector3 point = ray.GetPoint(rayDst);
            float pointX = Mathf.Round(point.x / gridSize) * gridSize;
            float pointZ = Mathf.Round(point.z / gridSize) * gridSize;

            mouseGridPosition = new Vector3(pointX, 0, pointZ);
            pointerGhost.transform.position = mouseGridPosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startDragPosition = mouseGridPosition;

            Ray cRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(cRay,out RaycastHit hit))
            {
                if(hit.collider.TryGetComponent<RuntimeDungeonEditor_RoomCell>(out RuntimeDungeonEditor_RoomCell cell))
                {
                    currentRoom = cell.room;
                }
                else
                {
                    currentRoom = null;
                }
            }
            else
            {
                currentRoom = null;
            }
        }

        if (Input.GetMouseButton(0) && !dragging)
        {
            if ((startDragPosition - mouseGridPosition).sqrMagnitude >= Mathf.Pow(gridSize, 2))
            {
                dragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                dragging = false;
                if (currentRoom == null)
                    rooms.Add(Room.CreateInstance(dragGridSize, dragGrid));
                else
                    currentRoom.Expand(dragGrid);

                ClearGrid(false);
                dragGridSize = Vector2.zero;
            }
        }

        pointerGhost.SetActive(!dragging);

        if (dragging)
        {
            float gridX = (mouseGridPosition.x - startDragPosition.x) / gridSize;
            float gridY = (mouseGridPosition.z - startDragPosition.z) / gridSize;

            Vector2 grid = new Vector2(gridX, gridY);

            if (dragGridSize != grid)
            {
                dragGridSize = grid;

                ClearGrid();

                for (int i = 0; i <= Mathf.Abs(gridX); i++)
                {
                    for(int j = 0; j <= Mathf.Abs(gridY); j++)
                    {
                        Vector3 position = startDragPosition + Vector3.right * (Mathf.Sign(gridX) * (i * gridSize)) +
                            Vector3.forward * (Mathf.Sign(gridY) * (j * gridSize));

                        GameObject cell = Instantiate(pointerGhost, position, Quaternion.identity);
                        cell.SetActive(true);
                        dragGrid.Add(cell);
                    }
                }
            }
        }
    }

    void ClearGrid(bool destroyObjects = true)
    {
        if (destroyObjects)
        {
            foreach (GameObject g in dragGrid)
            {
                Destroy(g);
            }
        }
        dragGrid.Clear();
    }

    Vector3 GetGridPosition(Vector3 position)
    {
        float pointX = Mathf.Round(position.x / gridSize) * gridSize;
        float pointZ = Mathf.Round(position.z / gridSize) * gridSize;

        return new Vector3(pointX, 0, pointZ);
    }
}

