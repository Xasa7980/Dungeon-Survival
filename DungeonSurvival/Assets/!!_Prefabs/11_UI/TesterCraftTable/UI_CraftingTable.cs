using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using static UnityEditor.Progress;
using System.Linq;

public class UI_CraftingTable : MonoBehaviour
{
    public static CraftingTable currentTable;

    public static UI_CraftingTable current { get; private set; }

    GameObject currentLayout;

    [SerializeField] Sprite lockedSlotSprite;

    [Title("Global Toggles")]

    [SerializeField] Toggle recipeToggle;
    [SerializeField] Toggle itemsToggle;

    [Title("Info Panels")]
    [SerializeField] UI_RecipeInfo _recipeInfoPanel;
    [SerializeField] UI_RecipeInfo _dismantleInfoPanel;
    [SerializeField] UI_PieceInfo _pieceInfoPanel;
    [SerializeField] UI_CraftingStatsInfoPanel _craftingStatsInfoPanel;
    public UI_RecipeInfo recipeInfoPanel => _recipeInfoPanel;
    public UI_RecipeInfo dismantleInfoPanel => _dismantleInfoPanel;
    public UI_PieceInfo pieceInfoPanel => _pieceInfoPanel;
    public UI_CraftingStatsInfoPanel craftingStatsInfoPanel => _craftingStatsInfoPanel;

    [Title("Prefabs")]
    [SerializeField] UI_RecipeItemList_Layout recipeItemList_Prefab;
    [SerializeField] UI_DismantleItemList _dismantleItemList_Prefab;
    [SerializeField] UI_PieceItemList_Layout _pieceItemListPrefab;
    [SerializeField] UI_Stat_Layout statsLayoutPrefab;

    [Title("Recipe Window Elements")]
    [SerializeField] ItemTag recipeItemTagReference;
    [SerializeField] GameObject recipeWindow;
    [SerializeField] ToggleGroup recipeListContainer;

    iItemData selectedItem;
    public Item_Recipe recipe { get; private set; }

    [Title("Crafing Pieces Window Elements")]
    [SerializeField] GameObject piecesWindow;
    [SerializeField] ToggleGroup piecesListContainer;
    Transform pieceAnchor;

    [Title("Create Item Window Elements")]
    [SerializeField] GameObject newItemWindow;
    [SerializeField] Image newItemIcon;
    [SerializeField] InputField newItemName;
    [SerializeField] InputField newItemDescription;
    [SerializeField] Transform newItemStats;

    Item itemToBeCrafted;
    InventoryItem[] quickAccessInventory => PlayerInventory.current.allItems;
    InventoryItem[] backpackInventory => PlayerInventory.current.GetEquipedBackpack.allItems;
    public void SelectItem ( Item item )
    {
        selectedItem = item;
    }

    void Awake ( )
    {
        current = this;
        craftingStatsInfoPanel.gameObject.SetActive(false);
    }

    public void OpenRecipeWindow ( )
    {
        recipeWindow.SetActive(true);
        recipeToggle.isOn = true;

        recipeInfoPanel.gameObject.SetActive(false);
        dismantleInfoPanel.gameObject.SetActive(false);
    }

    public void CloseWindow ( GameObject window )
    {
        window.SetActive(false);

        recipeInfoPanel.gameObject.SetActive(false);
        dismantleInfoPanel.gameObject.SetActive(false);
    }
    List<Item_Recipe> recipes = new List<Item_Recipe>();

