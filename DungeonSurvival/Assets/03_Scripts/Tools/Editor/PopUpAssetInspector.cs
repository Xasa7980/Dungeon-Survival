using UnityEditor;
using UnityEngine;

public class PopUpAssetInspector : EditorWindow
{
    private Object asset;
    private Editor assetEditor;
    Component[] allComponents;
    Editor[] allComponentsEditors;
    Vector2 scroll;

    public static PopUpAssetInspector Create(Object asset)
    {
        var window = CreateWindow<PopUpAssetInspector>(asset.name);
        window.asset = asset;
        window.assetEditor = Editor.CreateEditor(asset);
        window.allComponents=((GameObject)asset).GetComponents<Component>();
        window.allComponentsEditors = new Editor[window.allComponents.Length];
        for (int i = 0; i < window.allComponents.Length; i++)
        {
            window.allComponentsEditors[i] = Editor.CreateEditor(window.allComponents[i]);
        }
        return window;
    }

    private void OnGUI()
    {
        assetEditor.DrawPreview(new Rect(0, 0, position.width, position.width / 2));

        GUILayout.Space(position.width / 2 + 5);

        if(GUILayout.Button("Open Asset"))
        {
            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GetAssetPath(asset)));
        }

        scroll = GUILayout.BeginScrollView(scroll);
        assetEditor.DrawHeader();
        for(int i = 0;i < allComponentsEditors.Length; i++)
        {
            allComponentsEditors[i].DrawHeader();
            allComponentsEditors[i].OnInspectorGUI();
        }
        GUILayout.EndScrollView();
    }
}