using System.Collections;
using UnityEngine;

public class UI_DismantleItemList : UI_InventoryItemList_Layout
{
    protected override void Select(bool value)
    {
        if (value)
        {
            UI_CraftingTable.current.SelectItem((Item)item);
            UI_CraftingTable.current.dismantleInfoPanel.Configure(item, transform.position);
        }
        else
        {
            UI_CraftingTable.current.dismantleInfoPanel.gameObject.SetActive(false);
        }
    }
}