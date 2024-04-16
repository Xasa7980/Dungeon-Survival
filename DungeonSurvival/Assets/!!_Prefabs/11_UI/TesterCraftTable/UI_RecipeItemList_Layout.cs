using System.Collections;
using UnityEngine;

public class UI_RecipeItemList_Layout : UI_InventoryItemList_Layout
{
    protected override void Select(bool value)
    {
        if (value)
        {
            UI_CraftingTable.current.recipeInfoPanel.Configure((Item)item, transform.position);
            UI_CraftingTable.current.SelectItem((Item)item);
        }
        else
        {
            UI_CraftingTable.current.recipeInfoPanel.gameObject.SetActive(false);
        }
    }
}