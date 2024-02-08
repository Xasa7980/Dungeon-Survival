using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("ANIMATOR LAYER")]
    private const int _COMBAT_LAYER = 1;
    public int COMBAT_LAYER { get { return _COMBAT_LAYER; } }

    [Header("ANIMATOR PARAMETERS")]
    private const string ATTACK_BASIC_TRIGGER = "BasicAttack";
    private const string ATTACK_CHARGED_TRIGGER = "ChargedAttack";
    private const string ATTACK_SPECIAL_TRIGGER = "SpecialAttack";
    private const string ATTACK_SKILL_TRIGGER = "SkillAttack";

    private const string BASIC_ATTACK_INDEX_FLOAT = "BasicAttackIndex";
    private const string CHARGED_ATTACK_INDEX_FLOAT = "ChargedAttackIndex";
    private const string SPECIAL_ATTACK_INDEX_FLOAT = "SpecialAttackIndex";
    private const string SKILL_ATTACK_INDEX_FLOAT = "SkillAttackIndex";

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

    [SerializeField] private float attackCombo_resetTimeMax;
    [SerializeField] private float totalBasicAttackAnimatios = 2;

    private float attackCombo_resetTime;

    private int basicAttackIndex;
    private int chargedAttackIndex;
    private int specialAttackIndex;
    private int skillAttackIndex;
    private AnimatorStateInfo currentCombatAnimatorState => SelectCurrentAnimatorState(COMBAT_LAYER);
    private Animator animator;
    private PlayerCombat playerCombat;
    private bool attackAnimationTriggered;

    private AttackCategory attackCategory = AttackCategory.basic;
    private void Awake ( )
    {
        playerCombat = GetComponent<PlayerCombat>();
        animator = GetComponent<Animator>();
    }

    private void Start ( )
    {
        playerCombat.OnBasicAttackPerformed += PlayerCombat_OnBasicAttackPerformed;
    }

    private void PlayerCombat_OnBasicAttackPerformed ( object sender, System.EventArgs e )
    {
        attackCategory = AttackCategory.basic;
        OnBasicAttack();
    }

    private void Update ( )
    {
        if (attackAnimationTriggered)
        {
            

            if (currentCombatAnimatorState.normalizedTime > 0.95f)
            {
                animator.SetLayerWeight(COMBAT_LAYER, 0f);

                if (attackAnimationTriggered)
                {
                    basicAttackIndex ++;
                    attackAnimationTriggered = false;
                }
                attackCombo_resetTime += Time.deltaTime;
                if (attackCombo_resetTime > attackCombo_resetTimeMax)
                {
                    ResetAttackCombo();
                }
                else if(basicAttackIndex > totalBasicAttackAnimatios)
                {
                    ResetAttackCombo();
                }
            }
        }
        else
        {
            attackCombo_resetTime += Time.deltaTime;
            if (attackCombo_resetTime > attackCombo_resetTimeMax)
            {
                ResetAttackCombo();
            }
        }
    }
    public Animator GetAnimator ( )
    {
        return animator;
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
            if(state.fullPathHash == animator.GetCurrentAnimatorStateInfo(COMBAT_LAYER).fullPathHash)
            {
                return state;
            }
        }
        return states[0];
    }
    private AnimatorStateInfo GetCurrentAnimationInfo ( int layer, string animationName )
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);

        if (currentStateInfo.IsName(animationName)) return currentStateInfo;
        else return currentStateInfo;
    }
    private void ResetAttackCombo ( )
    {
        basicAttackIndex = 0;
        attackCombo_resetTime = 0;
        attackAnimationTriggered = false;
        animator.SetLayerWeight(COMBAT_LAYER, 0f);
    }
    private void OnBasicAttack ( )
    {
        if (!attackAnimationTriggered)
        {
            animator.SetTrigger(ATTACK_BASIC_TRIGGER);
            animator.SetFloat(BASIC_ATTACK_INDEX_FLOAT, basicAttackIndex);
            attackCombo_resetTime = 0;
            attackAnimationTriggered = true;
        }

        animator.SetLayerWeight(COMBAT_LAYER, 1f);
    }
    private void OnChargedAttackPerformed ( )
    {
        if (!attackAnimationTriggered)
        {
            animator.SetTrigger(ATTACK_CHARGED_TRIGGER);
            animator.SetFloat(CHARGED_ATTACK_INDEX_FLOAT, basicAttackIndex);
            attackCombo_resetTime = 0;
            attackAnimationTriggered = true;
        }

        animator.SetLayerWeight(COMBAT_LAYER, 1f);
    }
    private void OnSpecialAttackPerformed ( )
    {
        if (!attackAnimationTriggered)
        {
            animator.SetTrigger(ATTACK_SPECIAL_TRIGGER);
            animator.SetFloat(BASIC_ATTACK_INDEX_FLOAT, basicAttackIndex);
            attackCombo_resetTime = 0;
            attackAnimationTriggered = true;
        }

        animator.SetLayerWeight(COMBAT_LAYER, 1f);
    }
    private void OnSkillAttackPerformed ( )
    {
        if (!attackAnimationTriggered)
        {
            animator.SetTrigger(ATTACK_SKILL_TRIGGER);
            animator.SetFloat(BASIC_ATTACK_INDEX_FLOAT, basicAttackIndex);
            attackCombo_resetTime = 0;
            attackAnimationTriggered = true;
        }

        animator.SetLayerWeight(COMBAT_LAYER, 1f);
    }
}