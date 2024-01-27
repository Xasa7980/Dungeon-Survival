using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private const int COMBAT_LAYER = 1;

    private const string ATTACK_BASIC_TRIGGER = "AttackBasic";
    private const string ATTACK_BASIC_INDEX_FLOAT = "AttackIndex";
    private const string _ATTACK_BASIC_TREE_NAME = "AttackBasicTree";

    public string ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME { get { return _ATTACK_BASIC_TREE_NAME; } }

    [SerializeField] private float attackCombo_resetTimeMax;
    [SerializeField] private float totalBasicAttackAnimatios = 2;

    private float attackCombo_resetTime;
    private int attackBasic_Index;
    private Animator animator;
    private PlayerCombat playerCombat;
    private bool animationTriggered;

    private void Awake ( )
    {
        playerCombat = GetComponent<PlayerCombat>();
        animator = GetComponent<Animator>();
    }

    private void Start ( )
    {
        playerCombat.OnBasicAttack += PlayerCombat_OnBasicAttack;
    }

    private void PlayerCombat_OnBasicAttack ( object sender, System.EventArgs e )
    {
        OnBasicAttack();
    }

    private void Update ( )
    {
        if (animationTriggered)
        {
            if (GetCurrentAnimationInfo(ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME).normalizedTime > 0.85f)
            {
                if (animationTriggered)
                {
                    attackBasic_Index += 1;
                    animationTriggered = false;
                }
                attackCombo_resetTime += Time.deltaTime;
                if (attackCombo_resetTime > attackCombo_resetTimeMax)
                {
                    ResetAttackCombo();
                }
                else if(attackBasic_Index > totalBasicAttackAnimatios)
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

    public AnimatorStateInfo GetCurrentAnimationInfo ( string animationName )
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(COMBAT_LAYER);
        if (currentStateInfo.IsName(animationName)) return currentStateInfo;
        else return currentStateInfo;
    }

    private void ResetAttackCombo ( )
    {
        attackBasic_Index = 0;
        attackCombo_resetTime = 0;
        animationTriggered = false;
        animator.SetLayerWeight(COMBAT_LAYER, 0f);
    }

    private void OnBasicAttack ( )
    {
        if (!animationTriggered)
        {
            animator.SetTrigger(ATTACK_BASIC_TRIGGER);
            animator.SetFloat(ATTACK_BASIC_INDEX_FLOAT, attackBasic_Index);
            attackCombo_resetTime = 0;
            animationTriggered = true;
        }

        animator.SetLayerWeight(COMBAT_LAYER, 1f);
    }

    public float GetAnimation_RemainingTime ( string animationName )
    {
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(COMBAT_LAYER);
        if (currentStateInfo.IsName(animationName))
        {
            return currentStateInfo.normalizedTime;
        }
        else return 1f;
    }
}