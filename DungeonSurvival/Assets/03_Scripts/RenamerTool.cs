using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenamerTool : EditorWindow
{
    public float height = 1.5f;
    public GameObject childObject1;
    public GameObject childObject2;

    private Vector2 scrollPos;
    private List<Object> selectedObjects = new List<Object>();
    private bool showSelectedObjects = true;  // Toggle para mostrar/ocultar objetos seleccionados

    [MenuItem("Tools/Renamer Tool")]
    public static void ShowWindow ( )
    {
        EditorWindow.GetWindow<RenamerTool>("Renamer Tool");
    }

    void OnGUI ( )
    {
        GUILayout.Label("Selected Objects", EditorStyles.boldLabel);
        showSelectedObjects = EditorGUILayout.Foldout(showSelectedObjects, "Show/Hide Selected Objects");
        if (showSelectedObjects)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));
            foreach (var selectedObject in selectedObjects)
            {
                EditorGUILayout.ObjectField("Object", selectedObject, typeof(Object), true);
            }
            EditorGUILayout.EndScrollView();
        }

        childObject1 = (GameObject)EditorGUILayout.ObjectField("Child Object 1", childObject1, typeof(GameObject), true);
        childObject2 = (GameObject)EditorGUILayout.ObjectField("Child Object 2", childObject2, typeof(GameObject), true);
        height = EditorGUILayout.FloatField("Height for Second Child", height);

        if (GUILayout.Button("Perform All Actions"))
        {
            PerformAllActions();
        }

        if (GUILayout.Button("Undo Last Action"))
        {
            Undo.PerformUndo();
        }

        if (GUILayout.Button("Remove Mesh Components and Set Layer"))
        {
            RemoveMeshComponentsAndSetLayer();
        }
    }

    void OnSelectionChange ( )
    {
        selectedObjects.Clear();
        selectedObjects.AddRange(Selection.objects);
        Repaint();
    }

    private void PerformAllActions ( )
    {
        AddChildrenToSelectedObjects();
        CloneAsChildOfFirstChild();
        RemoveAllChildrenFromFirstChild();
        RemoveMeshComponentsAndSetLayer(); // Include this function in the all-in-one action if desired
        Debug.Log("Performed all actions on selected objects.");
    }

    private void AddChildrenToSelectedObjects ( )
    {
        foreach (Object selectedObject in selectedObjects)
        {
            if (selectedObject is GameObject)
            {
                GameObject go = selectedObject as GameObject;
                Undo.RecordObject(go, "Add Children");

                GameObject child1 = null;
                if (childObject1 != null)
                {
                    child1 = Instantiate(childObject1, go.transform);
                    child1.transform.localPosition = Vector3.zero;
                    child1.transform.SetAsFirstSibling();
                    Undo.RegisterCreatedObjectUndo(child1, "Create Child 1");
                }

                if (childObject2 != null)
                {
                    GameObject child2 = Instantiate(childObject2, go.transform);
                    child2.transform.localPosition = Vector3.zero + Vector3.up * height;
                    child2.transform.SetSiblingIndex(child1 != null ? 1 : 0);
                    Undo.RegisterCreatedObjectUndo(child2, "Create Child 2");
                }
            }
        }
    }

private void CloneAsChildOfFirstChild()
{
    foreach (Object selectedObject in selectedObjects)
    {
        if (selectedObject is GameObject)
        {
            GameObject go = selectedObject as GameObject;
            if (go.transform.childCount > 0)
            {
                GameObject firstChild = go.transform.GetChild(0).gameObject;
                GameObject secondChild = go.transform.GetChild(1).gameObject;
                GameObject clone = Instantiate(go, firstChild.transform);
                clone.name = $"{go.name}_Clone";
                clone.transform.localPosition = Vector3.zero;
                Undo.RegisterCreatedObjectUndo(clone, "Clone as Child of First Child");

                // Ajustando el Renderer en EnvironmentItem si está disponible
                EnvironmentItem envItem = go.GetComponent<EnvironmentItem>();
                if (envItem != null && clone.GetComponent<Renderer>() != null)
                {
                    envItem.SetRenderer(clone.GetComponent<MeshRenderer>());
                    envItem.SetCanvas(secondChild);
                }
                EnvironmentItem cloneEnvironmentItem = clone.GetComponent<EnvironmentItem>();
                if (cloneEnvironmentItem != null)
                {
                    Undo.DestroyObjectImmediate(cloneEnvironmentItem);
                }
            }
        }
    }
}
    private void RemoveAllChildrenFromFirstChild ( )
    {
        foreach (Object selectedObject in selectedObjects)
        {
            if (selectedObject is GameObject)
            {
                GameObject go = selectedObject as GameObject;
                if (go.transform.childCount > 0)
                {
                    GameObject firstChild = go.transform.GetChild(0).gameObject;
                    if (firstChild.transform.childCount > 0)
                    {
                        GameObject firstGrandchild = firstChild.transform.GetChild(0).gameObject;
                        Undo.RecordObject(firstGrandchild, "Remove All Children from First Grandchild");
                        while (firstGrandchild.transform.childCount > 0)
                        {
                            Transform child = firstGrandchild.transform.GetChild(0);
                            Undo.DestroyObjectImmediate(child.gameObject);
                        }
                    }
                }
            }
        }
    }

    private void RemoveMeshComponentsAndSetLayer ( )
    {
        foreach (Object selectedObject in selectedObjects)
        {
            if (selectedObject is GameObject)
            {
                GameObject go = selectedObject as GameObject;
                Undo.RecordObject(go, "Remove Mesh Components and Set Layer");

                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    Undo.DestroyObjectImmediate(meshFilter);
                }

                Renderer selectedRenderer = go.GetComponent<Renderer>();
                if (selectedRenderer != null)
                {
                    Undo.DestroyObjectImmediate(selectedRenderer);
                }

                go.layer = 6; // Set the layer to 6
            }
        }
        Debug.Log($"Modified {selectedObjects.Count} objects.");
    }
}
