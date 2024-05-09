using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentDataHolder : MonoBehaviour
{
    public EquipmentElement GetEquipmentElement => equipmentElement;
 
    [SerializeField] private EquipmentDataSO equipmentDataSO;

    public WorldItem worldItem => _worldItem;
    [SerializeField] private WorldItem _worldItem;
    public void SetWorldItem(WorldItem worldItem ) {  _worldItem = worldItem; }
    public Item equipmentItem => _equipmentItem;
    [SerializeField] private Item _equipmentItem;
    public void SetEquipmentItem(Item item) { _equipmentItem = item; }

    [SerializeField] private EquipmentType equipmentType;
    [SerializeField] private EquipmentElement equipmentElement;
    [SerializeField] private EquipmentRank equipmentRank;
    [SerializeField] private ItemCategories itemCategory;
    [ShowIf("@itemCategory == ItemCategories.Weapon"), SerializeField] private WeaponHandler weaponHandlerType;
    [ShowIf("@itemCategory == ItemCategories.Weapon"), SerializeField] private CombatGearType weaponRange;

    [ShowIf("@itemCategory == ItemCategories.Weapon"), SerializeField] public Material slashMaterial;
    [ShowIf("@itemCategory == ItemCategories.Weapon"), SerializeField] private List<Transform> slashGameObject = new List<Transform>();

    private MeleeWeaponTrail weaponTrail;
    private AreaDrawer detectionArea;
    public bool Is2HandWeapon => weaponHandlerType == WeaponHandler.Hand_2;
    public bool isWorldItem => worldItem.isActiveAndEnabled;

    private void Awake ( )
    {
        weaponTrail = GetComponentInChildren<MeleeWeaponTrail>();
        detectionArea = GetComponent<AreaDrawer>();
        _worldItem = GetComponent<WorldItem>();
        _equipmentItem = worldItem.item;
    }
    private void OnValidate ( )
    {
        if (equipmentDataSO == null) return;

        slashGameObject.Clear();
        AddChildrenToList();
        equipmentDataSO.equipmentType = equipmentType;
        equipmentDataSO.equipmentElement = equipmentElement;
        equipmentDataSO.equipmentRank = equipmentRank;
        equipmentDataSO.weaponHandlerType = weaponHandlerType;
        equipmentDataSO.combatGearType = weaponRange;
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
    public MeleeWeaponTrail GetWeaponTrail ( )
    {
        return weaponTrail;
    }
    public EquipmentType GetEquipmentType()
    {
        return equipmentDataSO.equipmentType;
    }
}
public static class ExtendedEquipmentDataHolder
{
    public static bool IsType(this EquipmentDataHolder equipmentDataHolder, Item newEquipmentItem)
    {
        if(equipmentDataHolder.equipmentItem != null && equipmentDataHolder.equipmentItem.IsType(newEquipmentItem.itemTag))
        {
            return true;
        }
        return false;
    }
}