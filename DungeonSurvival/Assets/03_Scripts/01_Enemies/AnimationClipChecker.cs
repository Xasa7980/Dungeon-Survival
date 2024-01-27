using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipChecker : MonoBehaviour
{
    [SerializeField] private AnimationClipContainerSO animationClipContainerSO;
    private Animator animator;
    private void Awake ( )
    {
        animator = GetComponent<Animator> ();
        InitializeAnimationsChecker();
    }
    private void InitializeAnimationsChecker ( )
    {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController ( animator.runtimeAnimatorController);

        animator.runtimeAnimatorController = animatorOverrideController;

        animationClipContainerSO.ChangeCurrentAnimations(animatorOverrideController);
    }
}
