using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemInformationWindow_UI : MonoBehaviour
{
    private RectTransform infoWindowTransform;

    [SerializeField] private Image itemIcon;

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public EquipmentItemInfoPanel equipmentItemInfoPanel;

    [Serializable]
    public struct EquipmentItemInfoPanel
    {
        public ItemInformationTextContainer healthValue;
        public ItemInformationTextContainer manaValue;
        public ItemInformationTextContainer attackValue;
        public ItemInformationTextContainer attackRangeValue;
        public ItemInformationTextContainer attackSpeedValue;
        public ItemInformationTextContainer defenseValue;
        public ItemInformationTextContainer elementalPowerValue;
        public ItemInformationTextContainer criticalRateValue;
        public ItemInformationTextContainer criticalDamageValue;
        public ItemInformationTextContainer darknessImmunityValue;
    }
    public void CheckItemInformation ( )
    {
        ItemInformationTextContainer[] informationTextContainers = new ItemInformationTextContainer[]
        {
            equipmentItemInfoPanel.healthValue,
            equipmentItemInfoPanel.manaValue,
            equipmentItemInfoPanel.attackValue,
            equipmentItemInfoPanel.attackRangeValue,
            equipmentItemInfoPanel.attackSpeedValue,
            equipmentItemInfoPanel.defenseValue,
            equipmentItemInfoPanel.elementalPowerValue,
            equipmentItemInfoPanel.criticalRateValue,
            equipmentItemInfoPanel.darknessImmunityValue
        };
        foreach (ItemInformationTextContainer informationTextContainer in informationTextContainers)
        {
            if (string.IsNullOrEmpty(informationTextContainer.textValue.text))
            {
                informationTextContainer.gameObject.SetActive(false);
            }
            else
            {
                informationTextContainer.gameObject.SetActive(true);
            }
        }
    }
    private void SetItemName(string itemName )
    {
        itemName = itemName.ToUpper();
        itemNameText.text = itemName;
    }
    private void SetItemDescription(string itemDescription )
    {
        itemNameText.text = itemDescription;
    }
    private void SetItemIcon(Sprite _itemIcon, Item item )
    {
        itemIcon.transform.rotation = Quaternion.Euler(0,0,item._iconRotationZ);
        itemIcon.transform.localScale = new Vector3(item._iconScale, item._iconScale, item._iconScale);
        itemIcon.sprite = _itemIcon;
    }
    public void SetItemInformation(Item item )
    {
        SetItemName(item.name);
        SetItemDescription(item.description);
        SetItemIcon(item.icon, item);

        if (!item.equipable) return;

        if(item.equipmentDataSO == null)
        {
            //No es un equipamiento para equipar (backpack,etc)
            return; //colocar los slots que tiene el backpack, o otras cosas
        }
        EquipmentDataSO.EquipmentStats equipmentStats = item.equipmentDataSO.equipmentStats;

        equipmentItemInfoPanel.healthValue.textTitle.text = "Health :";
        equipmentItemInfoPanel.manaValue.textTitle.text = "Mana :";
        equipmentItemInfoPanel.attackValue.textTitle.text = "Attack :";
        equipmentItemInfoPanel.attackSpeedValue.textTitle.text = "Attack speed :";
        equipmentItemInfoPanel.defenseValue.textTitle.text = "Defense :";
        equipmentItemInfoPanel.elementalPowerValue.textTitle.text = "Elemental power :";
        equipmentItemInfoPanel.criticalRateValue.textTitle.text = "Critical rate :";
        equipmentItemInfoPanel.criticalDamageValue.textTitle.text = "Critical damage :";

        equipmentItemInfoPanel.healthValue.textValue.text = equipmentStats.healthPoints.ToString();
        equipmentItemInfoPanel.manaValue.textValue.text = equipmentStats.manaPoints.ToString();
        equipmentItemInfoPanel.attackValue.textValue.text = equipmentStats.attackPoints.ToString();
        equipmentItemInfoPanel.attackSpeedValue.textValue.text = equipmentStats.attackSpeed.ToString();
        equipmentItemInfoPanel.defenseValue.textValue.text = equipmentStats.defensePoints.ToString();
        equipmentItemInfoPanel.elementalPowerValue.textValue.text = equipmentStats.elementalPower.ToString();
        equipmentItemInfoPanel.criticalRateValue.textValue.text = equipmentStats.criticalRate.ToString();
        equipmentItemInfoPanel.criticalDamageValue.textValue.text = equipmentStats.criticalDamage.ToString();

        CheckItemInformation();
    }
}
