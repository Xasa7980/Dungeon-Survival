using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private AnimationClipContainerSO animationClipContainerSO;

    private Animator animator;
    private PlayerStats playerStats;
    private void Awake ( )
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        InitializeAnimationsChecker();
    }
    private void Start ( )
    {
    }
    private void InitializeAnimationsChecker ( )
    {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        animator.runtimeAnimatorController = animatorOverrideController;

        animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ChangeCurrentAnimations(animatorOverrideController, 
            playerStats.EquipmentDataHolder_RightHand.GetEquipmentDataSO());
    }

}
