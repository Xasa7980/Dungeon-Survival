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
    public const string ATTACK_BASIC_1 = "BasicAttack_Animation_01";
    public const string ATTACK_BASIC_2 = "BasicAttack_Animation_02";
    public const string ATTACK_BASIC_3 = "BasicAttack_Animation_03";
    public const string ATTACK_BASIC_4 = "BasicAttack_Animation_04";

    public const string CHARGED_ATTACK_1 = "ChargedAttack_Animation_01";
    public const string CHARGED_ATTACK_2 = "ChargedAttack_Animation_02";
    public const string CHARGED_ATTACK_3 = "ChargedAttack_Animation_03";
    public const string CHARGED_ATTACK_4 = "ChargedAttack_Animation_04";

    public const string LOADING_CHARGING_ATTACK_1 = "LoadingChargedAttack_Animation_01";
    public const string LOADING_CHARGING_ATTACK_2 = "LoadingChargedAttack_Animation_02";
    public const string LOADING_CHARGING_ATTACK_3 = "LoadingChargedAttack_Animation_03";
    public const string LOADING_CHARGING_ATTACK_4 = "LoadingChargedAttack_Animation_04";

    public const string SPECIAL_ATTACK_BASIC_1 = "SpecialAttack_Animation_01";
    public const string SPECIAL_ATTACK_BASIC_2 = "SpecialAttack_Animation_02";
    public const string SPECIAL_ATTACK_BASIC_3 = "SpecialAttack_Animation_03";
    public const string SPECIAL_ATTACK_BASIC_4 = "SpecialAttack_Animation_04";

    public const string LOADING_SPECIAL_ATTACK_BASIC_1 = "LoadingSpecialAttack_Animation_01";
    public const string LOADING_SPECIAL_ATTACK_BASIC_2 = "LoadingSpecialAttack_Animation_02";
    public const string LOADING_SPECIAL_ATTACK_BASIC_3 = "LoadingSpecialAttack_Animation_03";
    public const string LOADING_SPECIAL_ATTACK_BASIC_4 = "LoadingSpecialAttack_Animation_04";

    public const string SKILL_ATTACK_1 = "SkillAttack_Animation_01";
    public const string SKILL_ATTACK_2 = "SkillAttack_Animation_02";
    public const string SKILL_ATTACK_3 = "SkillAttack_Animation_03";
    public const string SKILL_ATTACK_4 = "SkillAttack_Animation_04";

    public const string LOADING_SKILL_ATTACK_1 = "LoadingSkillAttack_Animation_01";
    public const string LOADING_SKILL_ATTACK_2 = "LoadingSkillAttack_Animation_02";
    public const string LOADING_SKILL_ATTACK_3 = "LoadingSkillAttack_Animation_03";
    public const string LOADING_SKILL_ATTACK_4 = "LoadingSkillAttack_Animation_04";

    public bool getRandomAttackComboAnimation;
    public bool getRandomChargedAttackComboAnimation;
    public bool getRandomSpecialAttackComboAnimation;
    public bool getRandomSkillAttackComboAnimation;

    [PropertySpace(SpaceBefore = 10, SpaceAfter = 10)]

    [ShowIf("@attackCategory == AttackCategory.basic")] public float maxNumberOfBasicAttacks = 4;
    [ShowIf("@attackCategory == AttackCategory.charged")] public float maxNumberOfChargedAttacks = 4;
    [ShowIf("@attackCategory == AttackCategory.special")] public float maxNumberOfSpecialAttacks = 4;
    [ShowIf("@attackCategory == AttackCategory.skill")] public float maxNumberOfSkillAttacks = 4;

    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] basicAttackDualSwordWeapon;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] chargedAttackDualSwordWeapon;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] specialAttackDualSwordWeapon;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] skillAttackDualSwordWeapon;

    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] basicAttackDualDaggerWeapon;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] chargedAttackDualDaggerWeapon;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] specialAttackDualDaggerWeapon;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] skillAttackDualDaggerWeapon;

    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] basicAttackNoWeapons;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] chargedAttackNoWeapons;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] specialAttackNoWeapons;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] skillAttackNoWeapons;

    [ShowIf("@attackCategory == AttackCategory.basic"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] basicAttackShield;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] chargeAttackShield;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] specialAttackShield;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("ExtraAnimationsSetUp")] public AttacksDataSO[] skillAttackShield;



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
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip chargedAttackClip_03;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip chargedAttackClip_04;

    [PropertySpace(SpaceBefore = 10)]

    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip loadingChargingAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip loadingChargingAttackClip_02;    
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip loadingChargingAttackClip_03;
    [ShowIf("@attackCategory == AttackCategory.charged"), FoldoutGroup("Charged Attacks")]
    public AnimationClip loadingChargingAttackClip_04;

    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip specialAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip specialAttackClip_02;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip specialAttackClip_03;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip specialAttackClip_04;

    [PropertySpace(SpaceBefore = 10)]
    
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip loadingSpecialAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip loadingSpecialAttackClip_02;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip loadingSpecialAttackClip_03;
    [ShowIf("@attackCategory == AttackCategory.special"), FoldoutGroup("Special Attacks")]
    public AnimationClip loadingSpecialAttackClip_04;

    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_02;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_03;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_04;

    [PropertySpace(SpaceBefore = 10)]

    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip loadingSkillAttackClip_01;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip loadingSkillAttackClip_02;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip loadingSkillAttackClip_03;
    [ShowIf("@attackCategory == AttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip loadingSkillAttackClip_04;

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
    [Button("Special Attack")]
    private void SetSpecialAttack ( ) => attackCategory = AttackCategory.special;

    [PropertyOrder(3)]
    [HorizontalGroup("Toolbar_01")]
    [Button("Skill Attack")]
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
    private List<int> GetListRandomAnimationIndex ( float totalClips, AttacksDataSO[] clipsListLength )
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
    public List<AttacksDataSO> basicAttacksSO_List = new List<AttacksDataSO>();
    public void LoadBasicAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackBasicAnimationClips )
    {
        if(attackBasicAnimationClips.Length > 0)
        {
            basicAttacksSO_List.Clear();

            string[] attackClipKeys = { ATTACK_BASIC_1, ATTACK_BASIC_2, ATTACK_BASIC_3, ATTACK_BASIC_4 };
            AnimationClip[] attackClips = { attackBasicClip_01, attackBasicClip_02, attackBasicClip_03, attackBasicClip_04 };

            List<int> randomIndices = GetListRandomAnimationIndex(maxNumberOfBasicAttacks, attackBasicAnimationClips);
            for (int i = 0; i < maxNumberOfBasicAttacks; i++)
            {
                attackClips[i] = attackBasicAnimationClips[randomIndices[i]].release_Attack_Animation_Clip;
                basicAttacksSO_List.Add(attackBasicAnimationClips[randomIndices[i]]);
                animatorOverrideController[attackClipKeys[i]] = attackClips[i];
            }
        }
        else
        {
            Debug.Log("No basic attack clips available to assign.");
            return;
        }
    }
    public List<AttacksDataSO> chargedAttacksSO_List = new List<AttacksDataSO>();
    public void LoadChargedAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackChargedAnimationClips )
    {
        if (attackChargedAnimationClips.Length > 0)
        {
            chargedAttacksSO_List.Clear();

            string[] animationLoadingChargedClipKeys = { LOADING_CHARGING_ATTACK_1, LOADING_CHARGING_ATTACK_2, LOADING_CHARGING_ATTACK_3, LOADING_CHARGING_ATTACK_4 };
            string[] animationChargedClipKeys = { CHARGED_ATTACK_1, CHARGED_ATTACK_2, LOADING_CHARGING_ATTACK_3, LOADING_CHARGING_ATTACK_4 };

            AnimationClip[] loadingClips = { loadingChargingAttackClip_01, loadingChargingAttackClip_02, loadingChargingAttackClip_03, loadingChargingAttackClip_04 };
            AnimationClip[] chargedClips = { chargedAttackClip_01, chargedAttackClip_02, chargedAttackClip_03, chargedAttackClip_04 };

            int availableClips = attackChargedAnimationClips.Length;

            List<int> randomIndices = GetListRandomAnimationIndex(Math.Min(maxNumberOfChargedAttacks, availableClips), attackChargedAnimationClips);

            for (int i = 0; i < randomIndices.Count && i < availableClips; i++)
            {
                chargedClips[i] = attackChargedAnimationClips[randomIndices[i]].release_Attack_Animation_Clip;
                chargedAttacksSO_List.Add(attackChargedAnimationClips[randomIndices[i]]);
                
                if (attackChargedAnimationClips[randomIndices[i]].specialAttackNeedsLoading)
                {
                    loadingClips[i] = attackChargedAnimationClips[randomIndices[i]].loading_Attack_Animation_Clip;
                    animatorOverrideController[animationLoadingChargedClipKeys[i]] = loadingClips[i];
                }
                animatorOverrideController[animationChargedClipKeys[i]] = chargedClips[i];
            }
        }

        else
        {
            Debug.Log("No charged attack clips available to assign.");
            return;
        }
    }
    public List<AttacksDataSO> specialAttacksSO_List = new List<AttacksDataSO>();
    public void LoadSpecialAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackSpecialAnimationClips )
    {
        if (attackSpecialAnimationClips.Length > 0)
        {
            specialAttacksSO_List.Clear();

            string[] animationSpecialClipKeys = { SPECIAL_ATTACK_BASIC_1, SPECIAL_ATTACK_BASIC_2, SPECIAL_ATTACK_BASIC_3, SPECIAL_ATTACK_BASIC_4 };
            string[] animationSpecialLoadingClipKeys = { LOADING_SPECIAL_ATTACK_BASIC_1, LOADING_SPECIAL_ATTACK_BASIC_2, LOADING_SPECIAL_ATTACK_BASIC_3, LOADING_SPECIAL_ATTACK_BASIC_4 };

            AnimationClip[] specialLoadingClips = { loadingSpecialAttackClip_01, loadingSpecialAttackClip_02, loadingSpecialAttackClip_03, loadingSpecialAttackClip_04 };
            AnimationClip[] specialClips = { specialAttackClip_01, specialAttackClip_02, specialAttackClip_03, specialAttackClip_04 };

            List<int> randomIndices = GetListRandomAnimationIndex(Math.Min(maxNumberOfSpecialAttacks, attackSpecialAnimationClips.Length), attackSpecialAnimationClips);

            for (int i = 0; i < randomIndices.Count; i++)
            {
                specialClips[i] = attackSpecialAnimationClips[randomIndices[i]].release_Attack_Animation_Clip;
                if (attackSpecialAnimationClips[randomIndices[i]].specialAttackNeedsLoading)
                {
                    specialLoadingClips[i] = attackSpecialAnimationClips[randomIndices[i]].loading_Attack_Animation_Clip;
                    animatorOverrideController[animationSpecialLoadingClipKeys[i]] = specialLoadingClips[i];
                }
                specialAttacksSO_List.Add(attackSpecialAnimationClips[randomIndices[i]]);
                animatorOverrideController[animationSpecialClipKeys[i]] = specialClips[i];
            }
        }
        else
        {
            Debug.Log("No special attack clips available to assign.");
        }
    }

    public List<AttacksDataSO> skillAttacksSO_List = new List<AttacksDataSO>();
    public void LoadSkillAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController, AttacksDataSO[] attackSkillAnimationClips )
    {
        if (attackSkillAnimationClips.Length > 0)
        {
            skillAttacksSO_List.Clear();
            string[] animationCastingClipKeys = { LOADING_SKILL_ATTACK_1, LOADING_SKILL_ATTACK_2, LOADING_SKILL_ATTACK_3, LOADING_SKILL_ATTACK_4 };
            string[] animationSkillClipKeys = { SKILL_ATTACK_1, SKILL_ATTACK_2, SKILL_ATTACK_3, SKILL_ATTACK_4 };

            AnimationClip[] loadingClips = { loadingSkillAttackClip_01, loadingSkillAttackClip_02, loadingSkillAttackClip_03, loadingSkillAttackClip_04 };
            AnimationClip[] skillClips = { skillAttackClip_01, skillAttackClip_02, skillAttackClip_03, skillAttackClip_04 };

            List<int> randomIndices = GetListRandomAnimationIndex(Math.Min(maxNumberOfSkillAttacks, attackSkillAnimationClips.Length), attackSkillAnimationClips);

            for (int i = 0; i < randomIndices.Count; i++)
            {
                skillClips[i] = attackSkillAnimationClips[randomIndices[i]].release_Attack_Animation_Clip;
                if (attackSkillAnimationClips[randomIndices[i]].specialAttackNeedsLoading)
                {
                    loadingClips[i] = attackSkillAnimationClips[randomIndices[i]].loading_Attack_Animation_Clip;
                    animatorOverrideController[animationCastingClipKeys[i]] = loadingClips[i];
                }
                skillAttacksSO_List.Add(attackSkillAnimationClips[randomIndices[i]]);
                animatorOverrideController[animationSkillClipKeys[i]] = skillClips[i];
            }
        }
        else
        {
            Debug.Log("No skill attack clips available to assign.");
        }
    }
}