using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public enum EquipmentCategory
{
    Melee,
    Range,
    Shield,
    Armor
}
[CreateAssetMenu(fileName = "NewEquipmentData", menuName = "Dungeon Survival/Inventory/EquipmentData")]

public class EquipmentDataSO : ScriptableObject
{
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    [Title("$equipmentName", "$description", TitleAlignments.Centered)]
    [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]
    [SerializeField] 
    public string equipmentName;

    [GUIColor(0.15f, 0.8f, 0.8f, 1f)]
    [TextArea]
    [SerializeField]
    public string description;

    public EquipmentType equipmentType;
    public Element equipmentElement;
    public EquipmentRank equipmentRank;

    [ShowIf("@itemCategory == ItemCategories.Weapon")]
    public WeaponHandler weaponHandlerType = WeaponHandler.Hand_1;
    [ShowIf("@itemCategory == ItemCategories.Armor")]
    public int armorIndex;

    public EquipmentCategory equipmentCategory = EquipmentCategory.Melee;

    [GUIColor(0.3f, 1f, 0.5f, 1f), PropertySpace(SpaceBefore = 10, SpaceAfter = 10), FoldoutGroup("Main Stats")]
    public EquipmentStats equipmentStats;

    [GUIColor(0.3f, 1f, 0.8f, 1f), PropertySpace(SpaceBefore = 10, SpaceAfter = 10), FoldoutGroup("Visual Options")]
    public EquipmentVisualEffects equipmentVisualEffects;

    [GUIColor(0.3f, 1f, 0.8f, 1f), PropertySpace(SpaceBefore = 10, SpaceAfter = 10), FoldoutGroup("Animation Options")]
    public EquipmentAnimations equipmentAnimationClips;

    [OnValueChanged("SetEquipmentCategoryToStats"), OnValueChanged("SetEquipmentCategoryToVisuals")]
    private ItemCategories itemCategory => equipmentType >= EquipmentType.Dagger ? ItemCategories.Weapon : equipmentType < EquipmentType.Dagger && equipmentType >= EquipmentType.Boots ?
    ItemCategories.Armor : ItemCategories.Accesory;
    public bool secondWeaponAble => itemCategory == ItemCategories.Weapon && weaponHandlerType == WeaponHandler.Hand_1;
    private void SetEquipmentCategoryToStats ( ) => equipmentStats.equipmentCategory = itemCategory;
    private void SetEquipmentRankToStats ( ) => equipmentStats.equipmentRank = equipmentRank;
    private void SetEquipmentCategoryToVisuals ( ) => equipmentVisualEffects.equipmentCategory = itemCategory;
    private void SetEquipmentCategoryToAnimationClips ( ) => equipmentAnimationClips.equipmentCategory = itemCategory;
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
            case Element.None:

                if (equipmentVisualEffects.slashMaterials.Length > 0)
                    equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[0]);
                break;
            case Element.Water:

                if (equipmentVisualEffects.slashMaterials.Length > 0)
                    equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[1]);
                break;

            case Element.Fire:

                if (equipmentVisualEffects.slashMaterials.Length > 0)
                    equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[2]);
                break;

            case Element.Darkness:

                if (equipmentVisualEffects.slashMaterials.Length > 0)
                    equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[3]);
                break;
            case Element.Light:

                if (equipmentVisualEffects.slashMaterials.Length > 0)
                    equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[4]);
                break;

            case Element.Thunder:

                if (equipmentVisualEffects.slashMaterials.Length > 0)
                    equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[5]);
                break;

            case Element.Earth:

                if (equipmentVisualEffects.slashMaterials.Length > 0)
                    equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[6]);
                break;

            case Element.Wind:

                if (equipmentVisualEffects.slashMaterials.Length > 0)
                    equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[7]);
                break;
                
            case Element.Ice:

                if (equipmentVisualEffects.slashMaterials.Length > 0)
                    equipmentVisualEffects.SetWeaponSlashMaterial(equipmentVisualEffects.slashMaterials[8]);
                break;

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
        internal ItemCategories equipmentCategory;

        //[InfoBox("This particle effect only is enabled when Equipment rank is high and the equipment has particles", InfoMessageType.Warning)]
        public List<Transform> slashParticleEffect;
        [InfoBox("This particle effect only is enabled when Equipment rank is high and the equipment has secondary particles",InfoMessageType.Warning)]
        public GameObject equipmentSecondaryParticleEffect;

        public Color emissiveColor;

        [ShowIf("@itemCategory == ItemCategories.Weapon")] public bool hasSlashEffect;
        [ShowIf("@itemCategory == ItemCategories.Weapon")] public bool hasSecondaryParticleEffect;
        [ShowIf("@itemCategory == ItemCategories.Weapon")] public bool hasDefaultParticleEffects;

        [InfoBox("It requires an specific order on putting inside the materials, check Element list", InfoMessageType.Warning)]
        [ShowIf("@itemCategory == ItemCategories.Weapon")] public Material[] slashMaterials;
        [ShowIf("@itemCategory == ItemCategories.Weapon")] public Material weaponSlashMaterial;

        public void SetWeaponSlashMaterial ( Material material ) => weaponSlashMaterial = material;
    }
    [System.Serializable]
    public class EquipmentStats
    {
        internal ItemCategories equipmentCategory;
        internal EquipmentRank equipmentRank;

        [ShowIf("@itemCategory == ItemCategories.Weapon")] public float attackPoints;

        public float healthPoints;
        public float manaPoints;
        public float defensePoints;
        public float attackSpeed;
        public float elementalPower;
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
            manaPoints = MathF.Floor(UnityEngine.Random.Range(minimalValue * 2, 100f) * valuesMultiplier);

            //Valores estaticos
            attackSpeed = MathF.Floor(UnityEngine.Random.Range(minimalValue * 0.15f, 100f));
            criticalRate = MathF.Floor(UnityEngine.Random.Range(0, 70));
            criticalDamage = MathF.Floor(UnityEngine.Random.Range(minimalValue * 0.25f, 200f));
        }
    }
    [System.Serializable]
    public class EquipmentAnimations
    {
        internal ItemCategories equipmentCategory;
        internal WeaponHandler weaponHandler;

        [ShowIf("@itemCategory == ItemCategories.Weapon")] public AttacksDataSO[] selectedBasicAttackClips;
        [ShowIf("@itemCategory == ItemCategories.Weapon")] public AttacksDataSO[] selectedChargedAttackClips;
        [ShowIf("@itemCategory == ItemCategories.Weapon")] public AttacksDataSO[] selectedSpecialAttackClips;
        [ShowIf("@itemCategory == ItemCategories.Weapon")] public AttacksDataSO[] selectedSkillAttackClips;
    }
}

