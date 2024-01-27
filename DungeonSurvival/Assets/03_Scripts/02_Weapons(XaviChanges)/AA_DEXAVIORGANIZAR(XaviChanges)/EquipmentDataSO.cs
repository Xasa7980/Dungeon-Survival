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
    Wind
}
public enum EquipmentType
{
    Axe1Head,
    Axe2Head,
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

    [GUIColor(0.3f, 1f, 0.5f, 1f), PropertySpace(SpaceBefore = 10, SpaceAfter = 10), FoldoutGroup("Main Stats")] 
    public EquipmentStats equipmentStats;

    [GUIColor(0.3f, 1f, 0.8f, 1f), PropertySpace(SpaceBefore = 10, SpaceAfter = 10), FoldoutGroup("Visual Options")] 
    public EquipmentVisuals equipmentVisuals;

    private string EquipmentInformation => equipmentType.ToString();
    private void SetEquipmentCategoryToStats() => equipmentStats.equipmentCategory = equipmentCategory;
    private void SetEquipmentRankToStats() => equipmentStats.equipmentRank = equipmentRank;
    private void SetEquipmentCategoryToVisuals ( ) => equipmentVisuals.equipmentCategory = equipmentCategory;
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

                equipmentVisuals.SetWeaponSlashMaterial(equipmentVisuals.slashMaterials[0]);

                equipmentVisuals.colorKey_1 = new Color(1, 1, 1, 1);
                equipmentVisuals.colorKey_2 = new Color(1, 1, 1, 1);
                break;
            case EquipmentElement.Water:

                equipmentVisuals.SetWeaponSlashMaterial(equipmentVisuals.slashMaterials[0]);

                equipmentVisuals.colorKey_1 = new Color(0, 0.372566968f, 7.9066987f, 1);
                equipmentVisuals.colorKey_2 = new Color(0, 0.826963913f, 5.34031343f, 1);
                break;

            case EquipmentElement.Fire:

                equipmentVisuals.SetWeaponSlashMaterial(equipmentVisuals.slashMaterials[1]);

                equipmentVisuals.colorKey_1 = new Color(1.84430265f, 0.215726241f, 0, 1);
                equipmentVisuals.colorKey_2 = new Color(7.12973881f, 0.359262943f, 0, 1);
                break;

            case EquipmentElement.Darkness:

                equipmentVisuals.SetWeaponSlashMaterial(equipmentVisuals.slashMaterials[2]);

                equipmentVisuals.colorKey_1 = new Color(0.223678052f, 0, 1.78942442f, 1);
                equipmentVisuals.colorKey_2 = new Color(0.376470596f, 0, 2.50980401f, 1);
                break;
            case EquipmentElement.Light:

                equipmentVisuals.SetWeaponSlashMaterial(equipmentVisuals.slashMaterials[3]);

                equipmentVisuals.colorKey_1 = new Color(8, 8, 8, 1);
                equipmentVisuals.colorKey_2 = new Color(10.6806269f, 10.6806269f, 10.6806269f, 1);
                break;

            case EquipmentElement.Thunder:

                equipmentVisuals.SetWeaponSlashMaterial(equipmentVisuals.slashMaterials[4]);

                equipmentVisuals.colorKey_1 = new Color(0, 0.47269088f, 7.12973881f, 1);
                equipmentVisuals.colorKey_2 = new Color(0.272053272f, 3.51327181f, 5.34031343f, 1);
                break;

            case EquipmentElement.Earth:

                equipmentVisuals.SetWeaponSlashMaterial(equipmentVisuals.slashMaterials[5]);

                equipmentVisuals.colorKey_1 = new Color(1.06666672f, 0.313725501f, 0, 1);
                equipmentVisuals.colorKey_2 = new Color(4, 2.13333344f, 0, 1);
                break;

            case EquipmentElement.Wind:

                equipmentVisuals.SetWeaponSlashMaterial(equipmentVisuals.slashMaterials[6]);

                equipmentVisuals.colorKey_1 = new Color(0, 0.335925996f, 3.99999952f, 1);
                equipmentVisuals.colorKey_2 = new Color(0.335078508f, 2.80628252f, 3.99999952f, 1);
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
        if (equipmentVisuals.equipmentParticleEffects != null) equipmentVisuals.hasSlashEffect = enable;
        
        if(equipmentVisuals.equipmentParticleEffects != null) equipmentVisuals.equipmentParticleEffects.SetActive( enable );
        
        if (equipmentVisuals.hasSecondaryParticleEffect)
        {
            if (equipmentVisuals.equipmentSecondaryParticleEffect != null) equipmentVisuals.equipmentSecondaryParticleEffect.SetActive( enable );
        }
    }
    public void SetGradientWithColors ( Gradient gradient )
    {
        gradient.mode = GradientMode.Blend;

        gradient.colorKeys[0].color = equipmentVisuals.colorKey_1;
        gradient.colorKeys[1].color = equipmentVisuals.colorKey_2;
    }
    public void SetEquipment_SlashEffectMaterial ( ParticleSystemRenderer particleSystemRenderer )
    {
        particleSystemRenderer.material = equipmentVisuals.weaponSlashEffectMaterial;
    }

    [System.Serializable]
    public class EquipmentVisuals
    {
        internal EquipmentCategory equipmentCategory;

        [InfoBox("This particle effect only is enabled when Equipment rank is high and the equipment has particles", InfoMessageType.Warning)]
        public GameObject equipmentParticleEffects;
        [InfoBox("This particle effect only is enabled when Equipment rank is high and the equipment has secondary particles",InfoMessageType.Warning)]
        public GameObject equipmentSecondaryParticleEffect;

        public Color colorKey_1;
        public Color colorKey_2;

        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public bool hasSlashEffect;
        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public bool hasSecondaryParticleEffect;

        [InfoBox("It requires an specific order on putting inside the materials, check EquipmentElement list", InfoMessageType.Warning)]
        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public Material[] slashMaterials;
        [ShowIf("@equipmentCategory == EquipmentCategory.Weapon")] public Material weaponSlashEffectMaterial;

        public void SetWeaponSlashMaterial ( Material material ) => weaponSlashEffectMaterial = material;
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

