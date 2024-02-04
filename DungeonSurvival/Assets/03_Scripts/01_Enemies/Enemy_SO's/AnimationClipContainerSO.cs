using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackCategory
{
    basic,
    charged,
    special,
    skill
}

public class AnimationClipContainerSO : ScriptableObject
{
    public const string ATTACK_BASIC_1 = "E_Attack_Basic_1";
    public const string ATTACK_BASIC_2 = "E_Attack_Basic_2";
    public const string ATTACK_BASIC_3 = "E_Attack_Basic_3";
    public const string ATTACK_BASIC_4 = "E_Attack_Basic_4";

    public const string CHARGED_ATTACK_1 = "ChargedAttack_01";
    public const string CHARGED_ATTACK_2 = "ChargedAttack_02";

    public const string CHARGING_ATTACK_1 = "ChargingAttack_01";
    public const string CHARGING_ATTACK_2 = "ChargingAttack_02";

    public const string SPECIAL_ATTACK_BASIC_1 = "SpecialAttack_01";
    public const string SPECIAL_ATTACK_BASIC_2 = "SpecialAttack_02";

    public const string SKILL_ATTACK_1 = "SpecialAttack_01";
    public const string SKILL_ATTACK_2 = "SpecialAttack_02";

    public const string CASTING_SKILL_ATTACK_1 = "SpecialAttack_01";
    public const string CASTING_SKILL_ATTACK_2 = "SpecialAttack_02";

    public bool getRandomAttackComboAnimation;
    public bool getRandomChargedAttackComboAnimation;
    public bool getRandomSpecialAttackComboAnimation;
    public bool getRandomSkillAttackComboAnimation;

    [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]

