using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EquipmentDataSO;

public class MonsterStats : MonoBehaviour,IHasProgress, iDamageable
{
    public event EventHandler OnWeaponChanged;
    public event EventHandler OnGetHurted;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public MonsterRank monsterRank => monsterDataSO.monsterRank;
    public bool IsDualWeaponWielding => equipmentDataHolder_RightHand != null && equipmentDataHolder_RightHand.GetEquipmentType() != EquipmentType.Shield &&
                equipmentDataHolder_LeftHand != null && equipmentDataHolder_LeftHand.GetEquipmentType() >= EquipmentType.Dagger;

    [Header("Battle Experience")]

    public int curLvl = 1, playerLvl = 1 /* Hare que quien tenga nivel sea el mundo, lo que haga que la dificultad aumente respecto al nivel del personaje, los bosses se quedan talcual */ , lvlDif;
    public int minExpGiven = 4, maxExpGiven = 10;
    public int ExpGiven => UnityEngine.Random.Range(minExpGiven, maxExpGiven);

    [SerializeField] private MonsterDataSO monsterDataSO;

    private StatsDatabase statsDatabase = new StatsDatabase();
    private AI_MainCore aI_MainCore;
    
    #region StatsAtributes
    private int VIT = 1;
    private int INT = 1;
    private int STR = 1;
    private int AGI = 1;
    private int DEX = 1;
    private int RES = 1;

    private float maxHealthPoints;
    private float maxManaPoints;

    private float healthPoints;
    private float manaPoints;

    private float attackPoints;
    private float attackSpeed;

    private float defensePoints;

    private float criticalRate;
    private float criticalDamage;
    #endregion
    
    [SerializeField] private Transform rightWeaponHandler;
    [SerializeField] private Transform leftWeaponHandler;

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

    private bool Death => healthPoints <= 0? true: false;

    public List<AttacksDataSO> AttacksDataSOs => attacksDataSOs;
    [SerializeField] private List<AttacksDataSO> attacksDataSOs = new List<AttacksDataSO>();
    public Dictionary<AttacksDataSO, float> AttackTimers => attackTimers;
    private Dictionary<AttacksDataSO, float> attackTimers = new Dictionary<AttacksDataSO, float>();
    
    private void Awake ( )
    {
        aI_MainCore = GetComponent<AI_MainCore>();
        InitializeStats();
        InitializeWeaponValues();

    }
    private void InitializeStats ( )
    {
        VIT = monsterDataSO.VIT;
        INT = monsterDataSO.INT;
        STR = monsterDataSO.STR;
        RES = monsterDataSO.RES;
        DEX = monsterDataSO.DEX;
        AGI = monsterDataSO.AGI;

        maxHealthPoints = statsDatabase.Get_HpOnBaseVit(VIT) + (int)monsterDataSO.extraHealthPoints;
        maxManaPoints = statsDatabase.Get_MpOnBaseInt(INT) + (int)monsterDataSO.extraManaPoints;

        healthPoints = maxHealthPoints;
        manaPoints = maxManaPoints;

        attackPoints = statsDatabase.Get_AtkOnBasePower(STR) + (int)monsterDataSO.extraAttackPoints;
        attackSpeed = (int)monsterDataSO.extraAttackSpeed;

        criticalRate = statsDatabase.Get_CritRateOnBaseDexAndAgi(DEX, AGI);
        criticalDamage = statsDatabase.Get_CritDmgOnBaseDex(DEX);

        defensePoints = statsDatabase.Get_DefenseOnBaseRes(RES) + (int)monsterDataSO.extraDefensePoints;
    }

