using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationsContainer", menuName = "Dungeon Survival/Enemy/Animations")]
public class AnimationClipContainerSO : ScriptableObject
{
    public const string E_ATTACK_BASIC_1 = "E_Attack_Basic_1";
    public const string E_ATTACK_BASIC_2 = "E_Attack_Basic_2";
    public const string E_ATTACK_BASIC_3 = "E_Attack_Basic_3";
    public const string E_ATTACK_BASIC_4 = "E_Attack_Basic_4";
    public const string E_DEATHBACK = "E_DeathBack";
    public const string E_IDLE = "E_Idle";
    public const string E_IDLECOMBAT = "E_IdleCombat";
    public const string E_RUN_FORWARD = "E_Run_Forward";
    public const string E_WALK_FORWARD = "E_Walk_Forward";

    public bool getRandomAttackComboAnimation;
    [ShowIf("@getRandomAttackComboAnimation == true")] public List<AnimationClip> basicAttackAnimationClips = new List<AnimationClip>();

    [ShowIf("@getRandomAttackComboAnimation == false")] public AnimationClip eAttackBasic1Clip;
    [ShowIf("@getRandomAttackComboAnimation == false")] public AnimationClip eAttackBasic2Clip;
    [ShowIf("@getRandomAttackComboAnimation == false")] public AnimationClip eAttackBasic3Clip;
    [ShowIf("@getRandomAttackComboAnimation == false")] public AnimationClip eAttackBasic4Clip;

    public AnimationClip eDeathBackClip;
    public AnimationClip eIdleClip;
    public AnimationClip eIdleCombatClip;
    public AnimationClip eRunForwardClip;
    public AnimationClip eWalkForwardClip;
    public void ChangeCurrentAnimations ( AnimatorOverrideController animatorOverrideController )
    {
        if(getRandomAttackComboAnimation)
        {
            List<int> selectedIndices = new List<int>();

            while (selectedIndices.Count < 4)
            {
                int index = Random.Range(0, basicAttackAnimationClips.Count);
                if (!selectedIndices.Contains(index))
                {
                    selectedIndices.Add(index);
                }
            }

            eAttackBasic1Clip = basicAttackAnimationClips[selectedIndices[0]];
            eAttackBasic2Clip = basicAttackAnimationClips[selectedIndices[1]];
            eAttackBasic3Clip = basicAttackAnimationClips[selectedIndices[2]];
            eAttackBasic4Clip = basicAttackAnimationClips[selectedIndices[3]];
        }
        else
        {
            animatorOverrideController[E_ATTACK_BASIC_1] = eAttackBasic1Clip;
            animatorOverrideController[E_ATTACK_BASIC_2] = eAttackBasic2Clip;
            animatorOverrideController[E_ATTACK_BASIC_3] = eAttackBasic3Clip;
            animatorOverrideController[E_ATTACK_BASIC_4] = eAttackBasic4Clip;
        }
        animatorOverrideController[E_DEATHBACK] = eDeathBackClip;
        animatorOverrideController[E_IDLE] = eIdleClip;
        animatorOverrideController[E_IDLECOMBAT] = eIdleCombatClip;
        animatorOverrideController[E_RUN_FORWARD] = eRunForwardClip;
        animatorOverrideController[E_WALK_FORWARD] = eWalkForwardClip;
    }
}
