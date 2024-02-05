using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("ANIMATOR LAYER")]
    private const int _COMBAT_LAYER = 1;
    public int COMBAT_LAYER { get { return _COMBAT_LAYER; } }

    [Header("ANIMATOR PARAMETERS")]
    private const string ATTACK_BASIC_TRIGGER = "AttackBasic";
    private const string ATTACK_CHARGED_TRIGGER = "AttackCharged";
    private const string ATTACK_SPECIAL_TRIGGER = "AttackSpecial";
    private const string ATTACK_SKILL_TRIGGER = "AttackSkill";

    private const string ATTACK_BASIC_INDEX_FLOAT = "AttackIndex";

    [Header ("ANIMATOR STATE NAME")]
    private const string _ATTACK_BASIC_TREE_NAME = "AttackBasicTree";
    public string ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME { get { return _ATTACK_BASIC_TREE_NAME; } }

    private const string _ATTACK_CHARGED_TREE_NAME = "AttackBasicTree";
    public string ANIMATION_ATTACK_CHARGED_TREE_PERFORMED_NAME { get { return _ATTACK_BASIC_TREE_NAME; } }

    private const string _ATTACK_SPECIAL_TREE_NAME = "AttackBasicTree";
    public string ANIMATION_ATTACK_SPECIAL_TREE_PERFORMED_NAME { get { return _ATTACK_BASIC_TREE_NAME; } }

    private const string _ATTACK_SKILL_TREE_NAME = "AttackBasicTree";
    public string ANIMATION_ATTACK_SKILL_TREE_PERFORMED_NAME { get { return _ATTACK_BASIC_TREE_NAME; } }


    [SerializeField] private float attackCombo_resetTimeMax;
    [SerializeField] private float totalBasicAttackAnimatios = 2;

    private float attackCombo_resetTime;
    private int attackIndex;
    private Animator animator;
    private PlayerCombat playerCombat;
    private bool animationTriggered;

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
        if (animationTriggered)
        {
            if (GetCurrentAnimationInfo(COMBAT_LAYER, ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME).normalizedTime > 0.95f)
            {
                animator.SetLayerWeight(COMBAT_LAYER, 0f);

                if (animationTriggered)
                {
                    attackIndex ++;
                    animationTriggered = false;
                }
                attackCombo_resetTime += Time.deltaTime;
                if (attackCombo_resetTime > attackCombo_resetTimeMax)
                {
                    ResetAttackCombo();
                }
                else if(attackIndex > totalBasicAttackAnimatios)
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

    public AnimatorStateInfo GetCurrentAnimationInfo ( int layer, string animationName )
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);

        if (currentStateInfo.IsName(animationName)) return currentStateInfo;
        else return currentStateInfo;
    }
    public bool AnimatorStateInfoIsFinished( int layer, string animationName )
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(layer);

        if (currentStateInfo.IsName(animationName) && currentStateInfo.normalizedTime < 1f) return true;
        else return false;
    }
    public Animator GetAnimator ( )
    {
        return animator;
    }
    private void ResetAttackCombo ( )
    {
        attackIndex = 0;
        attackCombo_resetTime = 0;
        animationTriggered = false;
        animator.SetLayerWeight(COMBAT_LAYER, 0f);
    }
    private void OnBasicAttack ( )
    {
        if (!animationTriggered)
        {
            animator.SetTrigger(ATTACK_BASIC_TRIGGER);
            animator.SetFloat(ATTACK_BASIC_INDEX_FLOAT, attackIndex);
            attackCombo_resetTime = 0;
            animationTriggered = true;
        }

        animator.SetLayerWeight(COMBAT_LAYER, 1f);
    }
    private void OnChargedAttackPerformed ( )
    {
        if (!animationTriggered)
        {
            animator.SetTrigger(ATTACK_CHARGED_TRIGGER);
            animator.SetFloat(ATTACK_BASIC_INDEX_FLOAT, attackIndex);
            attackCombo_resetTime = 0;
            animationTriggered = true;
        }

        animator.SetLayerWeight(COMBAT_LAYER, 1f);
    }
    private void OnSpecialAttackPerformed ( )
    {
        if (!animationTriggered)
        {
            animator.SetTrigger(ATTACK_SPECIAL_TRIGGER);
            animator.SetFloat(ATTACK_BASIC_INDEX_FLOAT, attackIndex);
            attackCombo_resetTime = 0;
            animationTriggered = true;
        }

        animator.SetLayerWeight(COMBAT_LAYER, 1f);
    }
    private void OnSkillAttackPerformed ( )
    {
        if (!animationTriggered)
        {
            animator.SetTrigger(ATTACK_SKILL_TRIGGER);
            animator.SetFloat(ATTACK_BASIC_INDEX_FLOAT, attackIndex);
            attackCombo_resetTime = 0;
            animationTriggered = true;
        }

        animator.SetLayerWeight(COMBAT_LAYER, 1f);
    }
}