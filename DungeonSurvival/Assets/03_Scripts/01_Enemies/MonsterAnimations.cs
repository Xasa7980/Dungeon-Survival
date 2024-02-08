using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimations : MonoBehaviour,ICombatBehaviour
{
    [Header("ANIMATOR LAYER")]
    private const int _COMBAT_LAYER = 1;
    public int COMBAT_LAYER { get { return _COMBAT_LAYER; } }

    [Header("ANIMATOR PARAMETERS")]
    private const string IS_WALKING_BOOL = "IsWalking";
    private const string IS_RUNNING_BOOL = "IsRunning";
    private const string DEATH_TRIGGER = "Death";

    private const string BASIC_ATTACK_TRIGGER = "BasicAttack";
    private const string CHARGED_ATTACK_BOOL = "ChargedAttack";
    private const string SPECIAL_ATTACK_BOOL = "SpecialAttack";
    private const string SKILL_ATTACK_BOOL = "SkillAttack";

    private const string BASIC_ATTACK_INDEX_FLOAT = "BasicAttackIndex";
    private const string CHARGED_ATTACK_INDEX_FLOAT = "ChargedAttackIndex";
    private const string SPECIAL_ATTACK_INDEX_FLOAT = "SpecialAttackIndex";
    private const string SKILL_ATTACK_INDEX_FLOAT = "SkillAttackIndex";

    private const string LOADING_CHARGED_ATTACK_TRIGGER = "LoadingChargedAttack";
    private const string LOADING_SPECIAL_ATTACK_TRIGGER = "LoadingSpecialAttack";
    private const string LOADING_SKILL_ATTACK_TRIGGER = "LoadingSkillAttack";

    #region ANIMATOR STATE NAMES
    public string ANIMATION_STATE_BASIC_ATTACK_TREE_PERFORMED_NAME { get { return BASIC_ATTACK_TREE_NAME; } }
    private const string BASIC_ATTACK_TREE_NAME = "BasicAttack_Tree";

    public string ANIMATION_STATE_CHARGED_ATTACK_TREE_PERFORMED_NAME { get { return CHARGED_ATTACK_TREE_NAME; } }
    private const string CHARGED_ATTACK_TREE_NAME = "ChargedAttack_Tree";

    public string ANIMATION_STATE_SPECIAL_ATTACK_TREE_PERFORMED_NAME { get { return SPECIAL_ATTACK_TREE_NAME; } }
    private const string SPECIAL_ATTACK_TREE_NAME = "SpecialAttack_Tree";

    public string ANIMATION_STATE_SKILL_ATTACK_TREE_PERFORMED_NAME { get { return SKILL_ATTACK_TREE_NAME; } }
    private const string SKILL_ATTACK_TREE_NAME = "SkillAttack_Tree ";


    public string ANIMATION_STATE_LOADING_CHARGED_ATTACK_TREE_PERFORMED_NAME { get { return LOADING_CHARGED_ATTACK_TREE_NAME; } }
    private const string LOADING_CHARGED_ATTACK_TREE_NAME = "LoadingChargedAttack_Tree";

    public string ANIMATION_STATE_LOADING_SPECIAL_ATTACK_TREE_PERFORMED_NAME { get { return LOADING_SPECIAL_ATTACK_TREE_NAME; } }
    private const string LOADING_SPECIAL_ATTACK_TREE_NAME = "LoadingSpecialAttack_Tree";

    public string ANIMATION_STATE_LOADING_SKILL_ATTACK_TREE_PERFORMED_NAME { get { return LOADING_SKILL_ATTACK_TREE_NAME; } }
    private const string LOADING_SKILL_ATTACK_TREE_NAME = "LoadingSkillAttack_Tree  ";
    #endregion

    private AI_MainCore ai_MainCore;
    private MonsterStats monsterStats;
    public EnemyAnimationHandler enemyAnimationHandler => GetComponent<EnemyAnimationHandler>();
    
    private bool isWalking;
    private bool isRunning;

    private int numberOfBasicAttacks => (int)enemyAnimationHandler.AnimationClipContainerSO.maxNumberOfBasicAttacks;
    private int numberOfChargedAttacks => (int)enemyAnimationHandler.AnimationClipContainerSO.maxNumberOfChargedAttacks;
    private int numberOfSpecialAttacks => (int)enemyAnimationHandler.AnimationClipContainerSO.maxNumberOfSpecialAttacks;
    private int numberOfSkillAttacks => (int)enemyAnimationHandler.AnimationClipContainerSO.maxNumberOfSkillAttacks;

    private float basicAttackIndex;
    private float chargedAttackIndex;
    private float specialAttackIndex;
    private float skillAttackIndex;

    private Animator animator;
    private AI_PatrolBehaviour patrolBehaviour;
    private AI_ChasingBehaviour chasingBehaviour;
    private AI_HostileBehaviour hostileBehaviour;
    private AnimatorStateInfo currentAnimatorStateInfo => SelectCurrentAnimatorState(COMBAT_LAYER);
    private bool loadingSpecialAttack;
    private bool loadingChargedAttack;
    private bool loadingSkillAttack;
    private void Awake ( )
    {
        ai_MainCore = GetComponentInParent<AI_MainCore>();
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
        hostileBehaviour.OnChargedAttack += AI_HostileBehaviour_OnChargedAttack;
        hostileBehaviour.OnLoadingSpecialAttack += AI_HostileBehaviour_OnLoadingSpecialAttack;
        hostileBehaviour.OnSpecialAttack += AI_HostileBehaviour_OnSpecialAttack;
        hostileBehaviour.OnCastingSkill += AI_HostileBehaviour_OnLoadingSkillAttack;
        hostileBehaviour.OnSkillAttack += AI_HostileBehaviour_OnSkillAttack;
    }


    private void HostileBehaviour_OnEnterCombat ( object sender, System.EventArgs e )
    {
        animator.SetLayerWeight(COMBAT_LAYER, 1);
    }
    private void HostileBehaviour_OnExitCombat ( object sender, System.EventArgs e )
    {
        print("salida de animaciones");
        animator.SetLayerWeight(COMBAT_LAYER, 0);
    }
    #region HostileBehaviour
    private void AI_HostileBehaviour_OnBasicAttack ( object sender, System.EventArgs e )
    {
        basicAttackIndex = (int)UnityEngine.Random.Range(0, numberOfBasicAttacks);
        animator.SetFloat(BASIC_ATTACK_INDEX_FLOAT, basicAttackIndex);

        animator.SetTrigger(BASIC_ATTACK_TRIGGER);
    }
    private void AI_HostileBehaviour_OnChargedAttack ( object sender, System.EventArgs e )
    {
        animator.SetTrigger(LOADING_CHARGED_ATTACK_TRIGGER);

        print("chargedattacked");
    }
    private void AI_HostileBehaviour_OnLoadingChargedAttack ( object sender, AI_HostileBehaviour.OnLoadingAttackEventArgs e )
    {
        chargedAttackIndex = e.attackIndex;
        animator.SetBool(CHARGED_ATTACK_BOOL, true);
        animator.SetFloat(CHARGED_ATTACK_INDEX_FLOAT, chargedAttackIndex);
        
        print("Loading" + e.progressNormalized + e.attacksDataSO);
    }
    private void AI_HostileBehaviour_OnSpecialAttack ( object sender, System.EventArgs e )
    {
        animator.SetTrigger(LOADING_SPECIAL_ATTACK_TRIGGER);
        print("special_attacked");

    }
    private void AI_HostileBehaviour_OnLoadingSpecialAttack ( object sender, AI_HostileBehaviour.OnLoadingSpecialEventArgs e )
    {
        specialAttackIndex = e.attackIndex;
        animator.SetBool(SPECIAL_ATTACK_BOOL, true);
        animator.SetFloat(SPECIAL_ATTACK_INDEX_FLOAT, specialAttackIndex);
    }
    private void AI_HostileBehaviour_OnSkillAttack ( object sender, System.EventArgs e )
    {
        animator.SetTrigger(LOADING_SKILL_ATTACK_TRIGGER);
        print("skill_attacked");
    }
    private void AI_HostileBehaviour_OnLoadingSkillAttack ( object sender, AI_HostileBehaviour.OnLoadingSkillEventArgs e )
    {

        skillAttackIndex = e.attackIndex;
        animator.SetFloat(SKILL_ATTACK_INDEX_FLOAT, skillAttackIndex);
        animator.SetBool(SKILL_ATTACK_BOOL, true);
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
        animator.SetBool(IS_RUNNING_BOOL, isRunning);
        animator.SetBool(IS_WALKING_BOOL, isWalking);

        //animator.SetBool(CHARGED_ATTACK_BOOL, !isAnimationEnded(currentAnimatorStateInfo,COMBAT_LAYER, ANIMATION_STATE_CHARGED_ATTACK_TREE_PERFORMED_NAME));
        //animator.SetBool(SPECIAL_ATTACK_BOOL, !isAnimationEnded(currentAnimatorStateInfo,COMBAT_LAYER, ANIMATION_STATE_LOADING_SPECIAL_ATTACK_TREE_PERFORMED_NAME));
        //animator.SetBool(SKILL_ATTACK_BOOL, !isAnimationEnded(currentAnimatorStateInfo,COMBAT_LAYER,ANIMATION_STATE_SKILL_ATTACK_TREE_PERFORMED_NAME));
    }
    private bool isAnimationEnded (AnimatorStateInfo currentAnimatorState,int layer, string animationStateName )
    {
        AnimatorStateInfo animatorStateInfo = GetCurrentAnimationInfo(layer, animationStateName);
        if(currentAnimatorState.fullPathHash == animatorStateInfo.fullPathHash)
        {
            if (animatorStateInfo.normalizedTime > 1)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsStateActive ( AnimatorStateInfo state )
    {
        return animator.GetCurrentAnimatorStateInfo(COMBAT_LAYER).fullPathHash == state.fullPathHash;
    }
    public AnimatorStateInfo SelectCurrentAnimatorState ( int layer )
    {
        AnimatorStateInfo[] states = { GetCurrentAnimationInfo(layer, ANIMATION_STATE_BASIC_ATTACK_TREE_PERFORMED_NAME),
                                       GetCurrentAnimationInfo(layer, ANIMATION_STATE_CHARGED_ATTACK_TREE_PERFORMED_NAME),
                                       GetCurrentAnimationInfo(layer, ANIMATION_STATE_SPECIAL_ATTACK_TREE_PERFORMED_NAME),
                                       GetCurrentAnimationInfo(layer, ANIMATION_STATE_SKILL_ATTACK_TREE_PERFORMED_NAME) };
        foreach (AnimatorStateInfo state in states)
        {
            if (state.fullPathHash == animator.GetCurrentAnimatorStateInfo(COMBAT_LAYER).fullPathHash)
            {
                return state;
            }
        }
        return states[0];
    }

    public AnimatorStateInfo GetCurrentAnimationInfo ( int layer, string animationName )
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);

        if (currentStateInfo.IsName(animationName)) return currentStateInfo;
        else return currentStateInfo;
    }
    public void OnAnimationEvent_AttackCallback ( )
    {
        //ai_MainCore.GetHostileBehaviour().SetHit(true);
    }
    public void OnAnimationEvent_AttackEffectCallback ( )
    {
        print("b");
    }
}
