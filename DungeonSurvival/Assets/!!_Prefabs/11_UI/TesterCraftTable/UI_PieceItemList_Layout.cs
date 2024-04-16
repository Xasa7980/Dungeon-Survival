using System.Collections;
using UnityEngine;

public class UI_PieceItemList_Layout : UI_InventoryItemList_Layout
{
    protected override void Select(bool value)
    {
        if (value)
        {
            UI_CraftingTable.current.pieceInfoPanel.Configure((Item)item, transform.position);
            UI_CraftingTable.current.SelectItem((Item)item);
        }
        else
        {
            UI_CraftingTable.current.recipeInfoPanel.gameObject.SetActive(false);
        }
    }
}