using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipContainerSO : ScriptableObject
{
    public event EventHandler OnChangeAnimation;

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

    [ShowIf("@attackCategory == AnimationAttackCategory.basic")] public float numberOfBasicAttacks = 4;
    [ShowIf("@attackCategory == AnimationAttackCategory.charged")] public float numberOfChargedAttacks = 4;
    [ShowIf("@attackCategory == AnimationAttackCategory.special")] public float numberOfSpecialAttacks = 4;
    [ShowIf("@attackCategory == AnimationAttackCategory.skill")] public float numberOfSkillAttacks = 4;

    [ShowIf("@getRandomAttackComboAnimation == true && attackCategory == AnimationAttackCategory.basic"), FoldoutGroup("Basic Attacks"), GUIColor(0.3f, 0.8f, 0.8f, 1f)] 
    public List<AnimationClip> basicAttackAnimationClips = new List<AnimationClip>();

    [ShowIf("@getRandomChargedAttackComboAnimation == true && attackCategory == AnimationAttackCategory.charged"), FoldoutGroup("Charged Attacks"), GUIColor(0.3f, 0.8f, 0.8f, 1f)] 
    public List<SpecialAttacksSO> chargedAttackAnimationClips = new List<SpecialAttacksSO>();

    [ShowIf("@getRandomSpecialAttackComboAnimation == true && attackCategory == AnimationAttackCategory.special"), FoldoutGroup("Special Attacks"), GUIColor(0.3f, 0.8f, 0.8f, 1f)] 
    public List<AnimationClip> specialAttackAnimationClips = new List<AnimationClip>();

    [ShowIf("@getRandomSkillAttackComboAnimation == true && attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks"), GUIColor(0.3f, 0.8f, 0.8f, 1f)] 
    public List<SpecialAttacksSO> skillAttackAnimationClips = new List<SpecialAttacksSO>();

    [ShowIf("@attackCategory == AnimationAttackCategory.basic"), FoldoutGroup("Basic Attacks")] 
    public AnimationClip attackBasicClip_01;
    [ShowIf("@attackCategory == AnimationAttackCategory.basic"), FoldoutGroup("Basic Attacks")] 
    public AnimationClip attackBasicClip_02;
    [ShowIf("@attackCategory == AnimationAttackCategory.basic"), FoldoutGroup("Basic Attacks")] 
    public AnimationClip attackBasicClip_03;
    [ShowIf("@attackCategory == AnimationAttackCategory.basic"), FoldoutGroup("Basic Attacks")] 
    public AnimationClip attackBasicClip_04;

    [ShowIf("@attackCategory == AnimationAttackCategory.charged"), FoldoutGroup("Charged Attacks")] 
    public AnimationClip chargedAttackClip_01;
    [ShowIf("@attackCategory == AnimationAttackCategory.charged"), FoldoutGroup("Charged Attacks")] 
    public AnimationClip chargedAttackClip_02;

    [PropertySpace(SpaceBefore = 10)]

    [ShowIf("@attackCategory == AnimationAttackCategory.charged"), FoldoutGroup("Charged Attacks")] 
    public AnimationClip chargingAttackClip_01;
    [ShowIf("@attackCategory == AnimationAttackCategory.charged"), FoldoutGroup("Charged Attacks")] 
    public AnimationClip chargingAttackClip_02;

    [ShowIf("@attackCategory == AnimationAttackCategory.special"), FoldoutGroup("Special Attacks")] 
    public AnimationClip specialAttackClip_01;
    [ShowIf("@attackCategory == AnimationAttackCategory.special"), FoldoutGroup("Special Attacks")] 
    public AnimationClip specialAttackClip_02;

    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_01;
    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_02;
    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip skillAttackClip_03;

    [PropertySpace(SpaceBefore = 10)]

    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip castingSkillAttackClip_01;
    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip castingSkillAttackClip_02;
    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip castingSkillAttackClip_03;

    private enum AnimationAttackCategory
    {
        basic,
        charged,
        special,
        skill
    }
    private AnimationAttackCategory attackCategory;
    #region EditorButtons
    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar_01")]
    [Button("Basic Attack")]
    private void SetBasicAttack() => attackCategory = AnimationAttackCategory.basic;

    [PropertyOrder(-2)]
    [HorizontalGroup("Toolbar_01")]
    [Button("Charged Attack")]
    private void SetChargedAttack() => attackCategory = AnimationAttackCategory.charged;    
    
    [PropertyOrder(-1)]
    [HorizontalGroup("Toolbar_01")]
    [Button("Basic Attack")]
    private void SetSpecialAttack() => attackCategory = AnimationAttackCategory.special;

    [PropertyOrder(-1)]
    [HorizontalGroup("Toolbar_01")]
    [Button("Charged Attack")]
    private void SetSkillAttack() => attackCategory = AnimationAttackCategory.skill;
    #endregion

    public EnemyAnimationContainerSO GetMonsterAnimationContainer ( AnimationClipContainerSO animationClipContainerSO )
    {
        return (EnemyAnimationContainerSO)animationClipContainerSO;
    }
    public PlayerAnimationContainerSO GetPlayerAnimationContainer ( AnimationClipContainerSO animationClipContainerSO )
    {
        return (PlayerAnimationContainerSO)animationClipContainerSO;
    }
    public int GetAnimationsList <T>( float totalClips, List<T> clipsList )
    {
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < totalClips)
        {
            int index = UnityEngine.Random.Range(0, clipsList.Count);
            if (!selectedIndices.Contains(index))
            {
                selectedIndices.Add(index);
                return index;
            }
        }
        return 0;
    }
    public void LoadBasicAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController )
    {
        if (getRandomAttackComboAnimation)
        {
            attackBasicClip_01 = basicAttackAnimationClips[GetAnimationsList(numberOfBasicAttacks, basicAttackAnimationClips)];
            attackBasicClip_02 = basicAttackAnimationClips[GetAnimationsList(numberOfBasicAttacks, basicAttackAnimationClips)];
            attackBasicClip_03 = basicAttackAnimationClips[GetAnimationsList(numberOfBasicAttacks, basicAttackAnimationClips)];
            attackBasicClip_04 = basicAttackAnimationClips[GetAnimationsList(numberOfBasicAttacks, basicAttackAnimationClips)];
        }
        
        animatorOverrideController[ATTACK_BASIC_1] = attackBasicClip_01;
        animatorOverrideController[ATTACK_BASIC_2] = attackBasicClip_02;
        animatorOverrideController[ATTACK_BASIC_3] = attackBasicClip_03;
        animatorOverrideController[ATTACK_BASIC_4] = attackBasicClip_04;

    }
    public void LoadChargedAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController )
    {
        if (getRandomChargedAttackComboAnimation)
        {
            if (numberOfChargedAttacks <= 0) return;
            chargedAttackClip_01 = chargedAttackAnimationClips[GetAnimationsList(numberOfChargedAttacks, chargedAttackAnimationClips)].release_Attack_Animation_Clip;
            chargedAttackClip_02 = chargedAttackAnimationClips[GetAnimationsList(numberOfChargedAttacks, chargedAttackAnimationClips)].release_Attack_Animation_Clip;

            chargingAttackClip_01 = chargedAttackAnimationClips[GetAnimationsList(numberOfChargedAttacks, chargedAttackAnimationClips)].loading_Attack_Animation_Clip;
            chargingAttackClip_02 = chargedAttackAnimationClips[GetAnimationsList(numberOfChargedAttacks, chargedAttackAnimationClips)].loading_Attack_Animation_Clip;
        }
        else
        {
            chargedAttackClip_01 = chargedAttackAnimationClips[0].release_Attack_Animation_Clip;
            chargedAttackClip_02 = chargedAttackAnimationClips[1].release_Attack_Animation_Clip;

            chargingAttackClip_01 = chargedAttackAnimationClips[0].loading_Attack_Animation_Clip;
            chargingAttackClip_02 = chargedAttackAnimationClips[1].loading_Attack_Animation_Clip;
        }
        animatorOverrideController[CHARGED_ATTACK_1] = chargedAttackClip_01;
        animatorOverrideController[CHARGED_ATTACK_2] = chargedAttackClip_02;

        animatorOverrideController[CHARGING_ATTACK_1] = chargingAttackClip_01;
        animatorOverrideController[CHARGING_ATTACK_2] = chargingAttackClip_02;
    }
    public void LoadSkillAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController )
    {
        if (getRandomChargedAttackComboAnimation)
        {
            if (numberOfChargedAttacks <= 0) return;
            skillAttackClip_01 = skillAttackAnimationClips[GetAnimationsList(numberOfSkillAttacks, skillAttackAnimationClips)].release_Attack_Animation_Clip;
            skillAttackClip_02 = skillAttackAnimationClips[GetAnimationsList(numberOfSkillAttacks, skillAttackAnimationClips)].release_Attack_Animation_Clip;

            castingSkillAttackClip_01 = skillAttackAnimationClips[GetAnimationsList(numberOfSkillAttacks, skillAttackAnimationClips)].loading_Attack_Animation_Clip;
            castingSkillAttackClip_02 = skillAttackAnimationClips[GetAnimationsList(numberOfSkillAttacks, skillAttackAnimationClips)].loading_Attack_Animation_Clip;
        }
        else
        {
            skillAttackClip_01 = skillAttackAnimationClips[0].release_Attack_Animation_Clip;
            skillAttackClip_02 = skillAttackAnimationClips[1].release_Attack_Animation_Clip;

            castingSkillAttackClip_01 = skillAttackAnimationClips[0].loading_Attack_Animation_Clip;
            castingSkillAttackClip_02 = skillAttackAnimationClips[1].loading_Attack_Animation_Clip;

        }
        animatorOverrideController[SKILL_ATTACK_1] = skillAttackClip_01;
        animatorOverrideController[SKILL_ATTACK_2] = skillAttackClip_02;

        animatorOverrideController[CASTING_SKILL_ATTACK_1] = castingSkillAttackClip_01;
        animatorOverrideController[CASTING_SKILL_ATTACK_2] = castingSkillAttackClip_02;

    }
}