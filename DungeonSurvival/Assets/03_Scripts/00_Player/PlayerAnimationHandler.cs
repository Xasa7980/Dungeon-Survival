using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    
    public AnimationClipContainerSO GetAnimationClipContainerSO => animationClipContainerSO;
    [SerializeField] private AnimationClipContainerSO animationClipContainerSO;

    private Animator animator;
    private PlayerStats playerStats;
    private AnimatorOverrideController animatorOverrideController;
    private void Awake ( )
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

    }
    private void Start ( )
    {
        GetAnimationsToOverride();
        ChangeCurrentCombatAnimations(animatorOverrideController);
        playerStats.OnWeaponChanged += PlayerStats_OnWeaponChanged;
    }

    private void PlayerStats_OnWeaponChanged ( object sender, System.EventArgs e )
    {
        ChangeCurrentCombatAnimations(animatorOverrideController);
    }
    private void GetAnimationsToOverride ( )
    {

        animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ChangeCurrentBasicAnimations(animatorOverrideController);
    }

    private void ChangeCurrentCombatAnimations ( AnimatorOverrideController animatorOverrideController )
    {
        if (playerStats.noWeaponsEquiped)
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
                animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ChangeCurretCombatAnimations(animatorOverrideController,
                playerStats.EquipmentDataHolder_RightHand.GetEquipmentDataSO());
            }
        }
    }
}
