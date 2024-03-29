using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "Dungeon Survival/Inventory/Item")]
public class Item : ScriptableObject, iItemData
{
    #region PrivateParams

    [Header("Main Options")]

    [SerializeField] string _displayName;
    [SerializeField] string _description;
    [SerializeField] bool _canBeInHotbar;
    [SerializeField] bool _isStackable;

    [SerializeField] Sprite _icon;
    [ShowIf("_isStackable"), SerializeField] int _stackAmount = 3;
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

    [FoldoutGroup("Equip"), SerializeField,ShowIf("equipable")] EquipmentDataSO _equipmentDataSO;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("showIfIsShield")] float _protectionAngle;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("showIfNotArmor")] bool _lookAtCursor;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("showLookSpeedField")] float _lookSpeed = 3;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("showIfNotArmor")] bool _canMove = true;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("showIfNotArmor")] protected AnimationClip _useAnimation;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("showIfNotArmor")] protected AnimationClip _useContinuousAnimation;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("showIfNotArmor")] protected AnimationClip _useEndAnimation;
    //[SerializeField, ShowIf("isRangeWeapon")] Proyectile _proyectile;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("isRangeWeapon")] bool hasRealoadAnimation;
    [FoldoutGroup("Equip"), SerializeField, ShowIf("showReloadField")] AnimationClip _reloadAnimation;

    [FoldoutGroup("Equip"), SerializeField] bool _autoEquip;

    [Header("Dismantle")]
    
    [FoldoutGroup("Dismantle"),SerializeField] bool _canBeDismantled;
    [FoldoutGroup("Dismantle"),SerializeField, ShowIf("_canBeDismantled")] Item[] _resultingPieces = new Item[0];

    [Header("World Options")]
    
    [FoldoutGroup("WorldOptions"),SerializeField] WorldItem _interactableModel;
    [FoldoutGroup("WorldOptions"), SerializeField] GameObject _visualizationModel;
    
    [Header("Item Use Properties")]

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
    public int stackAmount => _stackAmount;
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

    public virtual void EquipOnPlayer(Transform holster)
    {
        Instantiate(visualizationModel, holster);
    }

    public virtual void UnequipOnPlayer(iInventory inventory)
    {
        if (inventory.TryAddItem(this, out int index))
        {

        }
    }

    public virtual void UseItem()
    {
        //do something
    }

    public void Use ( PlayerInventory character )
    {
        throw new NotImplementedException();
    }

    iItemData iItemData.CreateInstance ( )
    {
        throw new NotImplementedException();
    }

    public void Reconfigure ( NewItemData data )
    {
        throw new NotImplementedException();
    }

    public GameObject CreateVisualizationObject ( )
    {
        throw new NotImplementedException();
    }

    public void Equip ( /*PlayerStats player, EquipmentDataSO equipmentDataSO*/)
    {
        //player.OnEquipRightWeapon
    }

    public void Unequip ( )
    {
        throw new NotImplementedException();
    }

    public ItemAction itemAction;
}
public class ActionResult<T>
{
    public bool Success { get; set; }
    public T Value { get; set; }
    public string Message { get; set; } // opcional

    public ActionResult ( bool success, T value, string message = "" )
    {
        Success = success;
        Value = value;
        Message = message;
    }
}