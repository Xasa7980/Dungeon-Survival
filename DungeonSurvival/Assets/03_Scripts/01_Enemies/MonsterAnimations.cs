using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimations : MonoBehaviour,ICombatBehaviour
{
    private const int BASE_LAYER_INDEX = 0;
    public int COMBAT_LAYER { get { return _COMBAT_LAYER; } }
    private const int _COMBAT_LAYER = 1;

    //private const int ADDITIVE_EFFECTS_LAYER_INDEX = 2;
    public string ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME { get { return _ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME; } }
    private const string _ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME = "AttackCombo";
    public string ANIMATION_ATTACK_CHARGED_TREE_PERFORMED_NAME { get { return _ATTACK_CHARGED_TREE_NAME; } }
    private const string _ATTACK_CHARGED_TREE_NAME = "ChargedAttack";
    public string ANIMATION_ATTACK_SPECIAL_TREE_PERFORMED_NAME { get { return _ATTACK_SPECIAL_TREE_NAME; } }
    private const string _ATTACK_SPECIAL_TREE_NAME = "SkillAttack";
    public string ANIMATION_ATTACK_SKILL_TREE_PERFORMED_NAME { get { return _ATTACK_SKILL_TREE_NAME; } }
    private const string _ATTACK_SKILL_TREE_NAME = "SkillAttack";

    private const string IS_WALKING_BOOL = "IsWalking";
    private const string IS_RUNNING_BOOL = "IsRunning";
    private const string ATTACK_TRIGGER_FLOAT = "Attack";
    private const string ATTACK_INDEX_FLOAT = "AttackIndex";
    private const string CHARGED_ATTACK_BOOL = "ChargedAttack";
    private const string RECHARGING_TRIGGER = "ChargingAttack";
    private const string SPECIAL_ATTACK_TRIGGER = "SpecialAttack";
    private const string SKILL_ATTACK_BOOL = "SkillAttack";
    private const string CASTING_SKILL_TRIGGER = "SkillCastingAttack";
    private const string DEATH_TRIGGER = "Death";


    private AI_MainCore ai_MainCore;
    private MonsterStats monsterStats;
    private EquipmentDataSO equipmentDataSO => monsterStats.equipmentDataHolder_RightHand.GetEquipmentDataSO();
    
    private bool isWalking;
    private bool isRunning;

    private int numberOfBasicAttacks => monsterStats ? equipmentDataSO.equipmentAnimationClips.basicAttackClips.Length : -1;
    private int numberOfChargedAttacks => monsterStats ? equipmentDataSO.equipmentAnimationClips.chargedAttackClips.Length : -1;
    private int numberOfSpecialAttacks => monsterStats ? equipmentDataSO.equipmentAnimationClips.specialAttackClips.Length : -1;
    private int numberOfSkillAttacks => monsterStats ? equipmentDataSO.equipmentAnimationClips.skillAttackClips.Length : -1;

    private float basicAttackIndex;
    private float chargedAttackIndex;
    private float specialAttackIndex;
    private float skillAttackIndex;

    private Animator animator;
    private AI_PatrolBehaviour patrolBehaviour;
    private AI_ChasingBehaviour chasingBehaviour;
    private AI_HostileBehaviour hostileBehaviour;
    private bool loadingSpecialAttack;
    private bool loadingChargedAttack;
    private bool loadingSkillAttack;
    private void Awake ( )
    {
        //ai_MainCore = GetComponentInParent<AI_MainCore>();
        animator = GetComponent<Animator>();
        SetBehaviours();
    }
    private void SetBehaviours ( )
    {
        patrolBehaviour = ai_MainCore.GetPatrolBehaviour();
        chasingBehaviour = ai_MainCore.GetChasingBehaviour();
        hostileBehaviour = ai_MainCore.GetHostileBehaviour();
    }
    private void Start ( )
    {
        patrolBehaviour.OnWalkAction += AI_PatrolBehaviour_OnWalkAction;
        chasingBehaviour.OnRunAction += AI_ChasingBehaviour_OnRunAction;
        hostileBehaviour.OnEnterCombat += HostileBehaviour_OnEnterCombat;
        hostileBehaviour.OnExitCombat += HostileBehaviour_OnExitCombat;
        hostileBehaviour.OnBasicAttack += AI_HostileBehaviour_OnBasicAttack;
        hostileBehaviour.OnChargingAttack += AI_HostileBehaviour_OnLoadingChargedAttack;
        hostileBehaviour.OnSpecialAttack += AI_HostileBehaviour_OnSpecialAttack;
        hostileBehaviour.OnCastingSkill += AI_HostileBehaviour_OnLoadingSkillAttack;
    }


    private void HostileBehaviour_OnEnterCombat ( object sender, System.EventArgs e )
    {
        animator.SetLayerWeight(COMBAT_LAYER, 1);
    }
    private void HostileBehaviour_OnExitCombat ( object sender, System.EventArgs e )
    {
        animator.SetLayerWeight(COMBAT_LAYER, 0);
    }
    #region HostileBehaviour
    private void AI_HostileBehaviour_OnBasicAttack ( object sender, System.EventArgs e )
    {
        basicAttackIndex = (int)UnityEngine.Random.Range(0, numberOfBasicAttacks);

        if (basicAttackIndex > numberOfBasicAttacks)
        {
            basicAttackIndex = 0;
        }

        animator.SetTrigger(ATTACK_TRIGGER_FLOAT);
        animator.SetFloat(ATTACK_INDEX_FLOAT, basicAttackIndex);

        basicAttackIndex += 1;
    }
    private void AI_HostileBehaviour_OnChargedAttack ( object sender, System.EventArgs e )
    {
        animator.SetBool(CHARGED_ATTACK_BOOL, true);
    }
    private void AI_HostileBehaviour_OnLoadingChargedAttack ( object sender, AI_HostileBehaviour.OnLoadingAttackEventArgs e )
    {
        chargedAttackIndex = (int)UnityEngine.Random.Range(0, numberOfChargedAttacks);

        if (chargedAttackIndex > numberOfChargedAttacks)
        {
            chargedAttackIndex = 0;
        }

        if (!loadingChargedAttack)
        {
            animator.SetTrigger(RECHARGING_TRIGGER);
            loadingChargedAttack = true;
        }
        if (e.progressNormalized <= 0)
        {
            hostileBehaviour.OnChargedAttack += AI_HostileBehaviour_OnChargedAttack;
        }
        animator.SetFloat(ATTACK_INDEX_FLOAT, chargedAttackIndex);
        chargedAttackIndex += 1;
    }
    private void AI_HostileBehaviour_OnSpecialAttack ( object sender, System.EventArgs e )
    {
        animator.SetTrigger(SPECIAL_ATTACK_TRIGGER);
    }
    private void AI_HostileBehaviour_OnLoadingSpecialAttack ( object sender, AI_HostileBehaviour.OnLoadingSpecialEventArgs e )
    {
        specialAttackIndex = (int)UnityEngine.Random.Range(0, numberOfSpecialAttacks);

        if (specialAttackIndex > numberOfSpecialAttacks)
        {
            specialAttackIndex = 0;
        }

        if (!loadingSkillAttack)
        {
            animator.SetTrigger(CASTING_SKILL_TRIGGER);
            loadingSkillAttack = true;
        }
        if (e.progressNormalized <= 0)
        {
            hostileBehaviour.OnSpecialAttack += AI_HostileBehaviour_OnSpecialAttack;
        }
        animator.SetFloat(ATTACK_INDEX_FLOAT, specialAttackIndex);
        specialAttackIndex += 1;
    }
    private void AI_HostileBehaviour_OnSkillAttack ( object sender, System.EventArgs e )
    {
        animator.SetBool(SKILL_ATTACK_BOOL, true);
        
    }
    private void AI_HostileBehaviour_OnLoadingSkillAttack ( object sender, AI_HostileBehaviour.OnLoadingSkillEventArgs e )
    {
        skillAttackIndex = (int)UnityEngine.Random.Range(0, numberOfSkillAttacks);

        if (skillAttackIndex > numberOfSkillAttacks)
        {
            skillAttackIndex = 0;
        }

        if (!loadingSkillAttack)
        {
            animator.SetTrigger(CASTING_SKILL_TRIGGER);
            loadingSkillAttack = true;
        }
        if (e.progressNormalized <= 0)
        {
            hostileBehaviour.OnSkillAttack += AI_HostileBehaviour_OnSkillAttack;
        }
        animator.SetFloat(ATTACK_INDEX_FLOAT, skillAttackIndex);
        skillAttackIndex += 1;
    }
    #endregion

    private void AI_PatrolBehaviour_OnWalkAction ( object sender, AI_PatrolBehaviour.OnWalkActionEventArgs e )
    {
        isWalking = e.isWalking;
    }

    private void AI_ChasingBehaviour_OnRunAction ( object sender, AI_ChasingBehaviour.OnRunActionEventArgs e )
    {
        isRunning = e.isRunning;
        isWalking = e.isWalking;
    }


    private void Update ( )
    {
        if (DetectAnimationEnding(COMBAT_LAYER, ANIMATION_ATTACK_SKILL_TREE_PERFORMED_NAME))
        {
            loadingSkillAttack = false;

            animator.SetBool(SKILL_ATTACK_BOOL, false);
        }
        else if (DetectAnimationEnding(COMBAT_LAYER, ANIMATION_ATTACK_CHARGED_TREE_PERFORMED_NAME))
        {
            loadingChargedAttack = false;

            animator.SetBool(CHARGED_ATTACK_BOOL, false);
        }
        animator.SetBool(IS_RUNNING_BOOL, isRunning);
        animator.SetBool(IS_WALKING_BOOL, isWalking);

    }
    private bool DetectAnimationEnding( int layerIndex, string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(COMBAT_LAYER).IsName(animationName))
        {
            if (animator.GetCurrentAnimatorStateInfo(COMBAT_LAYER).normalizedTime >= 0.95f) /* el valor varia segun la duracion de transicion, en este caso la salida sera en el segundo 0.95 y tardara  
                                                                                                   * 0,05 en hacer la transicion a otro estado */
            {
                return true;
            }
            print(animationName + " animation has been released;");
        }
        return false;
    }
    public AnimatorStateInfo GetCurrentAnimationInfo ( int layer, string animationName )
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);

        if (currentStateInfo.IsName(animationName)) return currentStateInfo;
        else return currentStateInfo;
    }
    public void OnAnimationEvent_AttackCallback ( )
    {
        ai_MainCore.GetHostileBehaviour().SetHit(true);
    }
    public void OnAnimationEvent_AttackEffectCallback ( )
    {
        print("b");
    }
}
