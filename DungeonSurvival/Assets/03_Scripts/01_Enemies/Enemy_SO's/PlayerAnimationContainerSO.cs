using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationsContainer", menuName = "Dungeon Survival/Animations/PlayerAnimations")]
public class PlayerAnimationContainerSO : AnimationClipContainerSO
{
    public AnimationClip deathBackwardClip;
    public AnimationClip idleClip;
    public AnimationClip idleCombatClip;

    public const string DEATH = "DeathBack";
    public const string IDLE = "Idle";
    public const string IDLECOMBAT = "IdleCombat";

    //MIRAR SI ALFINAL ESTAS CAMBIARAN O NO
    public AnimationClip runForwardClip;
    public AnimationClip walkForwardClip;
    public const string RUN_FORWARD = "Run_Forward";
    public const string WALK_FORWARD = "Walk_Forward";

    public void ChangeCurrentAnimations ( AnimatorOverrideController animatorOverrideController )
    {
        LoadBasicAttackAnimationsOnOverride(animatorOverrideController);
        LoadChargedAttackAnimationsOnOverride(animatorOverrideController);
        LoadSkillAttackAnimationsOnOverride(animatorOverrideController);

        animatorOverrideController[DEATH] = deathBackwardClip;
        animatorOverrideController[IDLE] = idleClip;
        animatorOverrideController[IDLECOMBAT] = idleCombatClip;
        animatorOverrideController[RUN_FORWARD] = runForwardClip;
        animatorOverrideController[WALK_FORWARD] = walkForwardClip;

    }
}
