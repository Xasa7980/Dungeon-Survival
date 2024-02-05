using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
    public const string ATTACK_BASIC_1 = "BasicAttack_Default_01";
    public const string ATTACK_BASIC_2 = "BasicAttack_Default_02";
    public const string ATTACK_BASIC_3 = "BasicAttack_Default_03";
    public const string ATTACK_BASIC_4 = "BasicAttack_Default_04";

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
    private List<int> GetListRandomAnimationIndex<T> ( float totalClips,T[] clipsListLength )
    {
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < totalClips)
        {
            int index = UnityEngine.Random.Range(0, clipsListLength.Length);
            if (!selectedIndices.Contains(index))
            {
                selectedIndices.Add(index);
            }
        }
        return selectedIndices;
        
    }
    public void LoadBasicAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackBasicAnimationClips )
    {
        string[] attackClipKeys = { ATTACK_BASIC_1, ATTACK_BASIC_2, ATTACK_BASIC_3, ATTACK_BASIC_4 };
        AnimationClip[] attackClips = { attackBasicClip_01, attackBasicClip_02, attackBasicClip_03, attackBasicClip_04 };

        List<int> randomIndices = GetListRandomAnimationIndex<AttacksDataSO>(maxNumberOfBasicAttacks, attackBasicAnimationClips);
        for (int i = 0; i < randomIndices.Count && i < attackClipKeys.Length; i++)
        {
            attackClips[i] = attackBasicAnimationClips[randomIndices[i]].release_Attack_Animation_Clip;
            Debug.Log(attackClips[i]);
            animatorOverrideController[attackClipKeys[i]] = attackClips[i];
        }
    }
    public void LoadChargedAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackChargedAnimationClips )
    {
        string[] animationChargingClipKeys = { CHARGING_ATTACK_1, CHARGING_ATTACK_2 };
        string[] animationChargedClipKeys = { CHARGED_ATTACK_1, CHARGED_ATTACK_2 };

        AnimationClip[] chargingClips = { chargingAttackClip_01, chargingAttackClip_02 };
        AnimationClip[] chargedClips = { chargedAttackClip_01, chargedAttackClip_02 };

        List<int> randomIndices = GetListRandomAnimationIndex<AttacksDataSO>(maxNumberOfChargedAttacks, attackChargedAnimationClips);
        for (int i = 0; i < randomIndices.Count && i < animationChargedClipKeys.Length; i++)
        {
            chargedClips[i] = attackChargedAnimationClips[randomIndices[i]].release_Attack_Animation_Clip;
            chargingClips[i] = attackChargedAnimationClips[randomIndices[i]].loading_Attack_Animation_Clip;
            Debug.Log(chargedClips[i]);
            animatorOverrideController[animationChargedClipKeys[i]] = chargedClips[i];
        }
    }
    public void LoadSpecialAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackSpecialAnimationClips )
    {
        string[] animationSpecialClipKeys = { CASTING_SKILL_ATTACK_1, CASTING_SKILL_ATTACK_2 };
        AnimationClip[] specialClips = { chargedAttackClip_01, chargedAttackClip_02 };
        string[] animationSpecialLoadingClipKeys = { CASTING_SKILL_ATTACK_1, CASTING_SKILL_ATTACK_2 };
        AnimationClip[] specialLoadingClips = { chargedAttackClip_01, chargedAttackClip_02 };

        List<int> randomIndices = GetListRandomAnimationIndex<AttacksDataSO>(maxNumberOfSpecialAttacks, attackSpecialAnimationClips);
        for (int i = 0; i < randomIndices.Count && i < attackSpecialAnimationClips.Length; i++)
        {
            specialClips[i] = attackSpecialAnimationClips[randomIndices[i]].release_Attack_Animation_Clip;
            if (attackSpecialAnimationClips[randomIndices[i]].loading_Attack_Animation_Clip != null )
            {
                specialLoadingClips[i] = attackSpecialAnimationClips[randomIndices[i]].loading_Attack_Animation_Clip;
            }
            animatorOverrideController[animationSpecialClipKeys[i]] = specialClips[i];
            animatorOverrideController[animationSpecialLoadingClipKeys[i]] = specialLoadingClips[i];
        }
    }

    public void LoadSkillAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackSkillAnimationClips )
    {
        string[] animationCastingClipKeys = { CASTING_SKILL_ATTACK_1, CASTING_SKILL_ATTACK_2 };
        string[] animationSkillClipKeys = { SKILL_ATTACK_1, SKILL_ATTACK_2 };

        AnimationClip[] castingClips = { chargingAttackClip_01, chargingAttackClip_02 };
        AnimationClip[] skillClips = { chargedAttackClip_01, chargedAttackClip_02 };

        List<int> randomIndices = GetListRandomAnimationIndex<AttacksDataSO>(maxNumberOfSpecialAttacks, attackSkillAnimationClips);
        for (int i = 0; i < randomIndices.Count && i < attackSkillAnimationClips.Length; i++)
        {
            skillClips[i] = attackSkillAnimationClips[randomIndices[i]].release_Attack_Animation_Clip;
            if (attackSkillAnimationClips[randomIndices[i]].loading_Attack_Animation_Clip != null)
            {
                castingClips[i] = attackSkillAnimationClips[randomIndices[i]].loading_Attack_Animation_Clip;
            }
            animatorOverrideController[animationSkillClipKeys[i]] = skillClips[i];
            animatorOverrideController[animationCastingClipKeys[i]] = castingClips[i];
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