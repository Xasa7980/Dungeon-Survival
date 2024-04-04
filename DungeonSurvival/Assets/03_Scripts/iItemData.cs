using UnityEngine;

public interface iItemData
{
    bool equiped { get; }

    string displayName { get; }
    string description { get; }
    Sprite icon { get; }

    ItemTag itemTag { get; }

    bool canBeDismantled { get; }

    Item[] resultingPieces { get; }

    WorldItem interactableModel { get; }
    GameObject visualizationModel { get; }
    ItemAction[] itemFunction { get; }

    bool equipable { get; }
    bool canBeInHotbar { get; }
    bool isStackable { get; }
    int currentStack { get; }
    int currentAmount { get; set; }
    bool lookAtCursor { get; }
    float lookSpeed { get; }
    bool canMove { get; }
    WeaponType weaponType { get; }

    AnimationClip useItemAnimation { get; }
    AnimationClip continueUsingItemAnimation { get; }
    AnimationClip endUsingItemAnimation { get; }

    bool instantUse { get; }
    AnimationClip[] useAnimations { get; }
    void Use ( PlayerInventory character );

    iItemData CreateInstance ( );

    void Reconfigure ( NewItemData data );

    GameObject CreateVisualizationObject ( );

    void Equip ( );

    void Unequip ( );
}
public struct NewItemData
{
    public string displayName;
    public string description;
    public Sprite icon;

    public ItemTag itemTag { get; }

    public bool canBeDismantled;

    public Item[] resultingPieces;

    public WorldItem inWorldObject;
    public GameObject presentationObject;

    public bool canBeInHotbar;
    public bool isStackable;
    public int stackAmount;

    public bool equipable;
    public bool lookAtCursor;
    public float lookSpeed;
    public bool canMove;
    public WeaponType weaponType;

    AnimationClip useItemAnimation;
    AnimationClip continueUsingItemAnimation;
    AnimationClip endUsingItemAnimation;
    public AnimationClip[] useAnimations;

    /// <summary>
    /// Usar proyectiles
    /// </summary>
    /// When using  <param name="item"> Items como flechas </param>
    //public Proyectile proyectile;

    public AnimationClip reloadAnimation;
}
