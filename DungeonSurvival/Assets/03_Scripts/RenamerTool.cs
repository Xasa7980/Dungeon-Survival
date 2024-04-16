using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenamerTool : EditorWindow
{
    private string newName = "";
    private Vector2 scrollPos;
    private List<Object> selectedObjects = new List<Object>();

    [System.Flags]
    public enum ItemTypeFlags
    {
        None = 0,
        Ingrediente = 1 << 0,
        Recurso = 1 << 1,
        Consumible = 1 << 2,
        Comida = 1 << 3,
        Alquimia = 1 << 4,
        Misc = 1 << 5,
        Axe = 1 << 6,
        Bow = 1 << 7,
        Hammer = 1 << 8,
        Sword = 1 << 9,
        Shield = 1 << 10,
        Spear = 1 << 11,
        Staff = 1 << 12,
        Bat = 1 << 13,
        Dagger = 1 << 14,
        Helmet = 1 << 15,
        Chest = 1 << 16,
        Gauntlets = 1 << 17,
        Leggins = 1 << 18,
        Boots = 1 << 19,
        Necklace = 1 << 20,
        Ring = 1 << 21

    }
    private ItemTypeFlags selectedItemTypes = ItemTypeFlags.None;

    public enum FileType
    {
        Icon,
        Recipe,
        Item,
        ItemTag,
        ItemCategory,
        EquipmentDataSO,
        ItemAction
    }
    private FileType selectedFileType = FileType.Item;

    // Abreviaturas para los tipos de items, puedes cambiarlos como prefieras
    private Dictionary<ItemTypeFlags, string> itemTypeAbbreviations = new Dictionary<ItemTypeFlags, string>
    {
        { ItemTypeFlags.Ingrediente, "Ing" },
        { ItemTypeFlags.Recurso, "Rec" },
        { ItemTypeFlags.Consumible, "Cons" },
        { ItemTypeFlags.Comida, "Com" },
        { ItemTypeFlags.Alquimia, "Alq" },
        { ItemTypeFlags.Misc, "Misc" }
    };

    [MenuItem("Tools/Renamer Tool")]
    public static void ShowWindow ( )
    {
        EditorWindow.GetWindow<RenamerTool>("Renamer Tool");
    }

    void OnGUI ( )
    {
        GUILayout.Label("Rename Settings", EditorStyles.boldLabel);

        newName = EditorGUILayout.TextField("New Name", newName);

        selectedItemTypes = (ItemTypeFlags)EditorGUILayout.EnumFlagsField("Item Type", selectedItemTypes);
        selectedFileType = (FileType)EditorGUILayout.EnumPopup("File Type", selectedFileType);

        // List of selected objects
        GUILayout.Label("Selected Objects", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(150));
        foreach (var selectedObject in selectedObjects)
        {
            EditorGUILayout.ObjectField("Object", selectedObject, typeof(Object), true);
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Rename Assets/Objects"))
        {
            RenameSelectedObjects();
        }
    }

    void OnSelectionChange ( )
    {
        selectedObjects.Clear();
        selectedObjects.AddRange(Selection.objects);
        Repaint();
    }

    private void RenameSelectedObjects ( )
    {
        foreach (Object selectedObject in selectedObjects)
        {
            if (selectedObject != null)
            {
                string itemTypePrefix = GetItemTypePrefix(selectedItemTypes);
                string fileTypeSuffix = selectedFileType.ToString();
                string finalName = $"{itemTypePrefix}{newName}_{fileTypeSuffix}".Trim();

                string assetPath = AssetDatabase.GetAssetPath(selectedObject);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    AssetDatabase.RenameAsset(assetPath, finalName);
                }
                else if (selectedObject is GameObject)
                {
                    GameObject go = selectedObject as GameObject;
                    Undo.RecordObject(go, "Rename GameObject");
                    go.name = finalName;
                }
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Renamed {selectedObjects.Count} objects to: {newName}");
    }

    private string GetItemTypePrefix ( ItemTypeFlags itemTypes )
    {
        string prefix = "";
        foreach (ItemTypeFlags itemType in System.Enum.GetValues(typeof(ItemTypeFlags)))
        {
            if (itemTypes.HasFlag(itemType) && itemType != ItemTypeFlags.None)
            {
                // Usa la abreviatura si está disponible
                string abbreviation;
                if (itemTypeAbbreviations.TryGetValue(itemType, out abbreviation))
                {
                    prefix += $"{abbreviation}_";
                }
                else
                {
                    prefix += $"{itemType}_";
                }
            }
        }
        return prefix;
    }
}