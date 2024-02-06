using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "NewEquipmentLibrary", menuName = "Dungeon Survival/Equipment/Library")]
public class EquipmentLibrarySO : ScriptableObject
{
#pragma warning disable IDE0052 // Quitar miembros privados no leídos
    private EquipmentType equipmentType;
#pragma warning restore IDE0052 // Quitar miembros privados no leídos

    [SerializeField, ShowIf("@equipmentType == EquipmentType.Axe")] List<EquipmentPart> axe = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Bow")] List<EquipmentPart> bow = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Hammer")] List<EquipmentPart> hammer = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Sword")] List<EquipmentPart> sword = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Shield")] List<EquipmentPart> shield = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Spear")] List<EquipmentPart> spear = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Staff")] List<EquipmentPart> staff = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Bat")] List<EquipmentPart> bat = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Dagger")] List<EquipmentPart> dagger = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Helmet")] List<EquipmentPart> helmet = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Chest")] List<EquipmentPart> chest = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Gauntlets")] List<EquipmentPart> gauntlets = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Leggins")] List<EquipmentPart> leggins = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Boots")] List<EquipmentPart> boots = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Necklace")] List<EquipmentPart> necklace = new List<EquipmentPart>();
    [SerializeField, ShowIf("@equipmentType == EquipmentType.Ring")] List<EquipmentPart> ring = new List<EquipmentPart>();

    private List<EquipmentPart> tempList = new List<EquipmentPart>();
    public List<GameObject> equipmentList = new List<GameObject>();

    #region EditorButtons

    [PropertySpace(SpaceBefore = 5)]

    [PropertyOrder(-5)]
    [HorizontalGroup("Toolbar")]
    [Button("Axe")]
    void SetAxe1HeadType ( )
        {
            equipmentType = EquipmentType.Axe;
            tempList = axe;
        }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-5)]
    [HorizontalGroup("Toolbar")]
    [Button("Bow")]
    void SetAxe2HeadType ( )
    {
        equipmentType = EquipmentType.Bow;
        tempList = bow;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-5)]
    [HorizontalGroup("Toolbar")]
    [Button("Hammer")]
    void SetHammerType ( )
    {
        equipmentType = EquipmentType.Hammer;
        tempList = hammer;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-5)]
    [HorizontalGroup("Toolbar")]
    [Button("Sword")]
    void SetSwordType ( )
    {
        equipmentType = EquipmentType.Sword;
        tempList = sword;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-4)]
    [HorizontalGroup("Toolbar2")]
    [Button("Shield")]
    void SetShieldType ( )
    {
        equipmentType = EquipmentType.Shield;
        tempList = shield;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-4)]
    [HorizontalGroup("Toolbar2")]
    [Button("Spear")]
    void SetSpearType ( )
    {
        equipmentType = EquipmentType.Spear;
        tempList = spear;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-4)]
    [HorizontalGroup("Toolbar2")]
    [Button("Staff")]
    void SetStaffType ( )
    {
        equipmentType = EquipmentType.Staff;
        tempList = staff;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-4)]
    [HorizontalGroup("Toolbar2")]
    [Button("Bat")]
    void SetBatType ( )
    {
        equipmentType = EquipmentType.Bat;
        tempList = bat;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-3)]
    [HorizontalGroup("Toolbar3")]
    [Button("Dagger")]
    void SetDaggerType ( )
    {
        equipmentType = EquipmentType.Dagger;
        tempList = dagger;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-3)]
    [HorizontalGroup("Toolbar3")]
    [Button("Helmet")]
    void SetHelmetType ( )
    {
        equipmentType = EquipmentType.Helmet;
        tempList = helmet;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-3)]
    [HorizontalGroup("Toolbar3")]
    [Button("Chest")]
    void SetChestType ( )
    {
        equipmentType = EquipmentType.Chest;
        tempList = chest;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-3)]
    [HorizontalGroup("Toolbar3")]
    [Button("Gauntlets")]
    void SetGauntletsType ( )
    {
        equipmentType = EquipmentType.Gauntlets;
        tempList = gauntlets;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar4")]
    [Button("Leggins")]
    void SetLegginsType ( )
    {
        equipmentType = EquipmentType.Leggins;
        tempList = leggins;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar4")]
    [Button("Boots")]
    void SetBootsType ( )
    {
        equipmentType = EquipmentType.Boots;
        tempList = boots;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar4")]
    [Button("Necklace")]
    void SetNecklaceType ( )
    {
        equipmentType = EquipmentType.Necklace;
        tempList = necklace;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar4")]
    [Button("Ring")]
    void SetRingType ( )
    {
        equipmentType = EquipmentType.Ring;
        tempList = ring;
    }

    [PropertySpace(SpaceBefore = 5, SpaceAfter = 5)]

    [GUIColor(0.97f, 0.41f, 0.25f, 1f)]
    [PropertyOrder(5)]
    [HorizontalGroup("Toolbar5")]
    [Button("ResetEquipmentList")]
    void ResetEquipmentList ( ) => equipmentList.Clear();

    [GUIColor(0.97f, 0.41f, 0.25f, 1f)]
    [PropertyOrder(6)]
    [HorizontalGroup("Toolbar6")]
    [Button("ResetCurrentEquipmentWindows")]
    void ResetCurrentEquipmentWindows ( )
    {
        Debug.Log(tempList[0]);
        tempList[0].parts.Clear();
    }

    #endregion

    private void OnValidate ( )
    {
        AddEquipmentClassifiedInList();
    }

    [OnValueChanged("SortEquipment")]
    private void AddEquipmentClassifiedInList ( )
    {
        axe[0].parts.Clear();
        bow[0].parts.Clear();
        hammer[0].parts.Clear();
        sword[0].parts.Clear();
        shield[0].parts.Clear();
        spear[0].parts.Clear();
        staff[0].parts.Clear();
        bat[0].parts.Clear();
        dagger[0].parts.Clear();
        helmet[0].parts.Clear();
        chest[0].parts.Clear();
        gauntlets[0].parts.Clear();
        leggins[0].parts.Clear();
        boots[0].parts.Clear();
        necklace[0].parts.Clear();
        ring[0].parts.Clear();

        foreach (GameObject item in equipmentList)
        {
            EquipmentType type = item.GetComponent<EquipmentDataHolder>().GetEquipmentType();
            switch (type)
            {
                case EquipmentType.Axe:

                    if (axe[0].parts.Contains(item)) continue;
                    axe[0].parts.Add(item);

                    break;
                case EquipmentType.Bow:

                    if (bow[0].parts.Contains(item)) continue;
                    bow[0].parts.Add(item);

                    break;
                case EquipmentType.Hammer:

                    if (hammer[0].parts.Contains(item)) continue;
                    hammer[0].parts.Add(item);

                    break;
                case EquipmentType.Sword:

                    if (sword[0].parts.Contains(item)) continue;
                    sword[0].parts.Add(item);

                    break;
                case EquipmentType.Shield:

                    if (shield[0].parts.Contains(item)) continue;
                    shield[0].parts.Add(item);

                    break;
                case EquipmentType.Spear:

                    if (spear[0].parts.Contains(item)) continue;
                    spear[0].parts.Add(item);

                    break;
                case EquipmentType.Staff:

                    if (staff[0].parts.Contains(item)) continue;
                    staff[0].parts.Add(item);

                    break;
                case EquipmentType.Bat:

                    if (bat[0].parts.Contains(item)) continue;
                    bat[0].parts.Add(item);

                    break;
                case EquipmentType.Dagger:

                    if (dagger[0].parts.Contains(item)) continue;
                    dagger[0].parts.Add(item);

                    break;
                case EquipmentType.Helmet:

                    if (helmet[0].parts.Contains(item)) continue;
                    helmet[0].parts.Add(item);

                    break;
                case EquipmentType.Chest:

                    if (chest[0].parts.Contains(item)) continue;
                    chest[0].parts.Add(item);

                    break;
                case EquipmentType.Gauntlets:

                    if (gauntlets[0].parts.Contains(item)) continue;
                    gauntlets[0].parts.Add(item);

                    break;
                case EquipmentType.Leggins:

                    if (leggins[0].parts.Contains(item)) continue;
                    leggins[0].parts.Add(item);

                    break;
                case EquipmentType.Boots:

                    if (boots[0].parts.Contains(item)) continue;
                    boots[0].parts.Add(item);

                    break;
                case EquipmentType.Necklace:

                    if (necklace[0].parts.Contains(item)) continue;
                    necklace[0].parts.Add(item);

                    break;
                case EquipmentType.Ring:

                    if (ring[0].parts.Contains(item)) continue;
                    ring[0].parts.Add(item);

                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    class EquipmentPart
    {
        [FoldoutGroup("@name")]
        [PropertyOrder(-2)]
        [SerializeField] string name;

        [SerializeField, HideInInspector] bool disabled;

        //[FoldoutGroup("@name")]
        [PropertyOrder(-1)]
        [Button("@disabled?\"Enable\":\"Disable\"", ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        public void Toggle ( )
        {
            disabled = !disabled;
            if (currentPart) currentPart.SetActive(!disabled);
        }

        [FoldoutGroup("@name")]
        [ListDrawerSettings(IsReadOnly = true)]
        public List<GameObject> parts;

        [FoldoutGroup("@name")]
        [SerializeField, ReadOnly] GameObject currentPart;
        [SerializeField, ReadOnly] EquipmentType currentPartType;
        [SerializeField, HideInInspector] int currentPartIndex = 0;

        [HideIf("disabled"), Button(ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        void Previous ( )
        {
            if (disabled) return;

            if (currentPart) currentPart.SetActive(false);

            if (currentPartIndex > 0)
                currentPartIndex--;
            else
                currentPartIndex = parts.Count - 1;

            currentPart = parts[currentPartIndex];
            currentPart.SetActive(true);
        }

        [HideIf("disabled"), Button(ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        void Next ( )
        {
            if (disabled) return;

            if (currentPart) currentPart.SetActive(false);

            if (currentPartIndex < parts.Count - 1)
                currentPartIndex++;
            else
                currentPartIndex = 0;

            currentPart = parts[currentPartIndex];
            currentPart.SetActive(true);
        }
    }
}

