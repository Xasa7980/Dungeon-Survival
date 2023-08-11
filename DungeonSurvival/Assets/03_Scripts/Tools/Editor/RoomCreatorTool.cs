using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class RoomCreatorTool : EditorWindow
{
    static RoomCreatorTool window;

    Category[] categories;

    Vector2 scroll = Vector2.zero;

    float zoom = 0;
    float minCellWidth = 60;
    float maxCellWidth = 120;
    float minCellHeight = 60;
    float maxCellHeight = 120;

    [MenuItem("Dungeon Tools/Room Editor")]
    static void Init()
    {
        window = (RoomCreatorTool)EditorWindow.GetWindow(typeof(RoomCreatorTool), false);
        window.Show();
        window.LoadAssets();
    }

    void LoadAssets()
    {
        string[] paths = AssetDatabase.GetSubFolders("Assets/!!_Prefabs/04_Environment");
        this.categories = new Category[paths.Length];

        for (int c = 0; c < paths.Length; c++)
        {
            string[] sections = paths[c].Split("/");
            string[] assetsPath = Directory.GetFiles(paths[c], "*.prefab", SearchOption.TopDirectoryOnly);

            List<GameObject> assets = new List<GameObject>();
            List<Texture2D> previews = new List<Texture2D>();
            foreach (string s in assetsPath)
            {
                GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(s);
                assets.Add(asset);
                previews.Add(GetAssetPreview(asset));
            }

            Category category = new Category()
            {
                name = sections[sections.Length - 1],
                assets = assets,
                previews = previews
            };

            this.categories[c] = category;
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;

        EditorGUILayout.BeginHorizontal(GUILayout.Height(30));
        if (GUILayout.Button("Update"))
            LoadAssets();

        GUILayout.FlexibleSpace();

        zoom = GUILayout.HorizontalSlider(zoom, 0, 1, GUILayout.Width(50));

        GUILayout.Space(10);
        EditorGUILayout.EndHorizontal();

        float cellWidth = Mathf.Lerp(minCellWidth, maxCellWidth, zoom);
        float cellHeight = Mathf.Lerp(minCellHeight, maxCellHeight, zoom);
        int columnCount = Mathf.FloorToInt((position.width - cellWidth * 0.75f) / cellWidth);

        scroll = GUILayout.BeginScrollView(scroll);
        foreach (Category category in categories)
        {
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button(category.expanded ? "-" : "+", GUILayout.Width(30)))
            {
                category.expanded = !category.expanded;
            }
            EditorGUILayout.LabelField(category.name);
            GUILayout.EndHorizontal();

            if (!category.expanded) continue;

            int rowCount = Mathf.FloorToInt((float)category.assets.Count / columnCount);
            int assetCount = 0;

            for (int r = 0; r <= rowCount; r++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int c = 0; c < columnCount; c++)
                {
                    if (GUILayout.Button(category.previews[assetCount],
                        GUILayout.Width(cellWidth), GUILayout.Height(cellHeight)))
                    {
                        GameObject asset = category.assets[assetCount];

                        if (e.button == 0)
                        {
                            //Select prefab and prepared the scene for placing mode
                            PrefabPlacer.StartPlacing(asset, category.name);
                        }

                        if(e.button == 1)
                        {
                            GenericMenu menu = new GenericMenu();

                            menu.AddItem(new GUIContent("Inspect Asset"), false,
                                () => PopUpAssetInspector.Create(asset));

                            menu.AddItem(new GUIContent("Select Prefab"), false,
                                () => SelectPrefabInProject(asset));

                            menu.AddItem(new GUIContent("Select Scene Instances"), false,
                                () => SelectSceneInstances(asset));

                            menu.ShowAsContext();
                        }
                    }

                    assetCount++;

                    if (assetCount == category.assets.Count)
                        break;
                }
                EditorGUILayout.EndHorizontal();

                if (assetCount == category.assets.Count)
                    break;
            }
        }
        EditorGUILayout.EndScrollView();
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

    void SelectSceneInstances(GameObject prefab)
    {
        GameObject[] prefabsInstances = GameObject.FindObjectsOfType<GameObject>().Where(go => PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Regular).ToArray();

        Selection.objects = prefabsInstances.Where(pi => PrefabUtility.GetCorrespondingObjectFromSource(pi) == prefab).ToArray();
    }

    class Category
    {
        public string name;
        public List<GameObject> assets;
        public List<Texture2D> previews;
        public bool expanded = true;
    }
}
