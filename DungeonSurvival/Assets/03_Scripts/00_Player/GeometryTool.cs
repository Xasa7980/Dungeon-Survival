using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GeometryTool
{
    public enum CenterType { Geometrical, CenterOfMass}

    /*
    void Start()
    {
        GeometryTools.CreatePresentationModel(pieces, container);
        filters = new List<MeshFilter>(GetFilters(container.gameObject));
        geometricalCenter = GeometryTools.GetCenterOf(filters.ToArray(), container, CenterType.Geometrical);
        float radius = GeometryTools.GetMinRotationRadius(filters.ToArray(), container);
        Vector3 viewDir = (viewPoint.position - container.position).normalized;
        viewPoint.position = container.position + viewDir * (radius + 2);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            container.RotateAround(geometricalCenter, viewPoint.up, -Input.GetAxis("Mouse X") * rotationy * Time.deltaTime);
            container.RotateAround(geometricalCenter, viewPoint.right, Input.GetAxis("Mouse Y") * rotationx * Time.deltaTime);
            container.RotateAround(geometricalCenter, viewPoint.forward, rotationz * Time.deltaTime);
        }
    }*/

    public static void CreatePresentationModel(Item[] pieces, Transform container)
    {
        for (int i =0; i < container.childCount; i++)
        {
            GameObject.Destroy(container.GetChild(0).gameObject);
        }

        foreach(Item piece in pieces)
        {
            GameObject pieceGO = GameObject.Instantiate(piece.visualizationModel, container);
        }
    }

    public static GameObject CreateInGameModel( Item[] pieces, Transform container)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            GameObject.Destroy(container.GetChild(0).gameObject);
        }

        foreach (Item piece in pieces)
        {
            GameObject pieceGO = GameObject.Instantiate(piece.interactableModel.gameObject, container);
        }

        return container.gameObject;
    }

    public static GameObject CreateInGameModel( Item[] pieces)
    {
        Transform container = new GameObject("Item").transform;

        foreach (Item piece in pieces)
        {
            GameObject pieceGO = GameObject.Instantiate(piece.interactableModel.gameObject, container);
        }

        return container.gameObject;
    }

    public static MeshFilter[] GetFilters (GameObject container)
    {
        return container.GetComponentsInChildren<MeshFilter>();
    }

    public static Vector3 GetCenterOf(MeshFilter[] filters, Transform container, CenterType centerType)
    {
        int vertexCount = 0;

        Vector3 allVertices = Vector3.zero;

        float maxX = float.MinValue;
        float maxY = float.MinValue;
        float maxZ = float.MinValue;

        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float minZ = float.MaxValue;

        foreach (MeshFilter filter in filters)
        {
            if (filter == null) continue;

            Vector3[] vertices = filter.mesh.vertices;
            vertexCount += vertices.Length;

            foreach (Vector3 v in vertices)
            {
                Vector3 wPoint = filter.transform.TransformPoint(v) - container.position;

                allVertices += wPoint;

                if (wPoint.x > maxX) maxX = wPoint.x;
                if (wPoint.y > maxY) maxY = wPoint.y;
                if (wPoint.z > maxZ) maxZ = wPoint.z;

                if (wPoint.x < minX) minX = wPoint.x;
                if (wPoint.y < minY) minY = wPoint.y;
                if (wPoint.z < minZ) minZ = wPoint.z;
            }
        }

        Vector3 min = new Vector3(minX, minY, minZ);
        Vector3 max = new Vector3(maxX, maxY, maxZ);

        if (centerType == CenterType.Geometrical)
            return container.TransformPoint(Vector3.Lerp(max, min, 0.5f));
        else
            return container.TransformPoint(allVertices / vertexCount);
    }

    public static Vector3 GetCenterOf(MeshFilter filter, Transform container, CenterType centerType)
    {
        int vertexCount = 0;

        Vector3 allVertices = Vector3.zero;

        float maxX = float.MinValue;
        float maxY = float.MinValue;
        float maxZ = float.MinValue;

        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float minZ = float.MaxValue;

        Vector3[] vertices = filter.mesh.vertices;
        vertexCount += vertices.Length;

        foreach (Vector3 v in vertices)
        {
            Vector3 wPoint = v + filter.transform.localPosition;

            allVertices += wPoint;

            if (wPoint.x > maxX) maxX = wPoint.x;
            if (wPoint.y > maxY) maxY = wPoint.y;
            if (wPoint.z > maxZ) maxZ = wPoint.z;

            if (wPoint.x < minX) minX = wPoint.x;
            if (wPoint.y < minY) minY = wPoint.y;
            if (wPoint.z < minZ) minZ = wPoint.z;
        }

        Vector3 min = new Vector3(minX, minY, minZ);
        Vector3 max = new Vector3(maxX, maxY, maxZ);

        if (centerType == CenterType.Geometrical)
            return container.TransformPoint(Vector3.Lerp(max, min, 0.5f));
        else
            return container.TransformPoint(allVertices / vertexCount);
    }

    public static float GetMinRotationRadius(MeshFilter[] filters)
    {
        int vertexCount = 0;

        Vector3 allVertices = Vector3.zero;

        Vector3 min = Vector3.zero;
        Vector3 max = Vector3.zero;

        foreach (MeshFilter filter in filters)
        {
            if (filter == null) continue;

            Vector3[] vertices = filter.mesh.vertices;
            vertexCount += vertices.Length;

            foreach (Vector3 v in vertices)
            {
                Vector3 wPoint = v + filter.transform.localPosition;

                allVertices += wPoint;

                max = Vector3.Max(max, wPoint);
                min = Vector3.Min(min, wPoint);
            }
        }


        return Vector3.Distance(min, max) / 2;
    }
}