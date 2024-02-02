using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private const string IS_WALKING_BOOL = "IsWalking";
    private const string IS_RUNNING_BOOL = "IsRunning";
    private const string ATTACK_TRIGGER_FLOAT = "Attack";
    private const string ATTACK_INDEX = "AttackIndex";
    private const string CHARGED_ATTACK_BOOL = "ChargedAttack";
    private const string RECHARGING_TRIGGER = "ChargingAttack";
    private const string SPECIAL_ATTACK_TRIGGER = "SpecialAttack";
    private const string SKILL_ATTACK_BOOL = "SkillAttack";
    private const string CASTING_SKILL_TRIGGER = "SkillCastingAttack";
    private const string DEATH_TRIGGER = "Death";

    private const int BASE_LAYER_INDEX = 0;
    private const int ATTACK_LAYER_INDEX = 1;
    private const int ADDITIVE_EFFECTS_LAYER_INDEX = 2;

    private const string ATTACK_COMBO_ANIMATION_NAME = "AttackCombo";
    private const string CHARGED_ATTACK_ANIMATION_NAME = "ChargedAttack";
    private const string SKILL_ATTACK_ANIMATION_NAME = "SkillAttack";

    [SerializeField] private int numberOfAttacks = 4;
    [SerializeField] private AI_MainCore ai_MainCore;
    
    private bool isWalking;
    private bool isRunning;
    private float basicAttackIndex;
    private float chargedAttackIndex;
    private float specialAttackIndex;
    private float skillAttackIndex;
    private Animator animator;
    private AI_PatrolBehaviour patrolBehaviour;
    private AI_ChasingBehaviour chasingBehaviour;
    private AI_HostileBehaviour hostileBehaviour;
    private bool chargingAttack;
    private bool castingSkill;
    private void Awake ( )
    {
        animator = GetComponent<Animator>();
        SetBehaviours();
        //ai_MainCore = GetComponentInParent<AI_MainCore>();
    }

    private void Start ( )
    {
        patrolBehaviour.OnWalkAction += AI_PatrolBehaviour_OnWalkAction;
        chasingBehaviour.OnRunAction += AI_ChasingBehaviour_OnRunAction;
        hostileBehaviour.OnEnterCombat += HostileBehaviour_OnEnterCombat;
        hostileBehaviour.OnExitCombat += HostileBehaviour_OnExitCombat;
        hostileBehaviour.OnBasicAttack += AI_HostileBehaviour_OnBasicAttack;
        hostileBehaviour.OnChargingAttack += AI_HostileBehaviour_OnChargingAttack;
        hostileBehaviour.OnSpecialAttack += AI_HostileBehaviour_OnSpecialAttack;
        hostileBehaviour.OnCastingSkill += AI_HostileBehaviour_OnCastingSkill1;
    }


    private void HostileBehaviour_OnEnterCombat ( object sender, System.EventArgs e )
    {
        animator.SetLayerWeight(ATTACK_LAYER_INDEX, 1);
        animator.SetLayerWeight(BASE_LAYER_INDEX, 0);
    }
    private void HostileBehaviour_OnExitCombat ( object sender, System.EventArgs e )
    {
        animator.SetLayerWeight(ATTACK_LAYER_INDEX, 0);
        animator.SetLayerWeight(BASE_LAYER_INDEX, 1);
    }
    #region HostileBehaviour
    private void AI_HostileBehaviour_OnBasicAttack ( object sender, System.EventArgs e )
    {
        basicAttackIndex = (int)UnityEngine.Random.Range(1, numberOfAttacks);

        if (basicAttackIndex > numberOfAttacks)
        {
            basicAttackIndex = 0;
        }

        animator.SetTrigger(ATTACK_TRIGGER_FLOAT);
        animator.SetFloat(ATTACK_INDEX, basicAttackIndex);

        basicAttackIndex += 1;
    }
    private void AI_HostileBehaviour_OnChargedAttack ( object sender, System.EventArgs e )
    {
        animator.SetBool(CHARGED_ATTACK_BOOL, true);
    }
    private void AI_HostileBehaviour_OnChargingAttack ( object sender, AI_HostileBehaviour.OnChargingAttackEventArgs e )
    {
        if (!chargingAttack)
        {
            animator.SetTrigger(RECHARGING_TRIGGER);
            chargingAttack = true;
        }
        if (e.progressNormalized <= 0)
        {
            hostileBehaviour.OnChargedAttack += AI_HostileBehaviour_OnChargedAttack;
        }
    }
    private void AI_HostileBehaviour_OnSpecialAttack ( object sender, System.EventArgs e )
    {
        animator.SetTrigger(SPECIAL_ATTACK_TRIGGER);
    }
    private void AI_HostileBehaviour_OnSkillAttack ( object sender, System.EventArgs e )
    {
        animator.SetBool(SKILL_ATTACK_BOOL, true);
        
    }
    private void AI_HostileBehaviour_OnCastingSkill1 ( object sender, AI_HostileBehaviour.OnCastingSkillEventArgs e )
    {
        if (!castingSkill)
        {
            animator.SetTrigger(CASTING_SKILL_TRIGGER);
            castingSkill = true;
        }
        if (e.progressNormalized <= 0)
        {
            hostileBehaviour.OnSkillAttack += AI_HostileBehaviour_OnSkillAttack;
        }
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
        if(DetectAnimationEnding(ATTACK_LAYER_INDEX, ATTACK_COMBO_ANIMATION_NAME))
        {

        }
        else if (DetectAnimationEnding(ATTACK_LAYER_INDEX, SKILL_ATTACK_ANIMATION_NAME))
        {
            castingSkill = false;

            animator.SetBool(SKILL_ATTACK_BOOL, false);
        }
        else if (DetectAnimationEnding(ATTACK_LAYER_INDEX, CHARGED_ATTACK_ANIMATION_NAME))
        {
            chargingAttack = false;

            animator.SetBool(CHARGED_ATTACK_BOOL, false);
        }
        animator.SetBool(IS_RUNNING_BOOL, isRunning);
        animator.SetBool(IS_WALKING_BOOL, isWalking);

    }
    private bool DetectAnimationEnding( int layerIndex, string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(ATTACK_LAYER_INDEX).IsName(animationName))
        {
            if (animator.GetCurrentAnimatorStateInfo(ATTACK_LAYER_INDEX).normalizedTime >= 1f)
            {
                return true;
            }
            print(animationName + " animation has been released;");
        }
        return false;
    }
    private bool DetectAnimationIntro( int layerIndex, string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(ATTACK_LAYER_INDEX).IsName(animationName))
        {
            if (animator.GetCurrentAnimatorStateInfo(ATTACK_LAYER_INDEX).normalizedTime != 0f)
            {
                return true;
            }
            print(animationName + " animation has been released;");
        }
        return false;
    }
    private bool DetectAnimationHalfWay( int layerIndex, string animationName)
    {
        if (animator.GetCurrentAnimatorStateInfo(ATTACK_LAYER_INDEX).IsName(animationName))
        {
            if (animator.GetCurrentAnimatorStateInfo(ATTACK_LAYER_INDEX).normalizedTime > 0.5f)
            {
                return true;
            }
            print(animationName + " animation has been released;");
        }
        return false;
    }
    private void SetBehaviours ( )
    {
        patrolBehaviour = ai_MainCore.GetPatrolBehaviour();
        chasingBehaviour = ai_MainCore.GetChasingBehaviour();
        hostileBehaviour = ai_MainCore.GetHostileBehaviour();
    }
}
