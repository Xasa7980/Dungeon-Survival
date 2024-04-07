using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/item")]
public class Item : ScriptableObject, iItemData
{
    public enum IconType
    {
        Normal,
        NormalInverted,
        Diagonal,
        DiagonalInverted
    }
    #region PrivateParams

    [Header("Main Options")]

    [SerializeField] string _displayName;
    [SerializeField] string _description;
    [SerializeField] bool _canBeInHotbar;
    [SerializeField] bool _isStackable;

    [SerializeField] Sprite _icon;
    [SerializeField] IconType _iconType;
    public float _iconRotationZ => _iconType == IconType.Normal ? 0 : 
        _iconType == IconType.NormalInverted ? 0 :
        _iconType == IconType.Diagonal ? 135.2f : -135.2f; //Primero se hace en editor para saber que valor poner despues de ":"
    public float _iconScale => _iconType == IconType.Normal ? 1 : 
        _iconType == IconType.NormalInverted ? -1 :
        _iconType == IconType.Diagonal ? 1.5f : -1.5f; //Primero se hace en editor para saber que valor poner despues de ":"

    [ShowIf("_isStackable"), SerializeField] int _stackAmount = 3;
    [SerializeField, DisableIf("isNotBase")] ItemTag _itemTag;
    private ItemAction itemAction;

    #region Verificables
    bool isRangeWeapon { get => equipable && weaponType == WeaponType.Range; }
    bool showReloadField { get => isRangeWeapon && hasRealoadAnimation; }
    bool showLookSpeedField { get => _lookAtCursor && _equipable; }
    bool showIfNotArmor { get => equipable && weaponType != WeaponType.Armor; }
    bool showIfIsShield { get => equipable && weaponType == WeaponType.Shield; }
    bool isNotBase { get => GetType() != typeof(Item); }

    #endregion

    [Header("Equiping")]
    [SerializeField] bool _equipable;

    [FoldoutGroup("EquipUI"), SerializeField,ShowIf("equipable")] EquipmentDataSO _equipmentDataSO;
    [FoldoutGroup("EquipUI"), SerializeField,ShowIf("equipable")] ItemCategory itemCategory;
    [FoldoutGroup("EquipUI"), SerializeField, ShowIf("showIfIsShield")] float _protectionAngle;
    [FoldoutGroup("EquipUI"), SerializeField, ShowIf("showIfNotArmor")] bool _lookAtCursor;
    [FoldoutGroup("EquipUI"), SerializeField, ShowIf("showLookSpeedField")] float _lookSpeed = 3;
    [FoldoutGroup("EquipUI"), SerializeField, ShowIf("showIfNotArmor")] bool _canMove = true;
    [FoldoutGroup("EquipUI"), SerializeField, ShowIf("showIfNotArmor")] protected AnimationClip _useAnimation;
    [FoldoutGroup("EquipUI"), SerializeField, ShowIf("showIfNotArmor")] protected AnimationClip _useContinuousAnimation;
    [FoldoutGroup("EquipUI"), SerializeField, ShowIf("showIfNotArmor")] protected AnimationClip _useEndAnimation;
    //[SerializeField, ShowIf("isRangeWeapon")] Proyectile _proyectile;
    [FoldoutGroup("EquipUI"), SerializeField, ShowIf("isRangeWeapon")] bool hasRealoadAnimation;
    [FoldoutGroup("EquipUI"), SerializeField, ShowIf("showReloadField")] AnimationClip _reloadAnimation;

    [FoldoutGroup("EquipUI"), SerializeField] bool _autoEquip;

    [Header("Dismantle")]
    
    [FoldoutGroup("Dismantle"),SerializeField] bool _canBeDismantled;
    [FoldoutGroup("Dismantle"),SerializeField, ShowIf("_canBeDismantled")] Item[] _resultingPieces = new Item[0];

    [Header("World Options")]
    
    [FoldoutGroup("WorldOptions"),SerializeField] WorldItem _interactableModel;
    [FoldoutGroup("WorldOptions"), SerializeField] GameObject _visualizationModel;
    
    [Header("item Use Properties")]

    [SerializeField] bool _instantUse = false;
    [SerializeField] ItemAction[] _itemFunction;
    [SerializeField, HideIf("_instantUse")] protected AnimationClip[] _useAnimations;


    #endregion

    #region PublicParams
    public string displayName => _displayName;
    public string description => _description;
    public bool autoEquip => _autoEquip;
    public bool equiped { get; private set; }
    public bool equipable => _equipable;
    public bool canBeInHotbar => _canBeInHotbar;
    public bool isStackable => _isStackable;
    public int currentStack => _stackAmount;
    public int currentAmount { get; set; }
    public bool lookAtCursor => _lookAtCursor;
    public float lookSpeed => _lookSpeed;
    public bool canMove => _canMove;
    public bool instantUse => _instantUse;
    public bool canBeDismantled => _canBeDismantled;

    public Sprite icon => _icon;
    public ItemTag itemTag => _itemTag;
    public Item[] resultingPieces => _resultingPieces;
    public WorldItem interactableModel => _interactableModel;
    public GameObject visualizationModel => _visualizationModel;
    public ItemAction[] itemFunction => _itemFunction;
    public WeaponType weaponType => _equipmentDataSO.weaponType;
    public EquipmentDataSO equipmentDataSO => _equipmentDataSO;
    public AnimationClip useItemAnimation => _useAnimation;
    public AnimationClip continueUsingItemAnimation => _useContinuousAnimation;
    public AnimationClip endUsingItemAnimation => _useEndAnimation;
    public AnimationClip[] useAnimations => _useAnimations;
    #endregion
    /// <summary>
    /// Crea una instancia del item para modificarlo y no afectar el Asset
    /// </summary>
    /// <returns>Instancia del item</returns>
    public Item CreateInstance()
    {
        return Instantiate(this);
    }

    public WorldItem InstantiateInWorld(Vector3 position)
    {
        return Instantiate(interactableModel, position, Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0, 360)));
    }

    public WorldItem InstantiateInWorld(Vector3 position, Item item)
    {
        WorldItem instance = Instantiate(interactableModel, position, Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0, 360)));
        instance.ChangeItemReference(item);
        return instance;
    }

    public virtual void EquipVisuals(Transform holster)
    {
        Instantiate(visualizationModel, holster);
    }

    public virtual void UnequipVisuals(iInventory inventory)
    {
        if (inventory.TryAddItem(this, out int index))
        {

        }
    }

    public virtual void UseItem()
    {
        //do something
    }

    public void Use ( Transform character )
    {
        //if (!instantUse)
        //{
        //    ((AnimatorOverrideController)character.anim.runtimeAnimatorController)["Use_Item"] = useAnimations[Random.Range(0, useAnimations.Length)];
        //    character.GetComponent<PlayerCombat>.SetTrigger("UseItem");
        //} En caso de haber una animacion para usar items (todavia no)
    }
    public void Equip ( /*PlayerStats player, EquipmentDataSO equipmentDataSO*/)
    {
        Debug.LogError("Misses to implement this function");
        equiped = true;
        //player.OnEquipRightWeapon
    }

    public void Unequip ( )
    {
        equiped = false;
        throw new NotImplementedException();
    }

    public bool TryGetAction ( out ItemAction itemAction )
    {
        if (this.itemAction != null)
        {
            itemAction = this.itemAction;
            return true;
        }
        else
        {
            itemAction = null;
            return false;
        }
    }
}