    private void Start ( )
    {
        foreach (var attackDataSO in attacksDataSOs)
        {
            attackTimers[attackDataSO] = 0;
        }
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            healthProgressNormalized = healthPoints / maxHealthPoints,
            manaProgressNormalized = manaPoints / maxManaPoints,
        });
    }
    private void InitializeWeaponValues ( )
    {
        if (leftWeaponHandler == null)
        {
            Debug.LogError("leftWeaponHandler no ha sido asignado en MonsterStats.");
            return;
        }
        if (rightWeaponHandler == null)
        {
            Debug.LogError("rightWeaponHandler no ha sido asignado en MonsterStats.");
            return;
        }

        equipmentDataHolder_RightHand = rightWeaponHandler.GetChild(0).GetComponent<EquipmentDataHolder>();
        equipmentDataHolder_LeftHand = leftWeaponHandler.GetChild(0).GetComponent<EquipmentDataHolder>();

        if(equipmentDataHolder_RightHand != null)
        {
            equipmentDataSO_RightHand = equipmentDataHolder_RightHand.GetEquipmentDataSO(); //HACER UNO PARA LOS RANGES QUE NO TENDRÁN AREA DRAWER EN EL ARCO SI NO EN LA FLECHA, LA FLECHA CALCULA DISTANCIAS ONTRIGGER ENTTER
            rightDetectionArea = equipmentDataHolder_RightHand.GetDetectionArea();
        }
        if (equipmentDataHolder_LeftHand != null)
        {
            equipmentDataSO_LeftHand = equipmentDataHolder_LeftHand.GetEquipmentDataSO();
            leftDetectionArea = equipmentDataHolder_LeftHand.GetDetectionArea();
        }
    }
    private void Update ( )
    {
        if (Death)
        {
            aI_MainCore.SetState(State.Death);
        }
    }
    public void ApplyDamage ( int dmg)
    {
        //int damage = arrowHit ? CalculateDamage(Mathf.Pow(attackPoints, 0.3f) * dmg) : CalculateDamage(dmg);

        //healthPoints -= damage;
        OnChangeProgress();
        OnGetHurted?.Invoke(this, EventArgs.Empty);
    }
    private DamageType GetDamageType ( EquipmentDataHolder equipmentDataHolder )
    {
        if (equipmentDataHolder.GetEquipmentElement == Element.None)
            if (criticalRate < UnityEngine.Random.Range(0, 100))
            {
                return DamageType.CriticalDamage;
            }
        return DamageType.NormalDamage;
    }
    public void TakeDamage ( PlayerStats playerStats, EquipmentDataHolder equipmentDataHolder )
    {
        int damage = CalculateDamage(attackPoints);

        Vector3 textPosition = playerStats.transform.position + UnityEngine.Random.onUnitSphere * 0.15f;
        textPosition.y = 2; // Mantén la altura del monstruo si solo te interesa el plano XZ.

        GUI_Pool_Manager.Instance.CreateNumberTexts(GetDamageType(equipmentDataHolder), damage, textPosition);

        playerStats.ApplyDamage(damage);
    }
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
        OnWeaponChanged?.Invoke(this, EventArgs.Empty);

        if (newEquipment != null)
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
            Destroy(equipmentDataHolder_RightHand.gameObject);
            if (equipmentDataHolder_LeftHand != null) Destroy(equipmentDataHolder_LeftHand.gameObject);
            RemoveStatsPoints(equipmentDataHolder_RightHand.GetEquipmentDataSO().equipmentStats, multiplier);
            currentEquipmentHolder = newEquipment;
        }
    }
    private void AddStatsPoints ( EquipmentStats equipmentStats, float multiplier )
    {
        healthPoints += equipmentStats.healthPoints * multiplier;
        manaPoints += equipmentStats.manaPoints * multiplier;

        attackPoints += equipmentStats.attackPoints * multiplier;
        attackSpeed += equipmentStats.attackSpeed * multiplier;
        defensePoints += equipmentStats.defensePoints * multiplier;

        criticalRate += equipmentStats.criticalRate * multiplier;
        criticalDamage += equipmentStats.criticalDamage * multiplier;
    }
    private void RemoveStatsPoints ( EquipmentStats equipmentStats, float multiplier )
    {
        healthPoints -= equipmentStats.healthPoints * multiplier;
        manaPoints -= equipmentStats.manaPoints * multiplier;

        attackPoints -= equipmentStats.attackPoints * multiplier;
        attackSpeed -= equipmentStats.attackSpeed * multiplier;
        defensePoints -= equipmentStats.defensePoints * multiplier;

        criticalRate -= equipmentStats.criticalRate * multiplier;
        criticalDamage -= equipmentStats.criticalDamage * multiplier;
    }

    private void OnChangeProgress ( )
    {
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            healthProgressNormalized = healthPoints / maxHealthPoints,
            manaProgressNormalized = manaPoints / maxManaPoints,
        });
    }
    //float CalculateNextLevelExpRequired ( ) // Formula para subir de nivel, es equilibrada, cuanto mas nivel seas mas dificil sera pero de una manera equitativa
    //{
    //    return (float)(4 * Mathf.Pow(curLvl, 3) + 0.8 * (Mathf.Pow(curLvl, 2) + 2) * 4);
    //}

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
}
