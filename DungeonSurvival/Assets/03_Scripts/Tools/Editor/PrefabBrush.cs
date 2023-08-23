using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class PrefabBrush : EditorWindow
{
    public static bool drawBrush;
    public static bool painting;
    static PrefabBrush window;

    Vector2 scroll;

    AnimationCurve paintCurve = new AnimationCurve()
    {
        keys = new Keyframe[]
        {
            new Keyframe(0, 0),
            new Keyframe(1, 1)
        }
    };

    [SerializeField] int density = 12;
    [SerializeField] float radius = 6;
    Vector3 center;
    Vector3 lastCenter;

    List<GameObject> objects = new List<GameObject>();
    List<GameObject> draggedObjects = new List<GameObject>();
    bool addObjectsFromProject = false;
    Texture2D[] previews;


    [MenuItem("Dungeon Tools/Prefab Brush")]
    static void Init()
    {
        window = (PrefabBrush)EditorWindow.GetWindow(typeof(PrefabBrush), false);
        window.Show();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui -= DrawBrush;
        SceneView.duringSceneGui += DrawBrush;
    }

    private void OnDisable()
    {
        drawBrush = false;
        painting = false;
        Tools.hidden = false;

        SceneView.duringSceneGui -= DrawBrush;
    }

    private void OnGUI()
    {
        Event e = Event.current;

        #region Drag Detection
        if (e.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            e.Use();
        }
        else if (e.type == EventType.DragPerform)
        {
            DragAndDrop.AcceptDrag();

            if (DragAndDrop.paths.Length == DragAndDrop.objectReferences.Length)
            {
                draggedObjects.Clear();
                draggedObjects = DragAndDrop.objectReferences.Where(obj => obj is GameObject).Cast<GameObject>().ToList();
                addObjectsFromProject = true;
            }
        }
        #endregion

        if (GUILayout.Button(drawBrush ? "Stop Painting" : "Start Painting", GUILayout.Height(40)) &&
            objects.Count > 0)
        {
            drawBrush = !drawBrush;

            Tools.hidden = drawBrush;

            if (drawBrush)
            {
                SceneView.duringSceneGui -= TryPaint;
                SceneView.duringSceneGui += TryPaint;
            }
            else
            {
                SceneView.duringSceneGui -= TryPaint;
            }
        }

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Presset", GUILayout.Height(30)))
        {

        }

        if (GUILayout.Button("Clear Brush Assets", GUILayout.Height(30)))
        {
            objects.Clear();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        density = EditorGUILayout.IntField("Density", density);
        radius = EditorGUILayout.FloatField("Radius", radius);

        scroll = GUILayout.BeginScrollView(scroll);
        Rect dropArea = new Rect(5, 20, position.width - 10, position.height - 50);
        GUILayout.BeginArea(dropArea, "", "box");

        if(addObjectsFromProject && dropArea.Contains(e.mousePosition))
        {
            objects.AddRange(draggedObjects);
            draggedObjects.Clear();
            addObjectsFromProject = false;
            UpdateBrushPreview();
        }

        int columnCount = Mathf.FloorToInt(position.width / 40) - 1;
        int count = objects.Count;

        GUILayout.BeginVertical();
        while (count > 0)
        {
            GUILayout.BeginHorizontal();
            for(int i = 0; i < columnCount; i++)
            {
                if(GUILayout.Button(previews[previews.Length - count], GUILayout.Width(40),GUILayout.Height(40)))
                {
                    GameObject asset = objects[objects.Count - count];

                    if (e.button == 1)
                    {
                        GenericMenu menu = new GenericMenu();

                        menu.AddItem(new GUIContent("Inspect Asset"), false,
                                () => PopUpAssetInspector.Create(asset));

                        menu.AddItem(new GUIContent("Select Prefab"), false,
                                () => SelectPrefabInProject(asset));

                        menu.ShowAsContext();
                    }
                }

                count--;

                if (count == 0)
                    break;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        GUILayout.EndArea();

        GUILayout.EndScrollView();
    }

    public void Paint()
    {
        if ((lastCenter - center).sqrMagnitude > 1f)
        {
            lastCenter = center;

            for(int i = 0; i < density; i++)
            {
                Vector2 point = Random.insideUnitCircle;
                Vector3 position = center + new Vector3(point.x, 0, point.y) * radius;

                GameObject prefab = PrefabUtility.InstantiatePrefab(objects[Random.Range(0, objects.Count)]) as GameObject;
                prefab.transform.position = position;
                prefab.transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));

                Undo.RegisterCreatedObjectUndo(prefab, "Prefabs Created");
            }

            Repaint();
        }
    }

    void TryPaint(SceneView sceneView)
    {
        Event e = Event.current;

        // Retrieve the control Id
        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        // Start treating your events
        switch (e.type)
        {
            case EventType.MouseDown:
                if(e.button == 0)
                {
                    GUIUtility.hotControl = controlId;
                    painting = true;
                }

                if(e.button == 1)
                {
                    drawBrush = false;
                    Tools.hidden = false;
                    SceneView.duringSceneGui -= TryPaint;
                }

                Repaint();
                e.Use();
                break;

            case EventType.MouseUp:
                if (e.button == 0)
                {
                    painting = false;
                }

                Repaint();
                e.Use();
                break;
        }

        if (painting)
        {
            Paint();
        }
    }

    void DrawBrush(SceneView sceneView)
    {
        if (!drawBrush) return;

        Plane placingPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float rayDst;

        if (placingPlane.Raycast(ray, out rayDst))
        {
            center = ray.GetPoint(rayDst);
            Color color = Color.magenta;
            
            Handles.color = color;
            Handles.DrawWireArc(center, Vector3.up, Vector3.forward, 360, radius);

            for (int i = 0; i <= 400; i++)
            {
                float percent = i / 400f;
                float value = paintCurve.Evaluate(percent);
                value = Mathf.Clamp01(value);
                color.a = value * 0.25f;
                Handles.color = color;
                Handles.DrawWireArc(center, Vector3.up, Vector3.forward, 360, (1 - percent) * radius);
                //UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, Vector3.forward, 360, radius);
            }
        }
        else
        {
            center = Vector3.zero;
        }

        sceneView.Repaint();
    }

    void UpdateBrushPreview()
    {
        previews = new Texture2D[objects.Count];
        for(int i = 0;i < objects.Count; i++)
        {
            previews[i] = GetAssetPreview(objects[i]);
        }
    }

    Texture2D GetAssetPreview(GameObject asset)
    {
        var editor = Editor.CreateEditor(asset);
        Texture2D tex = editor.RenderStaticPreview(AssetDatabase.GetAssetPath(asset), null, 90, 90);
        DestroyImmediate(editor);
        return tex;
    }

    void SelectPrefabInProject(GameObject instance)
    {
        Selection.activeGameObject = instance;
        EditorGUIUtility.PingObject(instance);
    }
}