    public void ShowRecipes ( bool value = true )
    {
        if (!value) return;

        UI_InventoryItemList_Layout[] prevRecipes = recipeListContainer.GetComponentsInChildren<UI_InventoryItemList_Layout>();
        foreach (var recipe in prevRecipes)
        {
            Destroy(recipe.gameObject);
        }

        recipes = GetAllInventoryRecipeList();
        foreach (Item_Recipe item in recipes)
        {
            print(item.displayName);

            if (item != null)
            {
                print("Creando instancia");
                UI_InventoryItemList_Layout.CreateInstance(recipeItemList_Prefab, item, recipeListContainer.transform);
            }
        }
        recipeListContainer.SetAllTogglesOff();

        recipeInfoPanel.gameObject.SetActive(false);
        dismantleInfoPanel.gameObject.SetActive(false);
    }
    private List<Item_Recipe> GetAllInventoryRecipeList ( )
    {
        List<Item_Recipe> recipes = new List<Item_Recipe>();

        if (PlayerInventory.current != null && quickAccessInventory != null)
        {
            foreach (InventoryItem inventoryItem in quickAccessInventory)
            {
                if (inventoryItem != null && inventoryItem.item != null && inventoryItem.item is Item_Recipe recipeItem)
                {
                    if (recipeItem.itemTag == recipeItemTagReference)
                    {
                        recipes.Add(recipeItem);
                    }
                }
            }
        }
        else if (PlayerInventory.current != null && backpackInventory != null)
        {
            foreach (InventoryItem inventoryItem in backpackInventory)
            {
                if (inventoryItem != null && inventoryItem.item != null && inventoryItem.item is Item_Recipe recipeItem)
                {
                    if (recipeItem.itemTag == recipeItemTagReference)
                    {
                        recipes.Add(recipeItem);
                    }
                }
            }
        }
        return recipes;
    }
    public void ShowItems ( bool value = true )
    {
        if (!value) return;

        UI_InventoryItemList_Layout[] prevRecipes = recipeListContainer.GetComponentsInChildren<UI_InventoryItemList_Layout>();
        foreach (var recipe in prevRecipes)
        {
            Destroy(recipe.gameObject);
        }

        List<iItemData> items = new List<iItemData>();

        foreach (iItemData item in backpackInventory)
        {
            if (item == null) continue;

            if (item.canBeDismantled)
                items.Add(item);
        }

        foreach (iItemData item in items)
        {
            UI_DismantleItemList.CreateInstance(_dismantleItemList_Prefab, item, recipeListContainer.transform);
        }

        recipeListContainer.SetAllTogglesOff();

        recipeInfoPanel.gameObject.SetActive(false);
        dismantleInfoPanel.gameObject.SetActive(false);
    }

    public void Dismantle ( )
    {
        for (int i = 0; i < selectedItem.resultingPieces.Length; i++)
        {
            PlayerInventory.current.TryAddItem(selectedItem.resultingPieces[i]);
        }

        PlayerInventory.current.TryRemoveItem((Item)selectedItem, 1);
        selectedItem = null;
        ShowItems();
        _dismantleInfoPanel.gameObject.SetActive(false);
    }

    public void SelectRecipe ( )
    {
        currentTable.ClearTable();

        recipe = (Item_Recipe)selectedItem;
        itemToBeCrafted = recipe.itemToCraft;
        if (currentTable.craftingPosition.childCount > 0)
        {
            Transform[] childs = currentTable.craftingPosition.GetComponentsInChildren<Transform>();
            foreach (Transform child in childs)
            {
                if (child == currentTable.craftingPosition) continue;
                Destroy(child.gameObject);
            }
        }

        currentLayout = Instantiate(recipe.layoutObject, currentTable.craftingPosition); //XAVI CORREGIR

        currentTable.PrepareTable(recipe);

        recipeWindow.SetActive(false);
    }

    public void ClearRecipe ( )
    {
        recipe = null;
    }

    public void ResetTable ( )
    {
        currentTable.ClearTable();
        ClearRecipe();
    }

    public void FindCraftingPiece ( Item ingredientItem, Transform anchor )
    {
        List<Item> itemPieces = new List<Item>();

        UI_PieceItemList_Layout[] currentListLayout = piecesListContainer.GetComponentsInChildren<UI_PieceItemList_Layout>();
        foreach (UI_PieceItemList_Layout currentListInLayout in currentListLayout)
        {
            Destroy(currentListInLayout.gameObject);
        }

        foreach (iItemData i in backpackInventory)
        {
            if (i == null) continue;

            if (i.GetType() == typeof(Item))
            {
                Item piece = (Item)i;
                if (piece == ingredientItem)
                    itemPieces.Add(piece);
            }
        }

        foreach (iItemData i in quickAccessInventory)
        {
            if (i == null) continue;

            if (i.GetType() == typeof(Item))
            {
                Item piece = (Item)i;
                if (piece == ingredientItem)
                    itemPieces.Add(piece);
            }
        }

        foreach (Item p in itemPieces)
        {
            UI_PieceItemList_Layout.CreateInstance(_pieceItemListPrefab, p, piecesListContainer.transform);
        }

        piecesWindow.SetActive(true);

        pieceAnchor = anchor;
    }

