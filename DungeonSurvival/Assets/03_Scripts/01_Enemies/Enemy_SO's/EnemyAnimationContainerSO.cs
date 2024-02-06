using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationsContainer", menuName = "Dungeon Survival/Animations/EnemyAnimations")]
public class EnemyAnimationContainerSO : AnimationClipContainerSO
{
    public MonsterDataSO monsterDataSO;

    public AnimationClip deathAnimationClip;
    public AnimationClip idleAnimationClip;
    public AnimationClip idleCombatAnimationClip;
    public AnimationClip runForwardAnimationClip;
    public AnimationClip walkForwardAnimationClip;

    public const string DEATH_ANIMATION = "Death_Animation";
    public const string IDLE_ANIMATION = "Idle_Animation";
    public const string IDLECOMBAT_ANIMATION = "IdleCombat_Animation";
    public const string RUN_FORWARD_ANIMATION = "Run_Forward_Animation";
    public const string WALK_FORWARD_ANIMATION = "Walk_Forward_Animation";

    private void OnValidate ( )
    {
        LimitMonsterAnimationsByMonsterRank(monsterDataSO);
    }
    public void ChangeCurrentBasicAnimations ( AnimatorOverrideController animatorOverrideController )
    {
        LimitMonsterAnimationsByMonsterRank(monsterDataSO);
        animatorOverrideController[DEATH_ANIMATION] = deathAnimationClip;
        animatorOverrideController[IDLE_ANIMATION] = idleAnimationClip;
        animatorOverrideController[IDLECOMBAT_ANIMATION] = idleCombatAnimationClip;
        animatorOverrideController[RUN_FORWARD_ANIMATION] = runForwardAnimationClip;
        animatorOverrideController[WALK_FORWARD_ANIMATION] = walkForwardAnimationClip;
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

    [OnValueChanged("LimitMonsterAnimationsByMonsterRank")]
    public void LimitMonsterAnimationsByMonsterRank ( MonsterDataSO monsterDataSO )
    {
        if (monsterDataSO != null)
        {
            switch (monsterDataSO.monsterRank)
            {
                case MonsterRank.Minion:

                    maxNumberOfBasicAttacks = 2;
                    maxNumberOfChargedAttacks = 1;
                    maxNumberOfSpecialAttacks = 1;
                    maxNumberOfSkillAttacks = 0;
                    break;

                case MonsterRank.Soldier:

                    maxNumberOfBasicAttacks = 3;
                    maxNumberOfChargedAttacks = 1;
                    maxNumberOfSpecialAttacks = 1;
                    maxNumberOfSkillAttacks = 0;
                    break;

                case MonsterRank.Guardian:

                    maxNumberOfBasicAttacks = 4;
                    maxNumberOfChargedAttacks = 1;
                    maxNumberOfSpecialAttacks = 1;
                    maxNumberOfSkillAttacks = 0;
                    break;

                case MonsterRank.General:

                    maxNumberOfBasicAttacks = 3;
                    maxNumberOfChargedAttacks = 2;
                    maxNumberOfSpecialAttacks = 1;
                    maxNumberOfSkillAttacks = 1;
                    break;

                case MonsterRank.Boss:

                    maxNumberOfBasicAttacks = 3;
                    maxNumberOfChargedAttacks = 2;
                    maxNumberOfSpecialAttacks = 2;
                    maxNumberOfSkillAttacks = 2;
                    break;

                case MonsterRank.EliteBoss:

                    maxNumberOfBasicAttacks = 3;
                    maxNumberOfChargedAttacks = 2;
                    maxNumberOfSpecialAttacks = 3;
                    maxNumberOfSkillAttacks = 2;
                    break;

                default:
                    break;
            }
        }
    }

}
