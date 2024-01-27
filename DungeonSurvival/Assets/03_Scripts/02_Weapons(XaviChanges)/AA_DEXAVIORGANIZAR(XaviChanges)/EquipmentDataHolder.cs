using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentDataHolder : MonoBehaviour
{
    [SerializeField] private EquipmentDataSO equipmentDataSO;
    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private EquipmentElement equipmentElement;
    [SerializeField] private EquipmentRank equipmentRank;
    [SerializeField] private EquipmentCategory equipmentCategory;
    
    private AreaDrawer detectionArea;
    private void Awake ( )
    {
        detectionArea = GetComponent<AreaDrawer>();
    }
    private void OnValidate ( )
    {
        equipmentDataSO.equipmentType = equipmentType;
        equipmentDataSO.equipmentElement = equipmentElement;
        equipmentDataSO.equipmentRank = equipmentRank;
        equipmentDataSO.equipmentCategory = equipmentCategory;

    }

    public AreaDrawer GetDetectionArea()
    {
        return detectionArea; 
    }
    public EquipmentDataSO GetEquipmentDataSO ( )
    {
        return equipmentDataSO;
    }
    public EquipmentType GetEquipmentType()
    {
        return equipmentDataSO.equipmentType;
    }
}
