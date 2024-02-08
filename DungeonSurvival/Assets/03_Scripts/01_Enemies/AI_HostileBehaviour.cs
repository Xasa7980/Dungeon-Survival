using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
        public int attackIndex;
        public AttacksDataSO attacksDataSO;
    }
    public event EventHandler OnSpecialAttack;
    public event EventHandler<OnLoadingSpecialEventArgs> OnLoadingSpecialAttack;
    public class OnLoadingSpecialEventArgs : EventArgs
    {
        public float progressNormalized;
        public int attackIndex;
        public AttacksDataSO attacksDataSO;
    }

    public event EventHandler OnSkillAttack;
    public event EventHandler<OnLoadingSkillEventArgs> OnCastingSkill;
    public class OnLoadingSkillEventArgs : EventArgs
    {
        public float progressNormalized;
        public int attackIndex; 
        public AttacksDataSO attacksDataSO;
    }

    public AttackCategory attackCategory;

    private List<AttacksDataSO> basicAttacks;
    private List<AttacksDataSO> chargedAttacks;
    private List<AttacksDataSO> specialAttacks;
    private List<AttacksDataSO> skillAttacks;

    private List<AttackInstance> basicAttacksInstances = new List<AttackInstance>();
    private List<AttackInstance> chargedAttacksInstances = new List<AttackInstance>();
    private List<AttackInstance> specialAttacksInstances = new List<AttackInstance>();
    private List<AttackInstance> skillAttacksInstances = new List<AttackInstance>();

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

    // Timers
    #region Timers
    private float basicAttackReleaseTimer;
    private float chargedAttackReleaseTimer;
    private float chargedAttackLoadingTimer;
    private float specialAttackReleaseTimer;
    private float specialAttackLoadingTimer;
    private float skillAttackReleaseTimer;
    private float skillAttackLoadingTimer;
    private float timerBetweenAttack;
    #endregion

    // Booleans
    private bool releasingAttack;
    public bool SetHit ( bool value ) => hit = value;
    private bool hit;
    private void OnEnable ( )
    {
        SetupComponents();
        ResetAttackTimers();
        OnEnterCombat?.Invoke(this, EventArgs.Empty);
        SetAttacksDataList();
    }
    private void SetupComponents ( )
    {
        ai_MainCore = GetComponent<AI_MainCore>();
        monsterStats = ai_MainCore.GetMonsterStats();
        monsterAnimations = ai_MainCore.GetMonsterAnimationHandler();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.isStopped = true;
    }
    private void SetAttacksDataList ( )
    {
        basicAttacks = monsterAnimations.enemyAnimationHandler.AnimationClipContainerSO.basicAttacksSO_List;
        chargedAttacks = monsterAnimations.enemyAnimationHandler.AnimationClipContainerSO.chargedAttacksSO_List;
        specialAttacks = monsterAnimations.enemyAnimationHandler.AnimationClipContainerSO.specialAttacksSO_List;
        skillAttacks = monsterAnimations.enemyAnimationHandler.AnimationClipContainerSO.skillAttacksSO_List;

        InitializeAttackList(basicAttacks, basicAttacksInstances);
        InitializeAttackList(chargedAttacks, chargedAttacksInstances);
        InitializeAttackList(specialAttacks, specialAttacksInstances);
        InitializeAttackList(skillAttacks, skillAttacksInstances);
    }
    private void InitializeAttackList ( List<AttacksDataSO> dataSOArray, List<AttackInstance> attackList )
    {
        foreach (var dataSO in dataSOArray)
        {
            attackList.Add(new AttackInstance(dataSO));
        }
    }
    private void ClearAttackInstances( )
    {
        List<AttackInstance>[] attackInstances = { basicAttacksInstances, chargedAttacksInstances, specialAttacksInstances, skillAttacksInstances };
        foreach (var attackInstance in attackInstances)
        {
            attackInstance.Clear();
        }
    }
    private void ResetAttackTimers ( )
    {
        basicAttackReleaseTimer = 0;
        chargedAttackReleaseTimer = 0;
        specialAttackReleaseTimer = 0;
        skillAttackReleaseTimer = 0;
        timerBetweenAttack = 0;
        releasingAttack = false;
    }

    private void OnDisable ( )
    {
        ClearAttackInstances();
        OnExitCombat?.Invoke(this, EventArgs.Empty);
    }
    private void Update ( )
    {
        CheckForEnemies();
        UpdateCooldownTimers();

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
        int randomIndex = GetRandomAttack();

        if (randomIndex >= 0) 
        {
            attackCategory = (AttackCategory)randomIndex;
        }
        attackCategory = (AttackCategory)randomIndex;
        releasingAttack = true;
        timerBetweenAttack = 0;
    }
    private void UpdateCooldownTimers ( )
    {
        UpdateCooldownTimer(chargedAttacksInstances);
        UpdateCooldownTimer(specialAttacksInstances);
        UpdateCooldownTimer(skillAttacksInstances);
    }

    private void UpdateCooldownTimer ( List<AttackInstance> attackInstances )
    {
        foreach (var attack in attackInstances)
        {
            if (attack.cooldownTimer < attack.attackData.coldownAttackTimerMax)
            {
                attack.cooldownTimer += Time.deltaTime * attackSpeed;
            }
        }
    }
    private void PerformAttack ( )
    {
        int index = 0;
        switch (attackCategory)
        {
            case AttackCategory.basic:

                if (basicAttacks.Count < 0) return;

                PerformBasicAttack();
                break;
            case AttackCategory.charged:

                if (chargedAttacks.Count < 0) return;

                index = UnityEngine.Random.Range( 0, chargedAttacks.Count);
                PerformChargedAttack(index);
                break;
            case AttackCategory.special:

                if (specialAttacks.Count < 0) return;

                index = UnityEngine.Random.Range( 0, specialAttacks.Count);
                PerformSpecialAttack(index);
                break;
            case AttackCategory.skill:

                if (skillAttacks.Count < 0) return;

                index = UnityEngine.Random.Range( 0, skillAttacks.Count);
                PerformSkillAttack(index);
                break;
        }
    }

    private void PerformBasicAttack ( )
    {
        if (basicAttackReleaseTimer < basicAttackReleaseTimerMax)
        {
            basicAttackReleaseTimer += Time.deltaTime;
        }
        else
        {
            ResetAttackTimers();
            OnBasicAttack?.Invoke(this, EventArgs.Empty);
        }
    }

    private void PerformChargedAttack ( int index )
    {
        if (chargedAttacksInstances[index].cooldownTimer > chargedAttacksInstances[index].attackData.coldownAttackTimerMax)
        {
            if (chargedAttacksInstances[index].attackData.specialAttackNeedsLoading)
            {
                if (chargedAttackLoadingTimer < chargedAttacksInstances[index].attackData.loadingTimerMax)
                {
                    chargedAttackLoadingTimer += Time.deltaTime;
                    OnChargingAttack?.Invoke(this, new OnLoadingAttackEventArgs
                    {
                        progressNormalized = chargedAttackLoadingTimer / chargedAttacksInstances[index].attackData.loadingTimerMax,
                        attackIndex = index,
                        attacksDataSO = chargedAttacksInstances[index].attackData
                    });
                }
                else if (chargedAttackReleaseTimer > chargedAttackReleaseTimerMax)
                {
                    ResetAttackTimers();
                    chargedAttackLoadingTimer = 0;
                    chargedAttacksInstances[index].cooldownTimer = 0;
                    OnChargedAttack?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    chargedAttackReleaseTimer += Time.deltaTime;
                }
            }
            else
            {
                OnChargingAttack?.Invoke(this, new OnLoadingAttackEventArgs
                {
                    progressNormalized = chargedAttackLoadingTimer / chargedAttacksInstances[index].attackData.loadingTimerMax,
                    attackIndex = index,
                    attacksDataSO = chargedAttacksInstances[index].attackData
                });

                if (chargedAttackReleaseTimer > chargedAttackReleaseTimerMax)
                {
                    ResetAttackTimers();
                    chargedAttackLoadingTimer = 0;
                    chargedAttacksInstances[index].cooldownTimer = 0;
                    OnChargedAttack?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    chargedAttackReleaseTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            attackCategory = AttackCategory.basic;
            return;
        }
    }

    private void PerformSpecialAttack ( int index )
    {
        if(specialAttacksInstances[index].cooldownTimer > specialAttacksInstances[index].attackData.coldownAttackTimerMax)
        {
            if (specialAttacksInstances[index].attackData.specialAttackNeedsLoading)
            {
                if (specialAttackLoadingTimer < specialAttacksInstances[index].attackData.loadingTimerMax)
                {
                    specialAttackLoadingTimer += Time.deltaTime;
                    OnLoadingSpecialAttack?.Invoke(this, new OnLoadingSpecialEventArgs
                    {
                        progressNormalized = specialAttackLoadingTimer / specialAttacksInstances[index].attackData.loadingTimerMax,
                        attackIndex = index,
                        attacksDataSO = specialAttacksInstances[index].attackData
                    });
                }
                else if (specialAttackReleaseTimer > specialAttackReleaseTimerMax)
                {
                    OnSpecialAttack?.Invoke(this, EventArgs.Empty);

                    ResetAttackTimers();
                    specialAttackLoadingTimer = 0;
                    specialAttacksInstances[index].cooldownTimer = 0;
                }
                else
                {
                    specialAttackReleaseTimer += Time.deltaTime;
                }
            }
            else
            {
                OnLoadingSpecialAttack?.Invoke(this, new OnLoadingSpecialEventArgs
                {
                    progressNormalized = specialAttackLoadingTimer / specialAttacksInstances[index].attackData.loadingTimerMax,
                    attackIndex = index,
                    attacksDataSO = specialAttacksInstances[index].attackData
                });

                if (specialAttackReleaseTimer > specialAttackReleaseTimerMax)
                {
                    ResetAttackTimers();
                    specialAttackLoadingTimer = 0;
                    specialAttacksInstances[index].cooldownTimer = 0;

                    OnSpecialAttack?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    specialAttackReleaseTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            attackCategory = AttackCategory.basic;
            return;
        }

    }
    private void PerformSkillAttack ( int index )
    {
        if(skillAttacksInstances[index].cooldownTimer > skillAttacksInstances[index].attackData.coldownAttackTimerMax)
        {
            if (skillAttacksInstances[index].attackData.specialAttackNeedsLoading)
            {
                if (skillAttackLoadingTimer < skillAttacksInstances[index].attackData.loadingTimerMax)
                {
                    skillAttackLoadingTimer += Time.deltaTime;
                    OnCastingSkill?.Invoke(this, new OnLoadingSkillEventArgs
                    {
                        progressNormalized = skillAttackLoadingTimer / skillAttacksInstances[index].attackData.loadingTimerMax,
                        attackIndex = index,
                        attacksDataSO = skillAttacksInstances[index].attackData
                    });
                }
                else if (skillAttackReleaseTimer > skillAttackReleaseTimerMax)
                {
                    ResetAttackTimers();
                    skillAttackLoadingTimer = 0;
                    skillAttacksInstances[index].cooldownTimer = 0;

                    OnSkillAttack?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    skillAttackReleaseTimer += Time.deltaTime;
                }
                Debug.Log(skillAttackLoadingTimer < skillAttacksInstances[index].attackData.loadingTimerMax);
            }
            else
            {
                OnCastingSkill?.Invoke(this, new OnLoadingSkillEventArgs
                {
                    progressNormalized = skillAttackLoadingTimer / skillAttacksInstances[index].attackData.loadingTimerMax,
                    attackIndex = index,
                    attacksDataSO = skillAttacksInstances[index].attackData
                });

                if (skillAttackReleaseTimer > skillAttackReleaseTimerMax)
                {
                    ResetAttackTimers();
                    skillAttackLoadingTimer = 0;
                    skillAttacksInstances[index].cooldownTimer = 0;

                    OnSkillAttack?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    skillAttackReleaseTimer += Time.deltaTime;
                }
            }
        }
        else if(skillAttacksInstances[index].cooldownTimer < skillAttacksInstances[index].attackData.coldownAttackTimerMax)
        {
            attackCategory = AttackCategory.basic;
            return;
        }

    }

    private int GetRandomAttack ( )
    {
        float randNum = UnityEngine.Random.value;
        float total = randNum * (attackRates.Count > 0 ? attackRates.Sum() : 1);
        float cumulativeRate = 0;

        for (int i = 0; i < attackRates.Count; i++)
        {
            cumulativeRate += attackRates[i];
            if (total < cumulativeRate)
            {
                return i; //Indice de attackRates seleccionado
            }
        }

        // Si llega aquí, significa que total no era menor que ninguna de las tasas acumuladas
        // Deberías decidir qué hacer en este caso, por ejemplo, retornar un valor por defecto.
        return 0; // O cualquier valor que indique una condición especial o un error.
    }
    private void CheckForEnemies ( )
    {
        Collider[] rightDetection = GetDetectionColliders(monsterStats.RightDetectionArea);
        Collider[] leftDetection = GetDetectionColliders(monsterStats.LeftDetectionArea);

        if(monsterAnimations.SelectCurrentAnimatorState(monsterAnimations.COMBAT_LAYER).normalizedTime < 0.95f)
        {
            if(rightDetection.Length > 0)
            {
                CheckAndDamageEnemies(rightDetection, monsterStats.EquipmentDataHolder_RightHand);
            }
            else if(leftDetection.Length > 0)
            {
                CheckAndDamageEnemies(leftDetection, monsterStats.EquipmentDataHolder_LeftHand);
            }
        }
        else
        {
            hit = false;
        }
    }
    private Collider[] GetDetectionColliders(AreaDrawer detectionArea )
    {
        Collider[] detectionColliders = new Collider[0];
        if(detectionArea != null)
        {
            if(detectionArea.drawMode == AreaDrawer.DrawMode.Box)
            {
                detectionColliders = Physics.OverlapBox(detectionArea.objectPosition, detectionArea.size, detectionArea.rotation, detectionMask);
            }
            else if(detectionArea.drawMode == AreaDrawer.DrawMode.Sphere)
            {
                detectionColliders = Physics.OverlapSphere(detectionArea.objectPosition, detectionArea.radius, detectionMask);
            }
        }
        return detectionColliders;
    }
    private void CheckAndDamageEnemies ( Collider[] detectionColliders, EquipmentDataHolder equipmentDataHolder )
    {
        if(!hit)
        {
            foreach (Collider target in detectionColliders)
            {
                if(target.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
                {
                    monsterStats.TakeDamage(playerStats,equipmentDataHolder);
                    hit = true;
                    break;
                }
            }
        }
    }
}
