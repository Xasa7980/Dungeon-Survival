using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponPropertyBuilder : EditorWindow
{
    public GameObject childObject; // Objeto hijo
    public GameObject firstChildObject; // Primer objeto hijo
    public float childHeight = 1.0f; // Altura para el objeto hijo
    public List<Item> items = new List<Item>(); // Lista de Items para asignar a cada objeto seleccionado

    private Vector2 scrollPos;
    private List<Object> selectedObjects = new List<Object>();
    private bool showSelectedObjects = true;

    [MenuItem("Tools/WeaponPropertyBuilder")]
    public static void ShowWindow ( )
    {
        EditorWindow.GetWindow<WeaponPropertyBuilder>("WeaponPropertyBuilder");
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

        childObject = (GameObject)EditorGUILayout.ObjectField("Child Object", childObject, typeof(GameObject), true);
        firstChildObject = (GameObject)EditorGUILayout.ObjectField("First Child Object", firstChildObject, typeof(GameObject), true);
        childHeight = EditorGUILayout.FloatField("Height for Child Object", childHeight);
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty itemsProperty = so.FindProperty("items");
        EditorGUILayout.PropertyField(itemsProperty, true); // Allows array editing in Inspector
        so.ApplyModifiedProperties(); // Apply changes to serialized property

        if (GUILayout.Button("Setup All"))
        {
            SetupAll();
        }

        if (GUILayout.Button("Undo Last Action"))
        {
            Undo.PerformUndo();
        }
    }

    void OnSelectionChange ( )
    {
        selectedObjects.Clear();
        selectedObjects.AddRange(Selection.objects);
        Repaint();
    }

    private void SetupAll ( )
    {
        for (int i = 0; i < selectedObjects.Count; i++)
        {
            GameObject go = selectedObjects[i] as GameObject;
            if (go != null && items.Count > i)
            {
                SetupWorldItemComponent(go, items[i]);
                SetupEquipmentDataHolder(go);
                SetupChildGameObject(go);
                SetupCollider(go);
                SetupFirstChildGameObject(go);
                CloneAsFirstChild(go);
            }
        }
    }

    private void SetupWorldItemComponent ( GameObject go, Item item )
    {
        Undo.RecordObject(go, "Modify GameObject");

        WorldItem worldItem = go.GetComponent<WorldItem>();
        if (worldItem == null)
        {
            worldItem = Undo.AddComponent<WorldItem>(go);
        }

        if (item != null)
        {
            worldItem.SetItem(item);
        }
    }

    private void SetupEquipmentDataHolder ( GameObject go )
    {
        EquipmentDataHolder equipmentDataHolder = go.GetComponent<EquipmentDataHolder>();
        if (equipmentDataHolder == null)
        {
            equipmentDataHolder = Undo.AddComponent<EquipmentDataHolder>(go);
        }

        WorldItem worldItem = go.GetComponent<WorldItem>();
        if (worldItem != null && worldItem.item != null)
        {
            equipmentDataHolder.SetWorldItem(worldItem);
            equipmentDataHolder.SetEquipmentItem(worldItem.item);
        }
    }

    private void SetupChildGameObject ( GameObject go )
    {
        GameObject newChild = Instantiate(childObject, go.transform);
        newChild.transform.position = go.transform.position + Vector3.up * childHeight;
        newChild.name = "Canvas";
        go.GetComponent<WorldItem>().SetCanvas( newChild );
        
        Undo.RegisterCreatedObjectUndo(newChild, "Canvas");
    }

    private void SetupFirstChildGameObject ( GameObject go )
    {
        GameObject firstChild = Instantiate(firstChildObject, go.transform.position, Quaternion.identity, go.transform);
        firstChild.transform.SetAsFirstSibling();
        Undo.RegisterCreatedObjectUndo(firstChild, "Create First Child");
    }

    private void CloneAsFirstChild ( GameObject go )
    {
        GameObject clone = Instantiate(go, go.transform.position, Quaternion.identity, go.transform.GetChild(0));
        go.transform.localScale = Vector3.one;
        clone.layer = LayerMask.NameToLayer("Default");
        go.GetComponent<WorldItem>().SetRenderer(clone.GetComponent<Renderer>());
        for (int i = clone.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(clone.transform.GetChild(i).gameObject);
        }
        foreach (Component component in clone.GetComponents<Component>())
        {
            if ((component is WorldItem || component is EquipmentDataHolder || component is AreaDrawer))
            {
                DestroyImmediate(component);
            }
        }
        foreach (Component component in go.GetComponents<Component>())
        {
            if ((component is MeshFilter || component is MeshRenderer))
            {
                DestroyImmediate(component);
            }
        }
        Undo.RegisterCreatedObjectUndo(clone, "Clone Created Child");
    }

    private void SetupCollider ( GameObject go )
    {
        Collider collider = go.GetComponent<Collider>();
        if (collider == null)
        {
            if (go.GetComponent<MeshFilter>() != null)
            {
                MeshCollider meshCollider = Undo.AddComponent<MeshCollider>(go); // Add a collider if none exists
                meshCollider.convex = true;
            }
            else
            {
                collider = Undo.AddComponent<BoxCollider>(go);
            }
        }
        else
        {
            collider.isTrigger = true;
            Undo.RecordObject(collider, "Modify Collider");
        }
    }
}