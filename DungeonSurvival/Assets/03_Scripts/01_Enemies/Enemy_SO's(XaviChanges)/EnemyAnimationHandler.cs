using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    [SerializeField] private AnimationClipContainerSO animationClipContainerSO;

    private Animator animator;
    private MonsterStats monsterStats;
    private void Awake ( )
    {
        monsterStats = GetComponentInParent<MonsterStats>();
        animator = GetComponent<Animator>();
        InitializeAnimationsChecker();
    }
    private void Start ( )
    {
    }
    private void InitializeAnimationsChecker ( )
    {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        animator.runtimeAnimatorController = animatorOverrideController;

        animationClipContainerSO.GetEnemyAnimationClipContainer(animationClipContainerSO).ChangeCurrentAnimations(animatorOverrideController, monsterStats.equipmentDataSO_RightHand);
    }

}
