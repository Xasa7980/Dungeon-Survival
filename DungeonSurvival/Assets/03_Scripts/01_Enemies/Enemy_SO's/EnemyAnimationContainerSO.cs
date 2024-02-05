using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationsContainer", menuName = "Dungeon Survival/Animations/EnemyAnimations")]
public class EnemyAnimationContainerSO : AnimationClipContainerSO
{
    public MonsterDataSO monsterDataSO;

    public AnimationClip eDeathBackClip;
    public AnimationClip eIdleClip;
    public AnimationClip eIdleCombatClip;
    public AnimationClip eRunForwardClip;
    public AnimationClip eWalkForwardClip;

    public const string E_DEATHBACK = "E_DeathBack";
    public const string E_IDLE = "E_Idle";
    public const string E_IDLECOMBAT = "E_IdleCombat";
    public const string E_RUN_FORWARD = "E_Run_Forward";
    public const string E_WALK_FORWARD = "E_Walk_Forward";


    public void ChangeCurrentBasicAnimations ( AnimatorOverrideController animatorOverrideController )
    {
        animatorOverrideController[E_DEATHBACK] = eDeathBackClip;
        animatorOverrideController[E_IDLE] = eIdleClip;
        animatorOverrideController[E_IDLECOMBAT] = eIdleCombatClip;
        animatorOverrideController[E_RUN_FORWARD] = eRunForwardClip;
        animatorOverrideController[E_WALK_FORWARD] = eWalkForwardClip;
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
}
