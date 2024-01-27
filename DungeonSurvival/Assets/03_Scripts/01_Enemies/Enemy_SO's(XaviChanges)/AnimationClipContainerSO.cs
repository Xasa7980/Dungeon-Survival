using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationsContainer", menuName = "Dungeon Survival/Enemy/Animations")]
public class AnimationClipContainerSO : ScriptableObject
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
    public const string E_ATTACK_BASIC_1 = "E_Attack_Basic_1";
    public const string E_ATTACK_BASIC_2 = "E_Attack_Basic_2";
    public const string E_ATTACK_BASIC_3 = "E_Attack_Basic_3";
    public const string E_ATTACK_BASIC_4 = "E_Attack_Basic_4";

    public const string E_CHARGED_ATTACK_1 = "ChargedAttack_01";
    public const string E_CHARGED_ATTACK_2 = "ChargedAttack_02";
    public const string E_CHARGING_ATTACK_1 = "ChargingAttack_01";
    public const string E_CHARGING_ATTACK_2 = "ChargingAttack_02";

    public const string E_SPECIAL_ATTACK_BASIC_1 = "SpecialAttack_01";
    public const string E_SPECIAL_ATTACK_BASIC_2 = "SpecialAttack_02";

    public const string E_SKILL_ATTACK_BASIC_1 = "SpecialAttack_01";
    public const string E_SKILL_ATTACK_BASIC_2 = "SpecialAttack_02";
    public const string E_CASTING_SKILL_ATTACK_BASIC_1 = "SpecialAttack_01";
    public const string E_CASTING_SKILL_ATTACK_BASIC_2 = "SpecialAttack_02";

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
    public AnimationClip eAttackBasicClip_01;

    [ShowIf("@attackCategory == AnimationAttackCategory.basic"), FoldoutGroup("Basic Attacks")] 
    public AnimationClip eAttackBasicClip_02;

    [ShowIf("@attackCategory == AnimationAttackCategory.basic"), FoldoutGroup("Basic Attacks")] 
    public AnimationClip eAttackBasicClip_03;

    [ShowIf("@attackCategory == AnimationAttackCategory.basic"), FoldoutGroup("Basic Attacks")] 
    public AnimationClip eAttackBasicClip_04;

    [ShowIf("@attackCategory == AnimationAttackCategory.charged"), FoldoutGroup("Charged Attacks")] 
    public AnimationClip eChargedAttackClip_01;
    [ShowIf("@attackCategory == AnimationAttackCategory.charged"), FoldoutGroup("Charged Attacks")] 
    public AnimationClip eChargedAttackClip_02;

    [PropertySpace(SpaceBefore = 10)]

    [ShowIf("@attackCategory == AnimationAttackCategory.charged"), FoldoutGroup("Charged Attacks")] 
    public AnimationClip eChargingAttackClip_01;
    [ShowIf("@attackCategory == AnimationAttackCategory.charged"), FoldoutGroup("Charged Attacks")] 
    public AnimationClip eChargingAttackClip_02;

    [ShowIf("@attackCategory == AnimationAttackCategory.special"), FoldoutGroup("Special Attacks")] 
    public AnimationClip eSpecialAttackClip_01;

    [ShowIf("@attackCategory == AnimationAttackCategory.special"), FoldoutGroup("Special Attacks")] 
    public AnimationClip eSpecialAttackClip_02;

    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip eSkillAttackClip_01;
    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip eSkillAttackClip_02;

    [PropertySpace(SpaceBefore = 10)]

    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip eCastingSkillAttackClip_01;

    [ShowIf("@attackCategory == AnimationAttackCategory.skill"), FoldoutGroup("Skill Attacks")]
    public AnimationClip eCastingSkillAttackClip_02;

    private enum AnimationAttackCategory
    {
        basic,
        charged,
        special,
        skill
    }
    private AnimationAttackCategory attackCategory;

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
    public void ChangeCurrentAnimations ( AnimatorOverrideController animatorOverrideController )
    {
        LoadBasicAttackAnimationsOnOverride(animatorOverrideController);
        LoadChargedAttackAnimationsOnOverride (animatorOverrideController);
        LoadSkillAttackAnimationsOnOverride(animatorOverrideController );

        animatorOverrideController[E_DEATHBACK] = eDeathBackClip;
        animatorOverrideController[E_IDLE] = eIdleClip;
        animatorOverrideController[E_IDLECOMBAT] = eIdleCombatClip;
        animatorOverrideController[E_RUN_FORWARD] = eRunForwardClip;
        animatorOverrideController[E_WALK_FORWARD] = eWalkForwardClip;

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
    private void LoadBasicAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController )
    {
        if (getRandomAttackComboAnimation)
        {
            eAttackBasicClip_01 = basicAttackAnimationClips[GetAnimationsList(numberOfBasicAttacks, basicAttackAnimationClips)];
            eAttackBasicClip_02 = basicAttackAnimationClips[GetAnimationsList(numberOfBasicAttacks, basicAttackAnimationClips)];
            eAttackBasicClip_03 = basicAttackAnimationClips[GetAnimationsList(numberOfBasicAttacks, basicAttackAnimationClips)];
            eAttackBasicClip_04 = basicAttackAnimationClips[GetAnimationsList(numberOfBasicAttacks, basicAttackAnimationClips)];

            animatorOverrideController[E_ATTACK_BASIC_1] = eAttackBasicClip_01;
            animatorOverrideController[E_ATTACK_BASIC_2] = eAttackBasicClip_02;
            animatorOverrideController[E_ATTACK_BASIC_3] = eAttackBasicClip_03;
            animatorOverrideController[E_ATTACK_BASIC_4] = eAttackBasicClip_04;
        }
        
        animatorOverrideController[E_ATTACK_BASIC_1] = eAttackBasicClip_01;
        animatorOverrideController[E_ATTACK_BASIC_2] = eAttackBasicClip_02;
        animatorOverrideController[E_ATTACK_BASIC_3] = eAttackBasicClip_03;
        animatorOverrideController[E_ATTACK_BASIC_4] = eAttackBasicClip_04;

    }
    private void LoadChargedAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController )
    {
        if (getRandomChargedAttackComboAnimation)
        {
            if (numberOfChargedAttacks <= 0) return;
            eChargedAttackClip_01 = chargedAttackAnimationClips[GetAnimationsList(numberOfChargedAttacks, chargedAttackAnimationClips)].release_Attack_Animation_Clip;
            eChargedAttackClip_02 = chargedAttackAnimationClips[GetAnimationsList(numberOfChargedAttacks, chargedAttackAnimationClips)].release_Attack_Animation_Clip;

            eChargingAttackClip_01 = chargedAttackAnimationClips[GetAnimationsList(numberOfChargedAttacks, chargedAttackAnimationClips)].loading_Attack_Animation_Clip;
            eChargingAttackClip_02 = chargedAttackAnimationClips[GetAnimationsList(numberOfChargedAttacks, chargedAttackAnimationClips)].loading_Attack_Animation_Clip;
        }
        else
        {
            eChargedAttackClip_01 = chargedAttackAnimationClips[0].release_Attack_Animation_Clip;
            eChargedAttackClip_02 = chargedAttackAnimationClips[1].release_Attack_Animation_Clip;

            eChargingAttackClip_01 = chargedAttackAnimationClips[0].loading_Attack_Animation_Clip;
            eChargingAttackClip_02 = chargedAttackAnimationClips[1].loading_Attack_Animation_Clip;
        }
        animatorOverrideController[E_CHARGED_ATTACK_1] = eChargedAttackClip_01;
        animatorOverrideController[E_CHARGED_ATTACK_2] = eChargedAttackClip_02;

        animatorOverrideController[E_CHARGING_ATTACK_1] = eChargingAttackClip_01;
        animatorOverrideController[E_CHARGING_ATTACK_2] = eChargingAttackClip_02;
    }
    private void LoadSkillAttackAnimationsOnOverride ( AnimatorOverrideController animatorOverrideController )
    {
        if (getRandomChargedAttackComboAnimation)
        {
            if (numberOfChargedAttacks <= 0) return;
            eSkillAttackClip_01 = skillAttackAnimationClips[GetAnimationsList(numberOfSkillAttacks, skillAttackAnimationClips)].release_Attack_Animation_Clip;
            eSkillAttackClip_02 = skillAttackAnimationClips[GetAnimationsList(numberOfSkillAttacks, skillAttackAnimationClips)].release_Attack_Animation_Clip;

            eCastingSkillAttackClip_01 = skillAttackAnimationClips[GetAnimationsList(numberOfSkillAttacks, skillAttackAnimationClips)].loading_Attack_Animation_Clip;
            eCastingSkillAttackClip_02 = skillAttackAnimationClips[GetAnimationsList(numberOfSkillAttacks, skillAttackAnimationClips)].loading_Attack_Animation_Clip;
        }
        else
        {
            eSkillAttackClip_01 = skillAttackAnimationClips[0].release_Attack_Animation_Clip;
            eSkillAttackClip_02 = skillAttackAnimationClips[1].release_Attack_Animation_Clip;

            eCastingSkillAttackClip_01 = skillAttackAnimationClips[0].loading_Attack_Animation_Clip;
            eCastingSkillAttackClip_02 = skillAttackAnimationClips[1].loading_Attack_Animation_Clip;

        }
        animatorOverrideController[E_CHARGED_ATTACK_1] = eSkillAttackClip_01;
        animatorOverrideController[E_CHARGED_ATTACK_2] = eSkillAttackClip_02;

        animatorOverrideController[E_CHARGING_ATTACK_1] = eCastingSkillAttackClip_01;
        animatorOverrideController[E_CHARGING_ATTACK_2] = eCastingSkillAttackClip_02;

    }
}