    public void OpenCreateItemWindow ( )
    {
        newItemWindow.SetActive(true);
        itemToBeCrafted = CreateItem();
        currentTable.locked = true;

        UI_Stat_Layout[] sls = newItemStats.GetComponentsInChildren<UI_Stat_Layout>();
        foreach (UI_Stat_Layout sl in sls)
        {
            Destroy(sl.gameObject);
        }
    }

    public void CloseCreateItemWindow ( )
    {
        currentTable.locked = false;
        newItemWindow.SetActive(false);
    }

    public void SetItemName ( string value )
    {
        itemToBeCrafted.displayName = value;
    }

    public void SetItemDescription ( string value )
    {
        itemToBeCrafted.description = value;
    }

    public void CraftItem ( )
    {
        if (!HasAbleItemsToCraft()) return;

        iItemData item = CreateItem(itemToBeCrafted);
        if (PlayerInventory.current.TryAddItem((Item)item))
        {
            newItemWindow.SetActive(false);

            Debug.Log("Item " + itemToBeCrafted.displayName + " successfully added to the inventory");
            foreach (Item piece in itemToBeCrafted.resultingPieces)
            {
                if (piece == null) return;

                PlayerInventory.current.TryRemoveItem(piece, recipe.ingredients.Select(r => r.amount).First());

            }
            ResetTable();
            itemToBeCrafted = null;
        }
        else
        {
            Debug.LogError("The item " + itemToBeCrafted.displayName + " can't be added to the inventory");
        }

        currentTable.locked = false;
    }
    private bool HasAbleItemsToCraft ( )
    {
        for (int i = 0; i < quickAccessInventory.Length; i++)
        {
            if(i < recipe.ingredients.Count)
            {
                if (quickAccessInventory[i].currentStack >= recipe.ingredients[i].amount)
                {
                    return true;
                }
                else
                {
                    Debug.LogError("Not enought items!!");
                }
            }
        }
        return false;
    }
    public iItemData CreateItem ( Item newItem)
    {
        Item item = ScriptableObject.CreateInstance(typeof(Item)) as Item;

        item.Reconfigure(newItem);

        return item;
    }

    public Item CreateItem ( )
    {
        Item data = (Item)ScriptableObject.CreateInstance("Item");

        data.displayName = string.IsNullOrEmpty(itemToBeCrafted.displayName) ? recipe.itemToCraft.displayName : "";
        data.description = string.IsNullOrEmpty(itemToBeCrafted.description) ? recipe.itemToCraft.description : "";
        data.icon = itemToBeCrafted.icon; //Buscar una forma de poner iconos a los items
        data.itemTag = recipe.itemToCraft.itemTag;

        data.canBeDismantled = true;
        data.resultingPieces = currentTable.requiredPieces;

        data.canBeInHotbar = recipe.itemToCraft.canBeInHotbar;//Es falso porque los items que se hacen en la mesa de crafteo
                                   //no pueden colocarse en el hotbar

        data.isStackable = recipe.itemToCraft.isStackable;  //Lo mismo aqui
        data.maxStack = 1;      //1 porque solo se craftea un item a la vez

        data.equipable = recipe.itemToCraft.equipable;
        data.lookAtCursor = recipe.itemToCraft.lookAtCursor;
        data.lookSpeed = recipe.itemToCraft.lookSpeed;
        data.canMove = recipe.itemToCraft.canMove;

        if(recipe.itemToCraft.equipable) data.weaponType = recipe.itemToCraft.weaponType;

        //data.idleAnimation = recipe.idleAnimation;
        //data.walkAnimation = recipe.walkAnimation;
        //data.runAnimation = recipe.runAnimation;
        data.useAnimations = recipe.itemToCraft.useAnimations;
        //data.proyectile = recipe.proyectile;
        //data.reloadAnimation = recipe.reloadAnimation;

        return data;
    }
}