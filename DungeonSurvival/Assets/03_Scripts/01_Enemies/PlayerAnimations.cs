using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("ANIMATOR LAYER")]
    private const int _COMBAT_LAYER = 1;
    public int COMBAT_LAYER { get { return _COMBAT_LAYER; } }

    [Header("ANIMATOR PARAMETERS")]
    private const string START_LOADING_TRIGGER = "StartLoading";
    private const string BASIC_ATTACK_TRIGGER = "BasicAttack";
    private const string CHARGED_ATTACK_TRIGGER = "ChargedAttack";
    private const string SPECIAL_ATTACK_TRIGGER = "SpecialAttack";
    private const string SKILL_ATTACK_TRIGGER = "SkillAttack";

    private const string LOADING_CHARGED_ATTACK_BOOL = "LoadingChargedAttack";
    private const string LOADING_SPECIAL_ATTACK_BOOL = "LoadingSpecialAttack";
    private const string LOADING_SKILL_ATTACK_BOOL = "LoadingSkillAttack";

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
    
    private float attackCombo_resetTime;

    private int basicAttackIndex;
    private int chargedAttackIndex;
    private int specialAttackIndex;
    private int skillAttackIndex;
    private float maxNumberOfBasicAttacks => playerAnimationHandler.GetAnimationClipContainerSO.maxNumberOfBasicAttacks - 1;
    private float maxNumberOfChachargedAttackIndexrgedAttacks => playerAnimationHandler.GetAnimationClipContainerSO.maxNumberOfChargedAttacks - 1;
    private float maxNumberOfSpecialAttacks => playerAnimationHandler.GetAnimationClipContainerSO.maxNumberOfSpecialAttacks - 1;
    private float maxNumberOfSkillAttacks => playerAnimationHandler.GetAnimationClipContainerSO.maxNumberOfSkillAttacks - 1;

    private bool attackAnimationTriggered;
    private AnimatorStateInfo currentCombatAnimatorState => SelectCurrentAnimatorState(COMBAT_LAYER);
    private AttackCategory attackCategory = AttackCategory.basic;
    public Animator animator => _animator;
    private Animator _animator;

    private PlayerCombat playerCombat;
    private PlayerAnimationHandler playerAnimationHandler;

    private void Awake ( )
    {
        playerCombat = GetComponent<PlayerCombat>();
        _animator = GetComponent<Animator>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
    }

    private void Start ( )
    {
        playerCombat.OnBasicAttackPerformed += PlayerCombat_OnBasicAttackPerformed;
        playerCombat.OnLoadingChargedAttackPerformed += PlayerCombat_OnLoadingChargedAttackPerformed;
        playerCombat.OnChargedAttackPerformed += PlayerCombat_OnChargedAttackPerformed;
        //playerCombat.OnSpecialAttackPerformed += 
        playerCombat.OnLoadingSkillAttackPerformed += PlayerCombat_OnLoadingSkillAttackPerformed;
        playerCombat.OnSkillAttackPerformed += PlayerCombat_OnSkillAttackPerformed;
        playerCombat.OnLoadCancelled += PlayerCombat_OnLoadCancelled;
    }

    private void PlayerCombat_OnLoadCancelled ( object sender, System.EventArgs e )
    {
        _animator.SetBool(LOADING_CHARGED_ATTACK_BOOL, false);
        _animator.SetBool(LOADING_SPECIAL_ATTACK_BOOL, false);
        _animator.SetBool(LOADING_SKILL_ATTACK_BOOL, false);
    }

    private void PlayerCombat_OnBasicAttackPerformed ( object sender, System.EventArgs e )
    {
        attackCategory = AttackCategory.basic;
        OnBasicAttack();
    }
    private void PlayerCombat_OnLoadingChargedAttackPerformed ( object sender, PlayerCombat.OnAttackIndexEventArgs e )
    {
        if(playerCombat.IsLoadingAttack())
        {
            _animator.SetLayerWeight(COMBAT_LAYER, 1f);
            chargedAttackIndex = (int)e.index;
            _animator.SetFloat(CHARGED_ATTACK_INDEX_FLOAT, chargedAttackIndex + 1);
            _animator.SetBool(LOADING_CHARGED_ATTACK_BOOL, true);
            _animator.SetTrigger(START_LOADING_TRIGGER);
        }
    }

    private void PlayerCombat_OnChargedAttackPerformed ( object sender, System.EventArgs e )
    {
        OnChargedAttackPerformed();
    }
    private void PlayerCombat_OnLoadingSkillAttackPerformed ( object sender, PlayerCombat.OnAttackIndexEventArgs e )
    {
        throw new System.NotImplementedException();
    }
    private void PlayerCombat_OnSkillAttackPerformed ( object sender, System.EventArgs e )
    {
        throw new System.NotImplementedException();
    }

    private void Update ( )
    {
        if (attackAnimationTriggered)
        {
            if (currentCombatAnimatorState.normalizedTime > 0.95f)
            {
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
                else if(basicAttackIndex > maxNumberOfBasicAttacks)
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
        return _animator;
    }
    public bool IsStateActive ( AnimatorStateInfo state )
    {
        return _animator.GetCurrentAnimatorStateInfo(COMBAT_LAYER).fullPathHash == state.fullPathHash;
    }
    public AnimatorStateInfo SelectCurrentAnimatorState ( int layer )
    {
        AnimatorStateInfo[] states = { GetCurrentAnimationInfo(layer, ANIMATION_STATE_BASIC_ATTACK_TREE_PERFORMED_NAME),
                                       GetCurrentAnimationInfo(layer, ANIMATION_STATE_CHARGED_ATTACK_TREE_PERFORMED_NAME),
                                       GetCurrentAnimationInfo(layer, ANIMATION_STATE_SPECIAL_ATTACK_TREE_PERFORMED_NAME),
                                       GetCurrentAnimationInfo(layer, ANIMATION_STATE_SKILL_ATTACK_TREE_PERFORMED_NAME) };
        foreach (AnimatorStateInfo state in states)
        {
            if(state.fullPathHash == _animator.GetCurrentAnimatorStateInfo(COMBAT_LAYER).fullPathHash)
            {
                return state;
            }
        }
        return states[0];
    }
    private AnimatorStateInfo GetCurrentAnimationInfo ( int layer, string animationName )
    {
        AnimatorStateInfo currentStateInfo = _animator.GetCurrentAnimatorStateInfo(layer);

        if (currentStateInfo.IsName(animationName)) return currentStateInfo;
        else return currentStateInfo;
    }
    private void ResetAttackCombo ( )
    {
        basicAttackIndex = 0;
        attackCombo_resetTime = 0;
        attackAnimationTriggered = false;
        _animator.SetLayerWeight(COMBAT_LAYER, 0f);
    }
    private void OnBasicAttack ( )
    {
        _animator.SetLayerWeight(COMBAT_LAYER, 1f);

        if (!attackAnimationTriggered)
        {
            _animator.SetTrigger(BASIC_ATTACK_TRIGGER);
            _animator.SetFloat(BASIC_ATTACK_INDEX_FLOAT, basicAttackIndex);
            attackCombo_resetTime = 0;
            attackAnimationTriggered = true;
        }
    }
    private void OnChargedAttackPerformed ( )
    {
        _animator.SetLayerWeight(COMBAT_LAYER, 1f);
        
        if (!attackAnimationTriggered)
        {
            _animator.SetTrigger(CHARGED_ATTACK_TRIGGER);
            attackCombo_resetTime = 0;
            attackAnimationTriggered = true;
        }
    }
    private void OnSpecialAttackPerformed ( )
    {
        _animator.SetLayerWeight(COMBAT_LAYER, 1f);

        if (!attackAnimationTriggered)
        {
            _animator.SetTrigger(SPECIAL_ATTACK_TRIGGER);
            _animator.SetFloat(BASIC_ATTACK_INDEX_FLOAT, basicAttackIndex);
            attackCombo_resetTime = 0;
            attackAnimationTriggered = true;
        }
    }
    private void OnSkillAttackPerformed ( )
    {
        _animator.SetLayerWeight(COMBAT_LAYER, 1f);

        if (!attackAnimationTriggered)
        {
            _animator.SetTrigger(SKILL_ATTACK_TRIGGER);
            _animator.SetFloat(BASIC_ATTACK_INDEX_FLOAT, basicAttackIndex);
            attackCombo_resetTime = 0;
            attackAnimationTriggered = true;
        }
    }
}