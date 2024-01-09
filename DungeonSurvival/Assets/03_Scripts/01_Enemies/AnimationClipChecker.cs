using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipChecker : MonoBehaviour
{
    [SerializeField] private AnimationClipContainerSO animationClipContainerSO;
    private void Awake ( )
    {
        InitializeAnimationsChecker();
    }
    private void InitializeAnimationsChecker ( )
    {
        Animator animator = GetComponent<Animator> ();
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController ( animator.runtimeAnimatorController);

        animator.runtimeAnimatorController = animatorOverrideController;

        animationClipContainerSO.ChangeCurrentAnimations(animatorOverrideController);
    }
}
