using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationsContainer", menuName = "Dungeon Survival/Animations/EnemyAnimations")]
public class EnemyAnimationContainerSO : AnimationClipContainerSO
{
    public AnimationClip eDeathBackClip;
    public AnimationClip eIdleClip;
    public AnimationClip eIdleCombatClip;
    public AnimationClip eRunForwardClip;
    public AnimationClip eWalkForwardClip;

    public const string E_DEATHBACK = "E_DeathBack";
    public const string E_IDLE = "E_Idle";
    public const string E_RUN_FORWARD = "E_Run_Forward";
    public const string E_WALK_FORWARD = "E_Walk_Forward";
    public const string E_IDLECOMBAT = "E_IdleCombat";
    
    
    public void ChangeCurrentAnimations ( AnimatorOverrideController animatorOverrideController )
    {
        LoadBasicAttackAnimationsOnOverride(animatorOverrideController);
        LoadChargedAttackAnimationsOnOverride(animatorOverrideController);
        LoadSkillAttackAnimationsOnOverride(animatorOverrideController);

        animatorOverrideController[E_DEATHBACK] = eDeathBackClip;
        animatorOverrideController[E_IDLE] = eIdleClip;
        animatorOverrideController[E_IDLECOMBAT] = eIdleCombatClip;
        animatorOverrideController[E_RUN_FORWARD] = eRunForwardClip;
        animatorOverrideController[E_WALK_FORWARD] = eWalkForwardClip;

    }
}
