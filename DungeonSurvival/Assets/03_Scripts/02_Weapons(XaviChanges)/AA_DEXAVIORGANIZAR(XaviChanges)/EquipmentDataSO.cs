using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentElement
{
    None,
    Water,
    Fire,
    Darkness,
    Light,
    Thunder,
    Earth,
    Wind,
    Ice
}
public enum EquipmentType
{
    Axe,
    Bow,
    Hammer,
    Sword,
    Shield,
    Spear,
    Staff,
    Bat,
    Dagger,
    Helmet,
    Chest,
    Gauntlets,
    Leggins,
    Boots,
    Necklace,
    Ring
}
public enum EquipmentCategory
{
    Weapon,
    Clothes,
    Accesory
}

public enum EquipmentRank
{
    Normal,
    Uncommon,
    Rare,
    Mythic,
    Legendary,
}
public enum WeaponHandler
{
    Hand_1,
    Hand_2
}
public enum WeaponRange
{
    Melee,
    Ranged
}
[CreateAssetMenu(fileName = "NewEquipmentData", menuName = "Dungeon Survival/Equipment/Data")]
public class EquipmentDataSO : ScriptableObject
{
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    [Title("$equipmentName", "$EquipmentInformation", TitleAlignments.Centered)]
    [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
    public string equipmentName;

    [GUIColor(0.15f, 0.8f, 0.8f, 1f)]
    [TextArea]
    public string description;

    public EquipmentType equipmentType;
    public EquipmentElement equipmentElement;
    [OnValueChanged("SetEquipmentRankToStats")]public EquipmentRank equipmentRank;
    [OnValueChanged("SetEquipmentCategoryToStats"), OnValueChanged("SetEquipmentCategoryToVisuals")] public EquipmentCategory equipmentCategory;

    [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")]public WeaponHandler weaponHandlerType = WeaponHandler.Hand_1;
    [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")]public WeaponRange weaponRange = WeaponRange.Melee;

    [GUIColor(0.3f, 1f, 0.5f, 1f), PropertySpace(SpaceBefore = 10, SpaceAfter = 10), FoldoutGroup("Main Stats")] 
    public EquipmentStats equipmentStats;

    [GUIColor(0.3f, 1f, 0.8f, 1f), PropertySpace(SpaceBefore = 10, SpaceAfter = 10), FoldoutGroup("Visual Options")] 
    public EquipmentVisualEffects equipmentVisualEffects;

    private string EquipmentInformation => equipmentType.ToString();

    private void SetEquipmentCategoryToStats() => equipmentStats.equipmentCategory = equipmentCategory;
    private void SetEquipmentRankToStats() => equipmentStats.equipmentRank = equipmentRank;
    private void SetEquipmentCategoryToVisuals ( ) => equipmentVisualEffects.equipmentCategory = equipmentCategory;
    private void OnValidate ( )
    {
        UpdateSlashColor();
        UpdateEquipmentRank();
    }

    [OnValueChanged("UpdateSlashColor")]
    private void UpdateSlashColor ( )
    {
        switch (equipmentElement)
        {
            case EquipmentElement.None:

                equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[0]);
                break;
            case EquipmentElement.Water:

                equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[1]);
                break;

            case EquipmentElement.Fire:

                equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[2]);
                break;

            case EquipmentElement.Darkness:

                equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[3]);
                break;
            case EquipmentElement.Light:

                equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[4]);
                break;

            case EquipmentElement.Thunder:

                equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[5]);
                break;

            case EquipmentElement.Earth:

                equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[6]);
                break;

            case EquipmentElement.Wind:

                equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[7]);
                break;
                
            case EquipmentElement.Ice:

                equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[8]);
                break;

            // Agrega casos para los otros elementos
            default:
                return;
        }
    }
    [OnValueChanged("UpdateEquipmentRank")]
    private void UpdateEquipmentRank ( )
    {
        switch (equipmentRank)
        {
            case EquipmentRank.Normal:
                EnableEffects(false);
                break;
            case EquipmentRank.Uncommon:
                EnableEffects(false);
                break;
            case EquipmentRank.Rare:
                EnableEffects(false);
                break;
            case EquipmentRank.Mythic:
                EnableEffects(true);
                break;
            case EquipmentRank.Legendary:
                EnableEffects(true);
                break;
            default:
                break;
        }
    }
    public void EnableEffects(bool enable )
    {
        if (equipmentVisualEffects.slashParticleEffect != null)
        {
            equipmentVisualEffects.hasSlashEffect = enable;
        }
        if (!equipmentVisualEffects.hasDefaultParticleEffects)
        {
            if (equipmentVisualEffects.hasSecondaryParticleEffect)
            {
                if (equipmentVisualEffects.equipmentSecondaryParticleEffect != null) equipmentVisualEffects.equipmentSecondaryParticleEffect.SetActive(enable);
            }
        }
        else return;
    }
    //public Color GetEmissiveColor (Material material )
    //{
    //    if (material.IsKeywordEnabled("_EMISSION"))
    //    {
    //        Color color = material.GetColor("_EmissionColor");
    //        return color;
    //    }
    //    else return Color.white;
    //}
    public void SetEquipment_SlashEffectMaterial ( ParticleSystemRenderer particleSystemRenderer )
    {
        particleSystemRenderer.material = equipmentVisualEffects.weaponSlashMaterial;
    }

    [System.Serializable]
    public class EquipmentVisualEffects
    {
        internal EquipmentCategory equipmentCategory;

        //[InfoBox("This particle effect only is enabled when Equipment rank is high and the equipment has particles", InfoMessageType.Warning)]
        public List<Transform> slashParticleEffect;
        [InfoBox("This particle effect only is enabled when Equipment rank is high and the equipment has secondary particles",InfoMessageType.Warning)]
        public GameObject equipmentSecondaryParticleEffect;

        public Color emissiveColor;

        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public bool hasSlashEffect;
        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public bool hasSecondaryParticleEffect;
        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public bool hasDefaultParticleEffects;

        [InfoBox("It requires an specific order on putting inside the materials, check EquipmentElement list", InfoMessageType.Warning)]
        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public Material[] slashMaterials;
        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public Material weaponSlashMaterial;

        public void SetWeaponSlashMaterial ( Material material ) => weaponSlashMaterial = material;
    }
    [System.Serializable]
    public class EquipmentStats
    {
        internal EquipmentCategory equipmentCategory;
        internal EquipmentRank equipmentRank;

        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public float attackPoints;
        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public float attackRange;

        public float attackSpeed;
        public float elementalPower;
        public float healthPoints;
        public float energyPoints;
        public float criticalRate;
        public float criticalDamage;

        private float valuesMultiplier;
        private float minimalValue;

        [PropertyOrder(-2)]
        [HorizontalGroup("Values")]
        [GUIColor(1f, 0.5f, 0f, 1f)]
        [Button(ButtonSizes.Large)]
        public void GetRandomValues ( )
        {
            switch (equipmentRank)
            {
                case EquipmentRank.Normal:
                    valuesMultiplier = 1f;
                    minimalValue = 1;
                    break;
                case EquipmentRank.Uncommon:
                    valuesMultiplier = 1.25f;
                    minimalValue = 15;
                    break;
                case EquipmentRank.Rare:
                    valuesMultiplier = 1.45f;
                    minimalValue = 35;
                    break;
                case EquipmentRank.Mythic:
                    valuesMultiplier = 1.75f;
                    minimalValue = 60;
                    break;
                case EquipmentRank.Legendary:
                    valuesMultiplier = 2.1f;
                    minimalValue = 85;
                    break;
                default:
                    break;
            }
             
            //Valores dinamicos
            attackPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue, 100) * valuesMultiplier);
            elementalPower = MathF.Floor(UnityEngine.Random.Range(minimalValue, 100f) * valuesMultiplier);
            healthPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue * 2, 1000f) * valuesMultiplier);
            energyPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue * 2, 100f) * valuesMultiplier);

            //Valores estaticos
            attackSpeed = MathF.Floor(UnityEngine.Random.Range(minimalValue * 0.15f, 100f));
            criticalRate = MathF.Floor(UnityEngine.Random.Range(0, 70));
            criticalDamage = MathF.Floor(UnityEngine.Random.Range(minimalValue * 0.25f, 200f));
        }
    }
}

