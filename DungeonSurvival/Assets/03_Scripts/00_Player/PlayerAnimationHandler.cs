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
    private bool noWeaponsEquiped => !playerStats.EquipmentDataSO_RightHand ? (!playerStats.EquipmentDataSO_LeftHand ? true : false) : false;
    private void InitializeAnimationsChecker ( )
    {
        AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        animator.runtimeAnimatorController = animatorOverrideController;
        if(noWeaponsEquiped )
        {
            animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).NoWeaponOverride(animatorOverrideController);
        }
        else
        {
            if (playerStats.IsDualWeaponWielding)
            {
                if (playerStats.EquipmentDataHolder_RightHand.GetEquipmentType() == EquipmentType.Sword)
                {
                    animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).DualSwordOverride(animatorOverrideController);
                }
                else if (playerStats.EquipmentDataHolder_RightHand.GetEquipmentType() == EquipmentType.Dagger)
                {
                    animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).DualDaggerOverride(animatorOverrideController);
                }
                else if (playerStats.EquipmentDataHolder_LeftHand.GetEquipmentType() == EquipmentType.Shield)
                {
                    animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ShieldOverride(animatorOverrideController);
                }
            }
            else
            {
                animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ChangeCurrentAnimations(animatorOverrideController,
                playerStats.EquipmentDataHolder_RightHand.GetEquipmentDataSO());
            }
        }
    }

}
