using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static EquipmentDataSO;

public class PlayerStats : MonoBehaviour, IHasProgress, iDamageable
{
    public event EventHandler OnWeaponChanged;
    public event EventHandler OnGetHurted;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public bool IsDualWeaponWielding => equipmentDataHolder_RightHand != null && equipmentDataHolder_RightHand.GetEquipmentType() != EquipmentType.Shield &&
    equipmentDataHolder_LeftHand != null && !equipmentDataHolder_LeftHand.Is2HandWeapon;

    [SerializeField] private PlayerDataSO playerDataSO;

    [Header("Battle Experience")]
    private int curLvl = 1, nextLevel = 1, lvlDif;
    private int minExpGiven = 4, maxExpGiven = 10;
    private int ExpGiven => UnityEngine.Random.Range(minExpGiven, maxExpGiven);
    private int currentExperience;
    private int maxExperience;

    private StatsDatabase statsDatabase = new StatsDatabase();
    #region StatsAtributes
    private int VIT = 1;
    private int INT = 1;
    private int STR = 1;
    private int RES = 1;
    private int DEX = 1;
    private int AGI = 1;

    private float maxHealthPoints;
    private float maxManaPoints;
    private float maxHungerPoints;

    private float healthPoints;
    private float manaPoints;
    private float hungerPoints;

    private float attackPoints;
    private float attackSpeed;

    private float defensePoints;

    private float criticalRate;

    private float criticalDamage;

    public Camera uiCamera;

