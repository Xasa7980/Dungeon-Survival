using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationsContainer", menuName = "Dungeon Survival/Animations/PlayerAnimations")]
public class PlayerAnimationContainerSO : AnimationClipContainerSO
{
    public AnimationClip deathAnimationClip;
    public AnimationClip idleAnimationClip;
    public AnimationClip idleCombatAnimationClip;

    public const string DEATH_ANIMATION = "Death_Animation";
    public const string IDLE_ANIMATION = "Idle_Animation";
    public const string IDLECOMBAT_ANIMATION = "IdleCombat_Animation";

    [InfoBox("Becareful with these, animations can't be rooted, the player moves with animation movement", InfoMessageType.Warning)]
    public AnimationClip runForwardAnimationClip;
    public AnimationClip walkForwardAnimationClip;
    public const string RUN_FORWARD_ANIMATION = "Run_Forward_Animation";
    public const string WALK_FORWARD_ANIMATION = "Walk_Forward_Animation";

    private void OnValidate ( )
    {
        SettingMaxAnimationsInList();
    }
    public void ChangeCurrentBasicAnimations(AnimatorOverrideController animatorOverrideController )
    {
        SettingMaxAnimationsInList();
        if (deathAnimationClip != null) animatorOverrideController[DEATH_ANIMATION] = deathAnimationClip;
        if (idleAnimationClip != null) animatorOverrideController[IDLE_ANIMATION] = idleAnimationClip;
        if (idleCombatAnimationClip != null) animatorOverrideController[IDLECOMBAT_ANIMATION] = idleCombatAnimationClip;

        if(runForwardAnimationClip != null) animatorOverrideController[RUN_FORWARD_ANIMATION] = runForwardAnimationClip;
        if (walkForwardAnimationClip != null) animatorOverrideController[WALK_FORWARD_ANIMATION] = walkForwardAnimationClip;
    }
    public void ChangeCurretCombatAnimations ( AnimatorOverrideController animatorOverrideController, EquipmentDataSO equipmentDataSO )
    {
        LoadBasicAttackAnimationsOnOverride(animatorOverrideController, equipmentDataSO.equipmentAnimationClips.basicAttackClips);
        LoadChargedAttackAnimationsOnOverride(animatorOverrideController, equipmentDataSO.equipmentAnimationClips.chargedAttackClips);
        LoadSpecialAttackAnimationsOnOverride(animatorOverrideController, equipmentDataSO.equipmentAnimationClips.specialAttackClips);
        LoadSkillAttackAnimationsOnOverride(animatorOverrideController, equipmentDataSO.equipmentAnimationClips.skillAttackClips);
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
    [OnValueChanged("SettingMaxAnimationsInList")]
    private void SettingMaxAnimationsInList ( )
    {
        maxNumberOfBasicAttacks = 4;
        maxNumberOfChargedAttacks = 4;
        maxNumberOfSpecialAttacks = 4;
        maxNumberOfSkillAttacks = 4;
    }
}
