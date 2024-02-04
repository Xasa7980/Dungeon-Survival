using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentDataHolder : MonoBehaviour
{
    public EquipmentElement GetEquipmentElement => equipmentElement;
 
    [SerializeField] private EquipmentDataSO equipmentDataSO;

    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private EquipmentElement equipmentElement;
    [SerializeField] private EquipmentRank equipmentRank;
    [SerializeField] private EquipmentCategory equipmentCategory;
    [ShowIf("@equipmentCategory == EquipmentCategory.Weapon"), SerializeField] private WeaponHandler weaponHandlerType;
    [ShowIf("@equipmentCategory == EquipmentCategory.Weapon"), SerializeField] private WeaponRange weaponRange;

    [ShowIf("@equipmentCategory == EquipmentCategory.Weapon"), SerializeField] public Material slashMaterial;
    [ShowIf("@equipmentCategory == EquipmentCategory.Weapon"), SerializeField] private List<Transform> slashGameObject = new List<Transform>();
    
    private AreaDrawer detectionArea;
    private void Awake ( )
    {
        detectionArea = GetComponent<AreaDrawer>();
    }
    private void OnValidate ( )
    {
        AddChildrenToList();
        equipmentDataSO.equipmentType = equipmentType;
        equipmentDataSO.equipmentElement = equipmentElement;
        equipmentDataSO.equipmentRank = equipmentRank;
        equipmentDataSO.weaponHandlerType = weaponHandlerType;
        equipmentDataSO.weaponRange = weaponRange;
        equipmentDataSO.equipmentVisualEffects.slashParticleEffect = slashGameObject;
        slashMaterial = equipmentDataSO.equipmentVisualEffects.weaponSlashMaterial;
    }
    private void AddChildrenToList ( )
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeleeWeaponTrail>() != null && !slashGameObject.Contains(child))
            {
                slashGameObject.Add(child);
            }
            else return;
        }
    }
    public AreaDrawer GetDetectionArea()
    {
        return detectionArea; 
    }
    public EquipmentDataSO GetEquipmentDataSO ( )
    {
        return equipmentDataSO;
    }
    public bool Is2HandWeapon => weaponHandlerType == WeaponHandler.Hand_2;
    public EquipmentType GetEquipmentType()
    {
        return equipmentDataSO.equipmentType;
    }
}
