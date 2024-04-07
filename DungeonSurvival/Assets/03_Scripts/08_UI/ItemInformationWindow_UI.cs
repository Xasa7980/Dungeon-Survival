using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemInformationWindow_UI : MonoBehaviour
{

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] RectTransform prefabParent;
    [SerializeField] private ItemInformationTextContainer textContainer;

    public void CheckItemInformation ( Item item )
    {
        if(item.TryGetAction(out ItemAction action))
        {
            if(action is HealingItemAction)
            {
                HealingItemAction healingItemAction = (HealingItemAction)action;
                ItemInformationTextContainer healPointsValue = Instantiate(textContainer, prefabParent.transform);
                ItemInformationTextContainer healSpeedValue = Instantiate(textContainer, prefabParent.transform);
                healPointsValue.textValue.text = healingItemAction.healPoints.ToString();
                healSpeedValue.textValue.text = healingItemAction.healingFillTime.ToString();
            }
            else if (action is FoodItemAction)
            {
                FoodItemAction foodItemAction = (FoodItemAction)action;
                ItemInformationTextContainer hungerRecoveringValue = Instantiate(textContainer, prefabParent.transform);
                ItemInformationTextContainer feedSpeedValue = Instantiate(textContainer, prefabParent.transform);
                hungerRecoveringValue.textValue.text = foodItemAction.hungerPoints.ToString();
                feedSpeedValue.textValue.text = foodItemAction.eatFillTime.ToString();
            }
            else if (action is BoostItemAction)
            {
                BoostItemAction boostItemAction = (BoostItemAction)action;

                ItemInformationTextContainer statPoints = Instantiate(textContainer, prefabParent.transform);

                statPoints.textValue.text = boostItemAction.statPoints.ToString();
            }
            else if (action is RepairItemAction)
            {
                RepairItemAction repairItemAction = (RepairItemAction)action;
                ItemInformationTextContainer repairingQuantityValue = Instantiate(textContainer, prefabParent.transform);
                ItemInformationTextContainer repairingSpeed = Instantiate(textContainer, prefabParent.transform);

                repairingQuantityValue.textValue.text = repairItemAction.repairingPoints.ToString();
                repairingSpeed.textValue.text = repairItemAction.repairingFillTime.ToString();
            }
            //else if(action is HealingItemAction)
            //{
            //MagicItemAction TENGO QUE COMPLETAR ESA CLASE
            //}
        }
    }
    private void SetItemName ( string itemName )
    {
        itemName = itemName.ToUpper();
        itemNameText.text = itemName;
    }
    private void SetItemDescription ( string itemDescription )
    {
        itemNameText.text = itemDescription;
    }
    private void SetItemIcon ( Sprite _itemIcon, Item item )
    {
        itemIcon.transform.rotation = Quaternion.Euler(0, 0, item._iconRotationZ);
        itemIcon.transform.localScale = new Vector3(item._iconScale, item._iconScale, item._iconScale);
        itemIcon.sprite = _itemIcon;
    }
    public void SetItemInformation ( Item item )
    {
        SetItemName(item.name);
        SetItemDescription(item.description);
        CheckItemInformation(item);
    }
}
