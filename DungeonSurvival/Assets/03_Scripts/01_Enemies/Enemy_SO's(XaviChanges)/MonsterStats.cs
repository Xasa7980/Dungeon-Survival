using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStats : MonoBehaviour,IHasProgress
{
    public event EventHandler OnGetHurted;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    [Header("Battle Experience")]

    public int curLvl = 1, playerLvl = 1 /* Hare que quien tenga nivel sea el mundo, lo que haga que la dificultad aumente respecto al nivel del personaje, los bosses se quedan talcual */ , lvlDif;
    public int minExpGiven = 4, maxExpGiven = 10;
    public int ExpGiven => UnityEngine.Random.Range(minExpGiven, maxExpGiven);


    [SerializeField] private MonsterDataSO monsterDataSO;
    [SerializeField] private AI_MainCore aI_MainCore;

    private StatsDatabase statsDatabase = new StatsDatabase();
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
    private bool Death => healthPoints <= 0? true: false;

    private void Awake ( )
    {
        aI_MainCore = GetComponent<AI_MainCore>();
        InitializeStats();

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            healthProgressNormalized = healthPoints / maxHealthPoints,
            manaProgressNormalized = manaPoints / maxManaPoints,
        });
    }
    private void Start ( )
    {
    }
    private void Update ( )
    {
        if (Death)
        {
            aI_MainCore.SetState(State.Death);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetDamage(555);
        }
    }
    public void GetDamage ( int dmg )
    {
        healthPoints -= dmg;
        OnChangeProgress();
        OnGetHurted?.Invoke(this, EventArgs.Empty);
    }
    private void TakeDamage ( PlayerStats playerStats )
    {
        int damage = CalculateDamage(attackPoints);
        playerStats.GetDamage(damage);
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

        criticalRate = statsDatabase.Get_CritRateOnBaseDexAndAgi(DEX,AGI);
        criticalDamage = statsDatabase.Get_CritDmgOnBaseDex(DEX);

        defensePoints = statsDatabase.Get_DefenseOnBaseRes(RES) + (int)monsterDataSO.extraDefensePoints;
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