    [ShowIf("@attackCategory == AttackCategory.basic")] public float maxNumberOfBasicAttacks = 4;
    [ShowIf("@attackCategory == AttackCategory.charged")] public float maxNumberOfChargedAttacks = 4;
    [ShowIf("@attackCategory == AttackCategory.special")] public float maxNumberOfSpecialAttacks = 4;
    [ShowIf("@attackCategory == AttackCategory.skill")] public float maxNumberOfSkillAttacks = 4;
    private float numberOfBasicAttacks = 0;
    private float numberOfChargedAttacks = 0;
    private float numberOfSpecialAttacks = 0;
    private float numberOfSkillAttacks = 0;

    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("Basic Attacks")] public AttacksDataSO[] basicAttackDualSwordWeapon;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")] public AttacksDataSO[] chargedAttackDualSwordWeapon;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")] public AttacksDataSO[] specialAttackDualSwordWeapon;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")] public AttacksDataSO[] skillAttackDualSwordWeapon;

    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("Basic Attacks")] public AttacksDataSO[] basicAttackDualDaggerWeapon;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")] public AttacksDataSO[] chargedAttackDualDaggerWeapon;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")] public AttacksDataSO[] specialAttackDualDaggerWeapon;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")] public AttacksDataSO[] skillAttackDualDaggerWeapon;

    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("Basic Attacks")] public AttacksDataSO[] basicAttackNoWeapons;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")] public AttacksDataSO[] chargedAttackNoWeapons;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")] public AttacksDataSO[] specialAttackNoWeapons;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")] public AttacksDataSO[] skillAttackNoWeapons;

    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("Basic Attacks")] public AttacksDataSO[] basicAttackShield;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")] public AttacksDataSO[] chargeAttackShield;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")] public AttacksDataSO[] specialAttackShield;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")] public AttacksDataSO[] skillAttackShield;



    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("Basic Attacks")]
    public AnimationClip attackBasicClip_01;
    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("Basic Attacks")]
    public AnimationClip attackBasicClip_02;
    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("Basic Attacks")]
    public AnimationClip attackBasicClip_03;
    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("Basic Attacks")]
    public AnimationClip attackBasicClip_04;

    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip chargedAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip chargedAttackClip_02;

    [PropertySpace(SpaceBefore = 10)]

    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip chargingAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip chargingAttackClip_02;

    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip specialAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip specialAttackClip_02;

    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_02;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_03;

    [PropertySpace(SpaceBefore = 10)]

    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip castingSkillAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip castingSkillAttackClip_02;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip castingSkillAttackClip_03;

    private AttackCategory attackCategory;
    #region EditorButtons
    [PropertyOrder(3)]
    [HorizontalGroup("Toolbar_01")]
    [Button("Basic Attack")]
    private void SetBasicAttack ( ) => attackCategory = AttackCategory.basic;

    [PropertyOrder(3)]
    [HorizontalGroup("Toolbar_01")]
    [Button("Charged Attack")]
    private void SetChargedAttack ( ) => attackCategory = AttackCategory.charged;

    [PropertyOrder(3)]
    [HorizontalGroup("Toolbar_01")]
    [Button("Basic Attack")]
    private void SetSpecialAttack ( ) => attackCategory = AttackCategory.special;

    [PropertyOrder(3)]
    [HorizontalGroup("Toolbar_01")]
    [Button("Charged Attack")]
    private void SetSkillAttack ( ) => attackCategory = AttackCategory.skill;
    #endregion

    public EnemyAnimationContainerSO GetEnemyAnimationClipContainer ( AnimationClipContainerSO animationClipContainerSO )
    {
        return (EnemyAnimationContainerSO)animationClipContainerSO;
    }
    public PlayerAnimationContainerSO GetPlayerAnimationContainer ( AnimationClipContainerSO animationClipContainerSO )
    {
        return (PlayerAnimationContainerSO)animationClipContainerSO;
    }
    private int GetAnimationIndexList<T> ( float totalClips,T[] clipsList )
    {
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < totalClips)
        {
            int index = UnityEngine.Random.Range(0, clipsList.Length);
            if (!selectedIndices.Contains(index))
            {
                selectedIndices.Add(index);
                return index;
            }
        }
        return 0;
    }
    public void LoadBasicAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackBasicAnimationClips )
    {
        string[] attackClipKeys = { ATTACK_BASIC_1, ATTACK_BASIC_2, ATTACK_BASIC_3, ATTACK_BASIC_4 };
        AnimationClip[] attackClips = { attackBasicClip_01, attackBasicClip_02, attackBasicClip_03, attackBasicClip_04 };

        for (int i = 0; i < attackClips.Length; i++)
        {
            if(numberOfBasicAttacks > maxNumberOfBasicAttacks)
            {
                break;
            }

            attackClips[i] = getRandomAttackComboAnimation ? attackBasicAnimationClips[GetAnimationIndexList(numberOfBasicAttacks, attackBasicAnimationClips)].release_Attack_Animation_Clip :
                attackBasicAnimationClips[(int)numberOfBasicAttacks].release_Attack_Animation_Clip;
            numberOfBasicAttacks++;
            animatorOverrideController[attackClipKeys[i]] = attackClips[i];
        }
    }
    public void LoadChargedAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackChargedAnimationClips )
    {
        string[] animationChargingClipKeys = { CHARGING_ATTACK_1, CHARGING_ATTACK_2 };
        string[] animationChargedClipKeys = {CHARGED_ATTACK_1, CHARGED_ATTACK_2};

        AnimationClip[] chargingClips = { chargingAttackClip_01, chargingAttackClip_02 };
        AnimationClip[] chargedClips = { chargedAttackClip_01, chargedAttackClip_02 };

        for (int i = 0; i < chargedClips.Length; i++) //AÑADIR MAS EN CASO DE QUE LOS ENEMIGOS VAYAN A TENER MAS ANIMACIONES SEGUN RANGO
        {
            if (numberOfChargedAttacks > maxNumberOfChargedAttacks)
            {
                break;
            }

            chargingClips[i] = getRandomChargedAttackComboAnimation ? attackChargedAnimationClips[GetAnimationIndexList(numberOfChargedAttacks, attackChargedAnimationClips)].loading_Attack_Animation_Clip :
                attackChargedAnimationClips[(int)numberOfChargedAttacks].loading_Attack_Animation_Clip;

            chargedClips[i] = getRandomChargedAttackComboAnimation ? attackChargedAnimationClips[GetAnimationIndexList(numberOfChargedAttacks, attackChargedAnimationClips)].release_Attack_Animation_Clip :
                attackChargedAnimationClips[(int)numberOfChargedAttacks].release_Attack_Animation_Clip;

            numberOfChargedAttacks++;

            animatorOverrideController[animationChargedClipKeys[i]] = chargedClips[i];
            animatorOverrideController[animationChargingClipKeys[i]] = chargedClips[i];
        }
    }
    public void LoadSpecialAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController,  AttacksDataSO[] attackSpecialAnimationClips )
    {
        string[] animationSpecialClipKeys = { CASTING_SKILL_ATTACK_1, CASTING_SKILL_ATTACK_2 };
        AnimationClip[] specialClips = { chargedAttackClip_01, chargedAttackClip_02 };
        string[] animationSpecialLoadingClipKeys = { CASTING_SKILL_ATTACK_1, CASTING_SKILL_ATTACK_2 };
        AnimationClip[] specialLoadingClips = { chargedAttackClip_01, chargedAttackClip_02 };

        for (int i = 0; i < specialClips.Length; i++)
        {
            if (numberOfSpecialAttacks > maxNumberOfSpecialAttacks) break;

            if (attackSpecialAnimationClips[i].specialAttackNeedsLoading)
            {
                specialLoadingClips[i] = getRandomSkillAttackComboAnimation ? attackSpecialAnimationClips[GetAnimationIndexList(numberOfSpecialAttacks, attackSpecialAnimationClips)].loading_Attack_Animation_Clip :
                attackSpecialAnimationClips[(int)numberOfSpecialAttacks].loading_Attack_Animation_Clip;

                specialClips[i] = getRandomSkillAttackComboAnimation ? attackSpecialAnimationClips[GetAnimationIndexList(numberOfSpecialAttacks, attackSpecialAnimationClips)].release_Attack_Animation_Clip :
                attackSpecialAnimationClips[(int)numberOfSpecialAttacks].release_Attack_Animation_Clip;
            }
            else
            {
                specialClips[i] = getRandomSpecialAttackComboAnimation ? attackSpecialAnimationClips[GetAnimationIndexList(numberOfSpecialAttacks, attackSpecialAnimationClips)].release_Attack_Animation_Clip :
                    attackSpecialAnimationClips[(int)numberOfSpecialAttacks].release_Attack_Animation_Clip;
            }
        
            numberOfSpecialAttacks++;

            animatorOverrideController[animationSpecialClipKeys[i]] = specialClips[i];
        }
    }

    public void LoadSkillAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackSkillAnimationClips )
    {
        string[] animationCastingClipKeys = { CASTING_SKILL_ATTACK_1, CASTING_SKILL_ATTACK_2 };
        string[] animationSkillClipKeys = { SKILL_ATTACK_1, SKILL_ATTACK_2 };

        AnimationClip[] castingClips = { chargingAttackClip_01, chargingAttackClip_02 };
        AnimationClip[] skillClips = { chargedAttackClip_01, chargedAttackClip_02 };

        for (int i = 0; i < skillClips.Length; i++)
        {
            if (numberOfSkillAttacks > maxNumberOfSkillAttacks) break;

            castingClips[i] = getRandomSkillAttackComboAnimation ? attackSkillAnimationClips[GetAnimationIndexList(numberOfSkillAttacks, attackSkillAnimationClips)].loading_Attack_Animation_Clip :
                attackSkillAnimationClips[(int)numberOfSkillAttacks].loading_Attack_Animation_Clip;

            skillClips[i] = getRandomSkillAttackComboAnimation ? attackSkillAnimationClips[GetAnimationIndexList(numberOfSkillAttacks, attackSkillAnimationClips)].release_Attack_Animation_Clip :
                attackSkillAnimationClips[(int)numberOfSkillAttacks].release_Attack_Animation_Clip;

            numberOfSkillAttacks++;

            animatorOverrideController[animationCastingClipKeys[i]] = castingClips[i];
            animatorOverrideController[animationSkillClipKeys[i]] = skillClips[i];
        }
    }
    public void LimitMonsterAnimationsByMonsterRank ( MonsterDataSO monsterDataSO )
    {
        if(monsterDataSO != null)
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