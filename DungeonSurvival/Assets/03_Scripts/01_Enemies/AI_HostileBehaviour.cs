using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AI_HostileBehaviour : MonoBehaviour
{
    public event EventHandler OnEnterCombat;
    public event EventHandler OnExitCombat;

    public event EventHandler OnBasicAttack;

    public event EventHandler OnChargedAttack;
    public event EventHandler<OnChargingAttackEventArgs> OnChargingAttack;
    public class OnChargingAttackEventArgs : EventArgs
    {
        public float progressNormalized;
    }
    public event EventHandler OnSpecialAttack;

    public event EventHandler OnSkillAttack;
    public event EventHandler<OnCastingSkillEventArgs> OnCastingSkill;
    public class OnCastingSkillEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    [SerializeField] private AttacksDataSO specialAttacksSO;

    [SerializeField] private float basicAtkTimerMax;
    [SerializeField] private float chargedAttackTimerMax;
    [SerializeField] private float chargingReleaseAttackMax;
    [SerializeField] private float specialAtkTimerMax;
    [SerializeField] private float skillReleaseTimeMax;
    [SerializeField] private float skillCastingTimerMax;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float timerMaxBetweenAttacks;

    [SerializeField] private List<float> attackRates = new List<float>();

    public AttackCategory attackState;
    private NavMeshAgent navAgent;
    private AI_MainCore ai_MainCore;

    private MonsterStats monsterStats;
    private float basicAtkTimer;
    private float chargedAttackTimer;
    private float chargingReleaseAttack;
    private float specialAtkTimer;
    private float specialCastingTimer;
    private float skillReleaseTime;
    private float skillCastingTimer;
    private float timerBetweenAttacks;

    private bool releasingAttack;
    private void OnEnable ( )
    {
        ai_MainCore = GetComponent<AI_MainCore>();
        monsterStats = ai_MainCore.GetMonsterStats();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.isStopped = true;

        OnEnterCombat?.Invoke(this, EventArgs.Empty);
        chargingReleaseAttack = chargingReleaseAttackMax;
        skillCastingTimer = specialAttacksSO.loadingTimerMax;
    }
    private void OnDisable ( )
    {
        OnExitCombat?.Invoke(this, EventArgs.Empty);
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
                //Deal dmg basic attack
                attackState = AttackCategory.basic;
            }
            if (randomAttackIndex == attackRates[1])
            {
                //Deal dmg charged attack
                attackState = AttackCategory.charged;
            }
            if (randomAttackIndex == attackRates[2])
            {
                //Deal dmg special attack
                attackState = AttackCategory.special;
            }
            if (randomAttackIndex == attackRates[3])
            {
                //Deal dmg skill attack
                attackState = AttackCategory.skill;
            }
            releasingAttack = true;
        }
    }
    private void Attack ( )
    {
        switch (attackState)
        {
            case AttackCategory.basic:

                if (basicAtkTimer < basicAtkTimerMax) basicAtkTimer += Time.deltaTime;

                else
                {
                    //Release basic attack 

                    basicAtkTimer = 0;
                    timerBetweenAttacks = 0;
                    releasingAttack = false;

                    OnBasicAttack?.Invoke(this, EventArgs.Empty);
                }
                break;

            case AttackCategory.charged:

                if(chargingReleaseAttack > 0) chargingReleaseAttack -= Time.deltaTime;

                OnChargingAttack?.Invoke(this, new OnChargingAttackEventArgs
                {
                    progressNormalized = chargingReleaseAttack / chargingReleaseAttackMax
                });

                if (chargingReleaseAttack <= 0)
                {
                    chargedAttackTimer += Time.deltaTime;

                    if (chargedAttackTimer > chargedAttackTimerMax)
                    {
                        //Release charged

                        chargedAttackTimer = 0;
                        timerBetweenAttacks = 0;
                        chargingReleaseAttack = chargingReleaseAttackMax;
                        releasingAttack = false;

                        OnChargedAttack?.Invoke(this, EventArgs.Empty);
                    }
                }
                break;

            case AttackCategory.special:

                if (specialAtkTimer < specialAtkTimerMax) specialAtkTimer += Time.deltaTime;

                else
                {
                    specialCastingTimer += Time.deltaTime;

                    if (specialCastingTimer > specialAttacksSO.loadingTimerMax) 
                    {
                        //Release special attack

                        specialAtkTimer = 0;
                        specialCastingTimer = 0;
                        timerBetweenAttacks = 0;
                        releasingAttack = false;

                        OnSpecialAttack?.Invoke(this, EventArgs.Empty);
                    }
                }
                break;

            case AttackCategory.skill:

                if (skillCastingTimer > 0) skillCastingTimer -= Time.deltaTime;
                OnCastingSkill?.Invoke(this, new OnCastingSkillEventArgs
                {
                    progressNormalized = skillCastingTimer / specialAttacksSO.loadingTimerMax
                });

                if (skillCastingTimer <= 0)
                {
                    skillReleaseTime += Time.deltaTime;
                    if (skillReleaseTime > skillReleaseTimeMax)
                    {
                        //Release charged

                        skillReleaseTime = 0;
                        timerBetweenAttacks = 0;
                        skillCastingTimer = specialAttacksSO.loadingTimerMax;
                        releasingAttack = false;

                        OnSkillAttack?.Invoke(this, EventArgs.Empty);
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