    [SerializeField] private GameObject rightWeaponHandler;
    [SerializeField] private GameObject leftWeaponHandler;
    internal EquipmentDataHolder EquipmentDataHolder_LeftHand => equipmentDataHolder_LeftHand;
    private EquipmentDataHolder equipmentDataHolder_LeftHand;
    private EquipmentDataHolder prev_EquipmentDataHolder_LeftHand;
    internal EquipmentDataHolder EquipmentDataHolder_RightHand => equipmentDataHolder_RightHand;
    private EquipmentDataHolder equipmentDataHolder_RightHand;
    private EquipmentDataHolder prev_EquipmentDataHolder_RightHand;
    internal EquipmentDataSO EquipmentDataSO_RightHand => equipmentDataSO_LeftHand;
    private EquipmentDataSO equipmentDataSO_RightHand;
    internal EquipmentDataSO EquipmentDataSO_LeftHand => equipmentDataSO_LeftHand;
    private EquipmentDataSO equipmentDataSO_LeftHand;
    internal AreaDrawer LeftDetectionArea => leftDetectionArea;
    private AreaDrawer leftDetectionArea;
    internal AreaDrawer RightDetectionArea => rightDetectionArea;
    private AreaDrawer rightDetectionArea;
    #endregion
    private bool death => healthPoints <= 0 ? true : false;
    public bool noWeaponsEquiped => EquipmentDataSO_RightHand == null && equipmentDataHolder_LeftHand == null;
    private void Awake ( )
    {
        InitializeStats();
        InitializeWeaponProperties();

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            healthProgressNormalized = healthPoints / maxHealthPoints,
            manaProgressNormalized = manaPoints / maxManaPoints,
        });
    }

    private void InitializeStats ( )
    {
        VIT = playerDataSO.VIT;
        INT = playerDataSO.INT;
        STR = playerDataSO.STR;
        RES = playerDataSO.RES;
        DEX = playerDataSO.DEX;
        AGI = playerDataSO.AGI;

        maxHealthPoints = statsDatabase.Get_HpOnBaseVit(VIT) + (int)playerDataSO.extraHealthPoints;
        maxManaPoints = statsDatabase.Get_MpOnBaseInt(INT) + (int)playerDataSO.extraManaPoints;

        healthPoints = maxHealthPoints;
        manaPoints = maxManaPoints;

        attackPoints = statsDatabase.Get_AtkOnBasePower(STR) + (int)playerDataSO.extraAttackPoints;
        attackSpeed = (int)playerDataSO.extraAttackSpeed;

        criticalRate = statsDatabase.Get_CritRateOnBaseDexAndAgi(DEX, AGI);
        criticalDamage = statsDatabase.Get_CritDmgOnBaseDex(DEX);

        defensePoints = statsDatabase.Get_DefenseOnBaseRes(RES) + (int)playerDataSO.extraDefensePoints;
    }

    private void Start ( )
    {
    }
    private void InitializeWeaponProperties ( )
    {
        equipmentDataHolder_RightHand = rightWeaponHandler.transform.GetChild(0).GetComponent<EquipmentDataHolder>();
        equipmentDataHolder_LeftHand = leftWeaponHandler.transform.GetChild(0).GetComponent<EquipmentDataHolder>();

        equipmentDataSO_RightHand = equipmentDataHolder_RightHand.GetEquipmentDataSO(); //HACER UNO PARA LOS RANGES QUE NO TENDRÁN AREA DRAWER EN EL ARCO SI NO EN LA FLECHA, LA FLECHA CALCULA DISTANCIAS ONTRIGGER ENTTER
        equipmentDataSO_LeftHand = equipmentDataHolder_LeftHand.GetEquipmentDataSO();

        if(equipmentDataHolder_RightHand.GetDetectionArea() == null)
        {
            return;
        }
        rightDetectionArea = equipmentDataHolder_RightHand.GetDetectionArea();
        leftDetectionArea = equipmentDataHolder_LeftHand.GetDetectionArea();
    }
    private void Update ( )
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //GetDamage(5);
        }
    }
    public void ApplyDamage ( int dmg, bool arrowHit )
    {
        int damage = arrowHit ? CalculateDamage(Mathf.Pow(attackPoints,0.3f) * dmg) : CalculateDamage(dmg);
        healthPoints -= dmg;

        OnChangeProgress();
        OnGetHurted?.Invoke(this, EventArgs.Empty);
    }
    private DamageType GetDamageType ( EquipmentDataHolder equipmentDataHolder )
    {
        if (equipmentDataHolder.GetEquipmentElement == EquipmentElement.None)
            if (criticalRate < UnityEngine.Random.Range(0, 100))
            {
                return DamageType.CriticalDamage;
            }
        return DamageType.NormalDamage;
    }
    public void TakeDamage ( MonsterStats monsterStats, EquipmentDataHolder equipmentDataHolder )
    {
        int damage = CalculateDamage(attackPoints);

        Vector3 textPosition = monsterStats.transform.position + UnityEngine.Random.onUnitSphere * 0.15f;
        textPosition.y = 2; // Mantén la altura del jugador si solo te interesa el plano XZ.

        GUI_Pool_Manager.Instance.CreateNumberTexts(GetDamageType(equipmentDataHolder),damage, textPosition);
        monsterStats.ApplyDamage(damage, false);
    }
    private int CalculateDamage ( float damage )
    {
        /* Esta opccion es como ataque + (( ataque^2) / 10) / (defensePoints + defensePoints), lo cual el daño realizado nunca sera menor al ataque por muy alta que sea la defensa
         * Hay otra que seria calcular primero : ataque + ((ataque ^2) / 10) y luego dividir este resultado por : / (defensa * 0.25f) un cuarto de la defensa
         * pudiendo darle aun mas aleatoriedad haciendo en la defensa en el multiplicador "0.25f" un Random.Range para determinar el valor multiplicador */

        return UnityEngine.Random.Range(
                Mathf.RoundToInt(damage + (Mathf.Pow(damage, 2f)) / 10) / ((int)defensePoints + (int)defensePoints),
                Mathf.RoundToInt(damage + (Mathf.Pow(damage, 2.3f)) / 10) / ((int)defensePoints + (int)defensePoints)
            );
    }
    public void Healing ( ItemAction.FunctionType functionType, float healingAmount )
    {
        if(functionType == ItemAction.FunctionType.Healing_HP)
        {
            healthPoints = Mathf.Clamp(healthPoints, healthPoints + healingAmount, maxHealthPoints);
        }
        else if(functionType == ItemAction.FunctionType.Healing_MP)
        {
            manaPoints = Mathf.Clamp(manaPoints, manaPoints + healingAmount, maxManaPoints);
        }
    }
    public void BoostMaxHealth ( float boostingAmount, int itemQualityLevel )
    {
        BoostStat(ref maxHealthPoints, ref healthPoints, boostingAmount, itemQualityLevel);
    }

    public void BoostMaxMana ( float boostingAmount, int itemQualityLevel )
    {
        BoostStat(ref maxManaPoints, ref manaPoints, boostingAmount, itemQualityLevel);
    }

    public void BoostAttack ( float boostingAmount, int itemQualityLevel )
    {
        BoostSimpleStat(ref attackPoints, boostingAmount, itemQualityLevel);
    }

    public void BoostAttackSpeed ( float boostingAmount, int itemQualityLevel )
    {
        BoostSimpleStat(ref attackSpeed, boostingAmount, itemQualityLevel);
    }

    public void BoostDefense ( float boostingAmount, int itemQualityLevel )
    {
        BoostSimpleStat(ref defensePoints, boostingAmount, itemQualityLevel);
    }

    public void BoostCriticalRate ( float boostingAmount, int itemQualityLevel )
    {
        BoostSimpleStat(ref criticalRate, boostingAmount, itemQualityLevel);
    }

    public void BoostCriticalDamage ( float boostingAmount, int itemQualityLevel )
    {
        BoostSimpleStat(ref criticalDamage, boostingAmount, itemQualityLevel);
    }

    private void BoostStat ( ref float statToBoost, ref float currentStat, float boostingAmount, int itemQualityLevel )
    {
        float boostingMultiplier = GetBoostingMultiplier(itemQualityLevel);
        float quantityBoosted = statToBoost + (boostingAmount * boostingMultiplier);
        float delta = quantityBoosted - statToBoost;
        currentStat += delta;
        statToBoost = quantityBoosted;
    }

    private void BoostSimpleStat ( ref float stat, float boostingAmount, int itemQualityLevel )
    {
        stat += boostingAmount * GetBoostingMultiplier(itemQualityLevel);
    }

    private float GetBoostingMultiplier ( int itemQualityLevel )
    {
        return itemQualityLevel switch
        {
            0 => 1.1f,
            1 => 1.25f,
            2 => 1.4f,
            3 => 1.7f,
            _ => 1f,
        };
    }
    //public void Equip ( Item item, EquipmentDataHolder newEquipmentDataHolder )
    //{
    //    if (PlayerInventory.current.item.equipable)
    //    {
    //        PlayerInventory.current.TryRemoveItem(item);
    //        PlayerInventory_UI_Manager.current.
    //    }
    //}
    public void OnEquipRightWeapon ( EquipmentDataHolder newEquipmentDataHolder )
    {
        prev_EquipmentDataHolder_RightHand = EquipmentDataHolder_RightHand;
        equipmentDataHolder_RightHand = newEquipmentDataHolder;

        if (equipmentDataHolder_RightHand != prev_EquipmentDataHolder_RightHand)
        {
            UpdateEquipmentStats(prev_EquipmentDataHolder_RightHand, equipmentDataHolder_RightHand, ref prev_EquipmentDataHolder_RightHand);
        }
        else return;
    }
    public void OnEquipLeftWeapon ( EquipmentDataHolder newEquipmentDataHolder )
    {
        if (equipmentDataHolder_RightHand.Is2HandWeapon)
        {
            return;
        }
        prev_EquipmentDataHolder_LeftHand = equipmentDataHolder_LeftHand;
        equipmentDataHolder_LeftHand = newEquipmentDataHolder;

        if (equipmentDataHolder_LeftHand != prev_EquipmentDataHolder_RightHand)
        {
            UpdateEquipmentStats(prev_EquipmentDataHolder_LeftHand, equipmentDataHolder_LeftHand, ref prev_EquipmentDataHolder_LeftHand);
        }
    }
    private void UpdateEquipmentStats ( EquipmentDataHolder prevEquipment, EquipmentDataHolder newEquipment, ref EquipmentDataHolder currentEquipmentHolder )
    {
        float multiplier = IsDualWeaponWielding ? 1.3f : 1f;
        if(newEquipment != null)
        {
            if (prevEquipment != newEquipment)
            {
                RemoveStatsPoints(prevEquipment.GetEquipmentDataSO().equipmentStats, multiplier);
                AddStatsPoints(newEquipment.GetEquipmentDataSO().equipmentStats, multiplier);
            }
            currentEquipmentHolder = newEquipment;
        }
        else
        {
            Destroy(newEquipment.gameObject);
            if (prevEquipment != newEquipment)
            {
                RemoveStatsPoints(prevEquipment.GetEquipmentDataSO().equipmentStats, multiplier);
            }
            currentEquipmentHolder = newEquipment;
        }
        OnWeaponChanged?.Invoke(this, EventArgs.Empty);
    }
    private void AddStatsPoints(EquipmentStats equipmentStats, float multiplier )
    {
        healthPoints += equipmentStats.healthPoints * multiplier;
        manaPoints += equipmentStats.manaPoints * multiplier;

        attackPoints += equipmentStats.attackPoints * multiplier;
        attackSpeed += equipmentStats.attackSpeed * multiplier;
        defensePoints += equipmentStats.defensePoints * multiplier;

        criticalRate += equipmentStats.criticalRate * multiplier;
        criticalDamage += equipmentStats.criticalDamage * multiplier;
    }
    private void RemoveStatsPoints(EquipmentStats equipmentStats, float multiplier )
    {
        healthPoints -= equipmentStats.healthPoints * multiplier;
        manaPoints -= equipmentStats.manaPoints * multiplier;

        attackPoints -= equipmentStats.attackPoints * multiplier;
        attackSpeed -= equipmentStats.attackSpeed * multiplier;
        defensePoints -= equipmentStats.defensePoints * multiplier;

        criticalRate -= equipmentStats.criticalRate * multiplier;
        criticalDamage -= equipmentStats.criticalDamage * multiplier;
    }
    private void LevelUp ( ) /* It needs to be implemented */
    {
        if(currentExperience >= maxExperience)
        {
            curLvl = nextLevel;
            nextLevel++;
            currentExperience = 0;
            maxExperience = (int)CalculateNextLevelExpRequired();
        }
    }
    private float CalculateNextLevelExpRequired ( ) // Formula para subir de nivel, es equilibrada, cuanto mas nivel seas mas dificil sera pero de una manera equitativa
    {
        return (float)(4 * Mathf.Pow(curLvl, 3) + 0.8 * (Mathf.Pow(curLvl, 2) + 2) * 4) + 40;
    }

    private void OnChangeProgress ( )
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            healthProgressNormalized = healthPoints / maxHealthPoints,
            manaProgressNormalized = manaPoints / maxManaPoints,
        });
    }

}

