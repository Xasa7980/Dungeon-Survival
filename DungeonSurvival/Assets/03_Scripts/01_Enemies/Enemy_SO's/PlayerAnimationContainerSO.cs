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

    public void ChangeCurrentAnimations ( AnimatorOverrideController animatorOverrideController, EquipmentDataSO equipmentDataSO )
    {
        
        LoadBasicAttackAnimationsOnOverride(animatorOverrideController, equipmentDataSO.equipmentAnimationClips.basicAttackClips);
        LoadChargedAttackAnimationsOnOverride(animatorOverrideController, equipmentDataSO.equipmentAnimationClips.chargedAttackClips);
        LoadSpecialAttackAnimationsOnOverride(animatorOverrideController, equipmentDataSO.equipmentAnimationClips.specialAttackClips);
        LoadSkillAttackAnimationsOnOverride(animatorOverrideController, equipmentDataSO.equipmentAnimationClips.skillAttackClips);

        animatorOverrideController[DEATH] = deathBackwardClip;
        animatorOverrideController[IDLE] = idleClip;
        animatorOverrideController[IDLECOMBAT] = idleCombatClip;
        animatorOverrideController[RUN_FORWARD] = runForwardClip;
        animatorOverrideController[WALK_FORWARD] = walkForwardClip;

    }
    public void DualSwordOverride ( AnimatorOverrideController animatorOverrideController )
    {

        LoadBasicAttackAnimationsOnOverride(animatorOverrideController, basicAttackDualSwordWeapon);
        LoadChargedAttackAnimationsOnOverride(animatorOverrideController, chargedAttackDualSwordWeapon);
        LoadSpecialAttackAnimationsOnOverride(animatorOverrideController, specialAttackDualSwordWeapon);
        LoadSkillAttackAnimationsOnOverride(animatorOverrideController, skillAttackDualSwordWeapon);
    }
    public void DualDaggerOverride ( AnimatorOverrideController animatorOverrideController )
    {

        LoadBasicAttackAnimationsOnOverride(animatorOverrideController, basicAttackDualDaggerWeapon);
        LoadChargedAttackAnimationsOnOverride(animatorOverrideController, chargedAttackDualDaggerWeapon);
        LoadSpecialAttackAnimationsOnOverride(animatorOverrideController, specialAttackDualDaggerWeapon);
        LoadSkillAttackAnimationsOnOverride(animatorOverrideController, skillAttackDualDaggerWeapon);
    }
    public void ShieldOverride ( AnimatorOverrideController animatorOverrideController )
    {

        LoadBasicAttackAnimationsOnOverride(animatorOverrideController, basicAttackShield);
        LoadChargedAttackAnimationsOnOverride(animatorOverrideController, chargeAttackShield);
        LoadSpecialAttackAnimationsOnOverride(animatorOverrideController, specialAttackShield);
        LoadSkillAttackAnimationsOnOverride(animatorOverrideController, skillAttackShield);
    }
    public void NoWeaponOverride ( AnimatorOverrideController animatorOverrideController )
    {

        LoadBasicAttackAnimationsOnOverride(animatorOverrideController, basicAttackNoWeapons);
        LoadChargedAttackAnimationsOnOverride(animatorOverrideController, chargedAttackNoWeapons);
        LoadSpecialAttackAnimationsOnOverride(animatorOverrideController, specialAttackNoWeapons);
        LoadSkillAttackAnimationsOnOverride(animatorOverrideController, skillAttackNoWeapons);
    }
}
