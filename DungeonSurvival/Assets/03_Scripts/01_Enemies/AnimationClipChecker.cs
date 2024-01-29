using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipChecker : MonoBehaviour
{
    private enum CharacterCategory
    {
        Player,
        Monster
    }
    [SerializeField] private AnimationClipContainerSO animationClipContainerSO;
    [SerializeField] private CharacterCategory characterCategory = CharacterCategory.Monster;

    private Animator animator;
    private void Awake ( )
    {
        animator = GetComponent<Animator> ();
        InitializeAnimationsChecker();
    }
    private void Start ( )
    {
    }
    private void InitializeAnimationsChecker ( )
    {
        if(characterCategory == CharacterCategory.Player)
        {
            AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

            animator.runtimeAnimatorController = animatorOverrideController;

            animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ChangeCurrentAnimations(animatorOverrideController);
        }
        else
        {
            AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

            animator.runtimeAnimatorController = animatorOverrideController;

            animationClipContainerSO.GetAnimationClipContainer(animationClipContainerSO).ChangeCurrentAnimations(animatorOverrideController);
        }
    }
}
