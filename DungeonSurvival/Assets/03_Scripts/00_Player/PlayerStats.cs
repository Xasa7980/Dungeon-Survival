using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EquipmentDataSO;

public class PlayerStats : MonoBehaviour
{
    public event EventHandler OnGetHurted;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;


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

    private float healthPoints;
    private float manaPoints;

    private float attackPoints;
    private float attackSpeed;

    private float defensePoints;

    private float criticalRate;

    private float criticalDamage;

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

    private void Awake ( )
    {
        InitializeStats();

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
        WeaponCheck();
    }
    private void WeaponCheck ( )
    {
        equipmentDataHolder_RightHand = rightWeaponHandler.transform.GetChild(0).GetComponent<EquipmentDataHolder>();
        equipmentDataHolder_LeftHand = leftWeaponHandler.transform.GetChild(0).GetComponent<EquipmentDataHolder>();

        equipmentDataSO_RightHand = equipmentDataHolder_RightHand.GetEquipmentDataSO(); //HACER UNO PARA LOS RANGES QUE NO TENDRÁN AREA DRAWER EN EL ARCO SI NO EN LA FLECHA, LA FLECHA CALCULA DISTANCIAS ONTRIGGER ENTTER
        equipmentDataSO_LeftHand = equipmentDataHolder_LeftHand.GetEquipmentDataSO();
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
    public void GetDamage ( int dmg )
    {
        int damage = CalculateDamage(dmg);
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

        GUI_Pool_Manager.Instance.CreateNumberTexts(GetDamageType(equipmentDataHolder),damage);
        monsterStats.GetDamage(damage);
    }
    public bool IsDualWeaponWielding => equipmentDataHolder_RightHand != null && equipmentDataHolder_RightHand.GetEquipmentType() != EquipmentType.Shield && 
        equipmentDataHolder_LeftHand != null && equipmentDataHolder_LeftHand.GetEquipmentType() != EquipmentType.Shield;
    public void EquipRightWeapon ( EquipmentDataHolder newEquipmentDataHolder )
    {
        prev_EquipmentDataHolder_RightHand = EquipmentDataHolder_RightHand;
        equipmentDataHolder_RightHand = newEquipmentDataHolder;

        if (equipmentDataHolder_RightHand != prev_EquipmentDataHolder_RightHand)
        {
            UpdateEquipmentStats(prev_EquipmentDataHolder_RightHand, equipmentDataHolder_RightHand, ref prev_EquipmentDataHolder_RightHand);
        }
        else return;
    }
    public void EquipLeftWeapon ( EquipmentDataHolder newEquipmentDataHolder )
    {
        prev_EquipmentDataHolder_LeftHand = equipmentDataHolder_LeftHand;
        equipmentDataHolder_LeftHand = newEquipmentDataHolder;

        if (equipmentDataHolder_LeftHand != prev_EquipmentDataHolder_RightHand)
        {
            UpdateEquipmentStats(prev_EquipmentDataHolder_LeftHand, equipmentDataHolder_LeftHand, ref prev_EquipmentDataHolder_LeftHand);
        }
    }
    private void UpdateEquipmentStats ( EquipmentDataHolder prevEquipment, EquipmentDataHolder newEquipment, ref EquipmentDataHolder currentEquipmentHolder )
    {
        if (prevEquipment != newEquipment)
        {
            RemoveStatsPoints(prevEquipment.GetEquipmentDataSO().equipmentStats);
            AddStatsPoints(newEquipment.GetEquipmentDataSO().equipmentStats);
        }
        currentEquipmentHolder = newEquipment;
    }
    private void AddStatsPoints(EquipmentStats equipmentStats )
    {
        healthPoints += equipmentStats.healthPoints;
        manaPoints += equipmentStats.manaPoints;

        attackPoints += equipmentStats.attackPoints;
        attackSpeed += equipmentStats.attackSpeed;
        defensePoints += equipmentStats.defensePoints;

        criticalRate += equipmentStats.criticalRate;
        criticalDamage += equipmentStats.criticalDamage;
    }
    private void RemoveStatsPoints(EquipmentStats equipmentStats )
    {
        healthPoints -= equipmentStats.healthPoints;
        manaPoints -= equipmentStats.manaPoints;

        attackPoints -= equipmentStats.attackPoints;
        attackSpeed -= equipmentStats.attackSpeed;
        defensePoints -= equipmentStats.defensePoints;

        criticalRate -= equipmentStats.criticalRate;
        criticalDamage -= equipmentStats.criticalDamage;
    }
    private void LevelUp ( )
    {
        if(currentExperience >= maxExperience)
        {
            curLvl = nextLevel;
            nextLevel++;
            currentExperience = 0;
            //maxExperience = CalculateNextLevelExpRequired();
        }
    }
    private float CalculateNextLevelExpRequired ( ) // Formula para subir de nivel, es equilibrada, cuanto mas nivel seas mas dificil sera pero de una manera equitativa
    {
        return (float)(4 * Mathf.Pow(curLvl, 3) + 0.8 * (Mathf.Pow(curLvl, 2) + 2) * 4);
    }

    //formula para calcular daño en base defensa, formula equilibrada
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
    private void OnChangeProgress ( )
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            healthProgressNormalized = healthPoints / maxHealthPoints,
            manaProgressNormalized = manaPoints / maxManaPoints,
        });
    }

}

