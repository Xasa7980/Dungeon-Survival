using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System;
using System.Linq;
using System.Collections.Concurrent;

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

    [GUIColor(0.15f, 0.8f, 0.8f, 1f),SerializeField] string _displayName;
    [GUIColor(0.15f, 0.8f, 0.8f, 1f),TextArea,SerializeField] string _description;
    [GUIColor(0.15f, 0.8f, 0.8f, 1f),SerializeField] int _itemQualityLevel;
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

    [ShowIf("_isStackable"), SerializeField] int _maxStack = 3;
    [SerializeField, DisableIf("isNotBase")] ItemTag _itemTag;

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
    [FoldoutGroup("EquipUI"), SerializeField,ShowIf("equipable")] int _durability = 1;
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
    [SerializeField] private bool multipleAction;
    [SerializeField, ShowIf("@!multipleAction")] private ItemAction itemAction;
    [SerializeField, ShowIf("@multipleAction")] ItemAction[] _itemActions;
    [SerializeField, HideIf("_instantUse")] protected AnimationClip[] _useAnimations;


    #endregion

    #region PublicParams
    public string displayName { get; set; }
    public string description { get; set; }
    public int itemQualityLevel => _itemQualityLevel;
    public bool autoEquip => _autoEquip;
    public bool equiped { get; private set; }
    public bool equipable { get { return _equipable; } set { _equipable = value; } }
    public int durability => durability;
    public bool canBeInHotbar { get { return _canBeInHotbar; } set { _canBeInHotbar = value; } }
    public bool isStackable { get { return _isStackable; } set { _isStackable = value; } }
    public int maxStack { get { return _maxStack; } set { _maxStack = value; } }
    public int currentAmount { get; set; }
    public bool lookAtCursor { get { return _lookAtCursor; } set { _lookAtCursor = value; } }
    public float lookSpeed { get { return _lookSpeed; } set { _lookSpeed = value; } }
    public bool canMove { get { return _canMove; } set { _canMove = value; } }
    public bool instantUse => _instantUse;
    public bool canBeDismantled { get; set; }

    public Sprite icon { get { return _icon; } set { _icon = value; } }
    public ItemTag itemTag { get { return _itemTag; } set { _itemTag = value; } }
    public Item[] resultingPieces { get { return _resultingPieces; } set { _resultingPieces = value; } }
    public WorldItem interactableModel => _interactableModel;
    public GameObject visualizationModel => _visualizationModel;
    public WeaponType weaponType { get { return _equipmentDataSO.weaponType; } set { _equipmentDataSO.weaponType = value; } }
    public EquipmentDataSO equipmentDataSO => _equipmentDataSO;
    public AnimationClip useItemAnimation => _useAnimation;
    public AnimationClip continueUsingItemAnimation => _useContinuousAnimation;
    public AnimationClip endUsingItemAnimation => _useEndAnimation;
    public AnimationClip[] useAnimations { get { return _useAnimations; } set { _useAnimations = value; } }
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
    public void Reconfigure ( Item data )
    {
        this._displayName = data.displayName;
        this._description = data.description;
        this._icon = data.icon;
        this._itemTag = data.itemTag;

        this._canBeDismantled = data.canBeDismantled;
        this._resultingPieces = data.resultingPieces;

        this._canBeInHotbar = data.canBeInHotbar;
        this._isStackable = data.isStackable;
        this._maxStack = data.maxStack;

        this._equipable = data.equipable;
        this._lookAtCursor = data.lookAtCursor;
        this._lookSpeed = data.lookSpeed;
        this._canMove = data.canMove;
        if(this.equipable) this.equipmentDataSO.weaponType = data.weaponType;
        //this._useItemAnimation = data.idleAnimation;
        //this._walkAnimation = data.walkAnimation;
        //this._runAnimation = data.runAnimation;
        this._useAnimations = data.useAnimations;
        //this._proyectile = data.proyectile;
        this._reloadAnimation = data._reloadAnimation;

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
        if (multipleAction)
        {
            itemAction = null;
            return false;
        }
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
    public bool TryGetMultipleActions(out ItemAction[] itemActions )
    {
        if(!multipleAction)
        {
            itemActions = null;
            return false;
        }
        if (this._itemActions.Length > 0)
        {
            itemActions = this._itemActions;
            return true;
        }
        else
        {
            itemActions = null;
            return false;
        }
    }
}
