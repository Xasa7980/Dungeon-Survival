using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabPlacer : EditorWindow
{
    GameObject prefab;
    string categoryName;

    static PrefabPlacer window;

    static float gridSize = 5;
    static float gridHeight = 0;
    static Vector2 offset = Vector2.zero;

    GameObject placeHolder;

    bool ignoreMouseEvents = false;

    public static void StartPlacing(GameObject asset, string categoryName)
    {
        window = (PrefabPlacer)EditorWindow.GetWindow(typeof(PrefabPlacer), false);
        window.categoryName = categoryName;
        window.prefab = asset;
        window.Show();
        window.placeHolder = PrefabUtility.InstantiatePrefab(asset) as GameObject;
        FocusWindowIfItsOpen<SceneView>();
        Tools.hidden = true;
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui -= DrawSceneWindows;
        SceneView.duringSceneGui += DrawSceneWindows;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DrawSceneWindows;
    }

    private void OnGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
        GUI.enabled = true;
        gridSize = EditorGUILayout.FloatField("Grid Size", gridSize);
        offset = EditorGUILayout.Vector2Field("Grid Offset", offset);
        gridHeight = EditorGUILayout.FloatField("Grid Height", gridHeight);

        GUILayout.Space(20);

        GUILayout.BeginVertical();
        EditorGUILayout.LabelField(", - Rotate 90 degrees left");
        EditorGUILayout.LabelField(". - Rotate 90 degrees right");
        GUILayout.EndVertical();
    }

    void DrawSceneWindows(SceneView sceneView)
    {
        if (prefab == null) return;

        Event e = Event.current;

        if ((e.type == EventType.MouseDown && e.button == 1) &&
            (!e.isKey && e.keyCode != KeyCode.LeftAlt))
        {
            Cancel();
            e.Use();
            return;
        }

        Plane placingPlane = new Plane(Vector3.up, Vector3.up * gridHeight);
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float rayDst;
        Vector3 assetPosition = Vector3.zero;

        if (placingPlane.Raycast(ray, out rayDst))
        {
            Vector3 point = ray.GetPoint(rayDst);

            float gridX = Mathf.Round(point.x / gridSize) * gridSize;
            float gridZ = Mathf.Round(point.z / gridSize) * gridSize;
            assetPosition = new Vector3(gridX, point.y, gridZ)
                + placeHolder.transform.right * offset.x + placeHolder.transform.forward * offset.y;
        }

        if ((e.type == EventType.MouseDown && e.button == 0) &&
            (!e.isKey && e.keyCode != KeyCode.LeftAlt))
        {
            Place();
            e.Use();
        }

        if (e.type == EventType.MouseUp && e.button == 0)
        {
            e.Use();
        }

        Handles.color = Color.green;
        Handles.DrawWireCube(assetPosition + Vector3.up * gridSize / 2, Vector3.one * gridSize);

        placeHolder.transform.position = assetPosition;

        if(e.type == EventType.KeyDown && e.keyCode == KeyCode.Comma)
        {
            placeHolder.transform.Rotate(Vector3.up, 90);
            e.Use();
        }

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Period)
        {
            placeHolder.transform.Rotate(Vector3.up, -90);
            e.Use();
        }

        sceneView.Repaint();
    }

    void Cancel()
    {
        prefab = null;
        categoryName = null;
        Tools.hidden = false;
        DestroyImmediate(placeHolder);
    }

    void Place()
    {
        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        instance.transform.position = placeHolder.transform.position;
        instance.transform.rotation = placeHolder.transform.rotation;

        GameObject container = GameObject.Find(categoryName);

        if (container == null)
        {
            container = new GameObject(categoryName);
            Undo.RegisterCreatedObjectUndo(container, "New object placed in scene");
        }

        instance.transform.parent = container.transform;

        Undo.RegisterCreatedObjectUndo(instance, "New object placed in scene");
    }
}
