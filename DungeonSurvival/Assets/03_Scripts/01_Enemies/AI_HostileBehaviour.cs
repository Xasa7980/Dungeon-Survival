using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AI_HostileBehaviour : MonoBehaviour
{
    public event EventHandler OnEnterCombat;
    public event EventHandler OnExitCombat;

    public event EventHandler OnBasicAttack;

    public event EventHandler OnChargedAttack;
    public event EventHandler<OnLoadingAttackEventArgs> OnChargingAttack;
    public class OnLoadingAttackEventArgs : EventArgs
    {
        public float progressNormalized;
    }
    public event EventHandler OnSpecialAttack;
    public event EventHandler<OnLoadingSpecialEventArgs> OnLoadingSpecialAttack;
    public class OnLoadingSpecialEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    public event EventHandler OnSkillAttack;
    public event EventHandler<OnLoadingSkillEventArgs> OnCastingSkill;
    public class OnLoadingSkillEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    public AttackCategory attackCategory;

    [SerializeField] private AttacksDataSO[] chargedAttacks;
    [SerializeField] private AttacksDataSO[] specialAttacks;
    [SerializeField] private AttacksDataSO[] skillAttacks;


    [SerializeField] private float basicAttackReleaseTimerMax = 1;
    [SerializeField] private float chargedAttackReleaseTimerMax = 2;
    [SerializeField] private float specialAttackReleaseTimerMax = 2;
    [SerializeField] private float skillAttackReleaseTimerMax = 3;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float timerBetweenAttackMax = 3;

    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private List<float> attackRates = new List<float>();

    private NavMeshAgent navAgent;
    private AI_MainCore ai_MainCore;
    private MonsterAnimations monsterAnimations;
    private MonsterStats monsterStats;
    private float basicAttackReleaseTimer;
    private float chargedAttackReleaseTimer;
    private float chargedAttackLoadingTimer;
    private float specialAttackReleaseTimer;
    private float specialAttackLoadingTimer;
    private float skillAttackReleaseTimer;
    private float skillAttackLoadingTimer;

    private float timerBetweenAttack;

    private bool releasingAttack;
    public bool SetHit ( bool value ) => hit = value;
    private bool hit;
    private void OnEnable ( )
    {
        SetupComponents();
        ResetAttackTimers();
        OnEnterCombat?.Invoke(this, EventArgs.Empty);
    }
    private void SetupComponents ( )
    {
        ai_MainCore = GetComponent<AI_MainCore>();
        monsterStats = ai_MainCore.GetMonsterStats();
        monsterAnimations = ai_MainCore.GetMonsterAnimationHandler();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.isStopped = true;
    }
    private void ResetAttackTimers ( )
    {
        basicAttackReleaseTimer = 0;
        chargedAttackReleaseTimer = 0;
        specialAttackReleaseTimer = 0;
        specialAttackLoadingTimer = 0;
        skillAttackReleaseTimer = 0;
        timerBetweenAttack = 0;
        releasingAttack = false;
    }

    private void OnDisable ( )
    {
        OnExitCombat?.Invoke(this, EventArgs.Empty);
    }
    private void Update ( )
    {
        CheckForEnemies();

        if (!releasingAttack)
        {
            timerBetweenAttack += Time.deltaTime * attackSpeed;
            if (timerBetweenAttack > timerBetweenAttackMax && !releasingAttack)
            {
                DetermineAttackType();
            }
        }
        else
        {
            PerformAttack();
        }
    }
    private void DetermineAttackType ( )
    {
        float randomAttackIndex = GetRandomAttack();
        attackCategory = (AttackCategory)Mathf.Clamp(randomAttackIndex, 0, Enum.GetValues(typeof(AttackCategory)).Length - 1); //Clampea randomAttackIndex entre el valor 0 y el tamaño del rango de "AttackCategory"
        releasingAttack = true;
        timerBetweenAttack = 0;
    }
    private void PerformAttack ( )
    {
        int index = 0;
        switch (attackCategory)
        {
            case AttackCategory.basic:
                PerformBasicAttack();
                break;
            case AttackCategory.charged:

                if (skillAttacks.Length == 0)
                {
                    return;
                }
                index = UnityEngine.Random.Range( 0, chargedAttacks.Length);
                PerformChargedAttack(chargedAttacks[index]);
                break;
            case AttackCategory.special:

                if (skillAttacks.Length == 0)
                {
                    return;
                }
                index = UnityEngine.Random.Range( 0, specialAttacks.Length);
                PerformSpecialAttack(specialAttacks[index]);
                break;
            case AttackCategory.skill:

                if(skillAttacks.Length == 0)
                {
                    return;
                }
                index = UnityEngine.Random.Range( 0, skillAttacks.Length);
                PerformSkillAttack(skillAttacks[index]);
                break;
        }
    }

    private void PerformBasicAttack ( )
    {
        if (basicAttackReleaseTimer > basicAttackReleaseTimerMax)
        {
            basicAttackReleaseTimer += Time.deltaTime;
        }
        else
        {
            ResetAttackTimers();
            OnBasicAttack?.Invoke(this, EventArgs.Empty);
        }
    }

    private void PerformChargedAttack ( AttacksDataSO attacksDataSO )
    {
        if (chargedAttackLoadingTimer < 0)
        {
            chargedAttackLoadingTimer -= Time.deltaTime;
            OnChargingAttack?.Invoke(this, new OnLoadingAttackEventArgs
            {
                progressNormalized = chargedAttackLoadingTimer / attacksDataSO.loadingTimerMax
            });
        }
        else if (chargedAttackReleaseTimer > chargedAttackReleaseTimerMax)
        {
            ResetAttackTimers();
            chargedAttackLoadingTimer = attacksDataSO.loadingTimerMax; // Reset for next charged attack
            OnChargedAttack?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            chargedAttackReleaseTimer += Time.deltaTime;
        }
    }

    private void PerformSpecialAttack ( AttacksDataSO attacksDataSO )
    {
        if (specialAttackLoadingTimer < 0)
        {
            skillAttackLoadingTimer -= Time.deltaTime;
            OnLoadingSpecialAttack?.Invoke(this, new OnLoadingSpecialEventArgs
            {
                progressNormalized = specialAttackLoadingTimer / attacksDataSO.loadingTimerMax
            });
        }
        else if (specialAttackReleaseTimer > specialAttackReleaseTimerMax)
        {
            ResetAttackTimers();
            specialAttackLoadingTimer = attacksDataSO.loadingTimerMax;
            OnSpecialAttack?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            specialAttackReleaseTimer += Time.deltaTime;
        }
    }
    private void PerformSkillAttack ( AttacksDataSO attacksDataSO )
    {
        if (skillAttackLoadingTimer < 0)
        {
            skillAttackLoadingTimer -= Time.deltaTime;
            OnCastingSkill?.Invoke(this, new OnLoadingSkillEventArgs
            {
                progressNormalized = skillAttackLoadingTimer / attacksDataSO.loadingTimerMax
            });
        }
        else if (skillAttackReleaseTimer > skillAttackReleaseTimerMax)
        {
            ResetAttackTimers();
            skillAttackLoadingTimer = attacksDataSO.loadingTimerMax;
            OnSkillAttack?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            skillAttackReleaseTimer += Time.deltaTime;
        }
    }

    private void CheckForEnemies ( )
    {
        Collider[] rightDetection = new Collider[0]; 
        if(monsterStats.equipmentDataHolder_RightHand != null && monsterStats.RightDetectionArea.drawMode == AreaDrawer.DrawMode.Box)
        {
            rightDetection = Physics.OverlapBox(monsterStats.RightDetectionArea.objectPosition,monsterStats.RightDetectionArea.size,
                             monsterStats.RightDetectionArea.rotation, detectionMask);
        }
        else if(monsterStats.equipmentDataHolder_RightHand != null && monsterStats.RightDetectionArea.drawMode == AreaDrawer.DrawMode.Sphere)
        {
            rightDetection = Physics.OverlapSphere(monsterStats.RightDetectionArea.objectPosition, monsterStats.RightDetectionArea.radius,
                             detectionMask);
        }
        Collider[] leftDetection = new Collider[0];
        if(monsterStats.equipmentDataHolder_LeftHand != null && monsterStats.LeftDetectionArea.drawMode == AreaDrawer.DrawMode.Box)
        {
            leftDetection = Physics.OverlapBox(monsterStats.LeftDetectionArea.objectPosition, monsterStats.LeftDetectionArea.size,
                           monsterStats.LeftDetectionArea.rotation, detectionMask);
        }
        else if (monsterStats.equipmentDataHolder_LeftHand != null && monsterStats.LeftDetectionArea.drawMode == AreaDrawer.DrawMode.Sphere)
        {
            leftDetection = Physics.OverlapSphere(monsterStats.LeftDetectionArea.objectPosition, monsterStats.LeftDetectionArea.radius,
                             detectionMask);
        }
        if (monsterAnimations.GetCurrentAnimationInfo(monsterAnimations.COMBAT_LAYER, monsterAnimations.ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME).normalizedTime < 0.85f)
        {
            if (rightDetection.Length > 0)
            {
                if (!hit)
                {
                    foreach (Collider target in rightDetection)
                    {
                        if (target.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
                        {
                            print("rightHitted");

                            monsterStats.TakeDamage(playerStats, monsterStats.EquipmentDataHolder_RightHand);
                            hit = true;
                            break;
                        }
                    }
                }
            }
            else if (leftDetection.Length > 0)
            {
                if (!hit)
                {
                    foreach (Collider target in leftDetection)
                    {
                        if (target.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
                        {
                            print("leftHitted");

                            monsterStats.TakeDamage(playerStats, monsterStats.EquipmentDataHolder_LeftHand);
                            hit = true;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            hit = false;
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
