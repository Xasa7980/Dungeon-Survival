using Sirenix.OdinInspector;
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
    [ShowIf("@equipmentCategory == EquipmentCategory.Weapon"), SerializeField] private WeaponHandler weaponHandlerType;
    [ShowIf("@equipmentCategory == EquipmentCategory.Weapon"), SerializeField] private WeaponRange weaponRange;
    [ShowIf("@equipmentCategory == EquipmentCategory.Weapon"), SerializeField] public Material slashMaterial;
    
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
        equipmentDataSO.weaponHandlerType = weaponHandlerType;
        equipmentDataSO.weaponRange = weaponRange;
        slashMaterial = equipmentDataSO.equipmentVisualEffects.weaponSlashMaterial;
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
