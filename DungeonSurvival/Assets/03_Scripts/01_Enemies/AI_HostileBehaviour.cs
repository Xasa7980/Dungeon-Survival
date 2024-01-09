using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_HostileBehaviour : MonoBehaviour
{
    public static event EventHandler OnBasicAttack;

    public static event EventHandler OnChargedAttack;
    public static event EventHandler OnChargingAttack;

    public static event EventHandler OnSpecialAttack;

    public static event EventHandler OnSkillAttack;
    public static event EventHandler OnCastingSkill;

    [SerializeField] private CombatBehaviourSO combatBehaviourSO;
    [SerializeField] private float basicAtkTimerMax;
    [SerializeField] private float chargedAttackTimerMax;
    [SerializeField] private float rechargingAttackTimerMax;
    [SerializeField] private float specialAtkTimerMax;
    [SerializeField] private float skillAtkTimerMax;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float timerMaxBetweenAttacks;

    [SerializeField] private List<float> attackRates = new List<float>();

    public AttackState attackState;
    private float basicAtkTimer;
    private float chargedAttackTimer;
    private float rechargingAttackTimer;
    private float specialAtkTimer;
    private float specialCastingTimer;
    private float skillAtkTimer;
    private float skillCastingTimer;
    private float timerBetweenAttacks;

    private bool releasingAttack;
    private void Start ( )
    {

    }
    private void Update ( )
    {
        if (!releasingAttack)
        {
            timerBetweenAttacks += Time.deltaTime * attackSpeed;
        }
        if(timerBetweenAttacks > timerMaxBetweenAttacks && !releasingAttack)
        {
            GetAttackState();
        }
        if(releasingAttack)
        {
            Attack();
        }
    }
    private void GetAttackState ( )
    {
        if(!releasingAttack)
        {
            float randomAttackIndex = GetRandomAttack();
            if (randomAttackIndex == attackRates[0])
            {
                Debug.Log(randomAttackIndex);
                attackState = AttackState.Basic;
            }
            if (randomAttackIndex == attackRates[1])
            {
                Debug.Log(randomAttackIndex);
                attackState = AttackState.Recharged;
            }
            if (randomAttackIndex == attackRates[2])
            {
                Debug.Log(randomAttackIndex);
                attackState = AttackState.Special;
            }
            if (randomAttackIndex == attackRates[3])
            {
                Debug.Log(randomAttackIndex);
                attackState = AttackState.Skill;
            }
            releasingAttack = true;
        }
    }
    private void Attack ( )
    {
        switch (attackState)
        {
            case AttackState.Basic:

                basicAtkTimer += Time.deltaTime;

                if (basicAtkTimer > basicAtkTimerMax)
                {
                    basicAtkTimer = 0;
                    timerBetweenAttacks = 0;
                    releasingAttack = false;

                    OnBasicAttack?.Invoke(this, EventArgs.Empty);

                    //Do atk basic
                }
                break;

            case AttackState.Recharged:

                chargedAttackTimer += Time.deltaTime;

                if (chargedAttackTimer > chargedAttackTimerMax)
                {
                    rechargingAttackTimer += Time.deltaTime;

                    OnChargedAttack?.Invoke(this, EventArgs.Empty);

                    if (rechargingAttackTimer > rechargingAttackTimerMax)
                    {
                        chargedAttackTimer = 0;
                        rechargingAttackTimer = 0;
                        timerBetweenAttacks = 0;
                        releasingAttack = false;

                        OnChargingAttack?.Invoke(this, EventArgs.Empty);

                        //Do recharged atk
                    }
                }
                break;

            case AttackState.Special:

                specialAtkTimer += Time.deltaTime;

                if (specialAtkTimer > specialAtkTimerMax)
                {
                    specialCastingTimer += Time.deltaTime;

                    if (specialCastingTimer > combatBehaviourSO.specialAtkData.castingTimerMax)
                    {
                        specialAtkTimer = 0;
                        specialCastingTimer = 0;
                        timerBetweenAttacks = 0;
                        releasingAttack = false;

                        OnSpecialAttack?.Invoke(this, EventArgs.Empty);

                        //Do special atk

                    }
                }
                break;

            case AttackState.Skill:

                skillAtkTimer += Time.deltaTime;

                if (skillAtkTimer > skillAtkTimerMax)
                {
                    skillCastingTimer += Time.deltaTime;

                    OnSkillAttack?.Invoke(this, EventArgs.Empty);

                    if (skillCastingTimer > combatBehaviourSO.skillAtkData.castingTimerMax)
                    {
                        skillCastingTimer = 0;
                        skillAtkTimer = 0;
                        timerBetweenAttacks = 0;
                        releasingAttack = false;

                        OnCastingSkill?.Invoke(this, EventArgs.Empty);

                        //Do skill atk
                    }
                }
                break;
        }
    }
    private void GetAttack ( )
    {
        if( !releasingAttack )
        {
            if (GetRandomAttack() == attackRates[0])
            {
            }
            else if (GetRandomAttack() == attackRates[1])
            {
            }
            else if (GetRandomAttack() == attackRates[2])
            {
            }
            else if (GetRandomAttack() == attackRates[3])
            {
            }
        }
    }
    private float GetRandomAttack ( )
    {
        float randNum = UnityEngine.Random.value;
        float total = randNum * 100;
        float cumulativeRate = 0;

        for (int i = 0; i < attackRates.Count; i++)
        {
            cumulativeRate += attackRates[i];
            if (total < cumulativeRate)
            {
                return attackRates[i];
            }
        }

        // Si llega aquí, significa que total no era menor que ninguna de las tasas acumuladas
        // Deberías decidir qué hacer en este caso, por ejemplo, retornar un valor por defecto.
        return -1; // O cualquier valor que indique una condición especial o un error.
        }
    }
