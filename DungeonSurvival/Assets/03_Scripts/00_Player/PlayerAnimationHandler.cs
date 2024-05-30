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
        PlayerInventory.current.OnChangeWeapon += PlayerInventory_OnChangeWeapon;
        Debug.Log(animatorOverrideController["LoadingBasicAttack_Animation_01"]);
        Debug.Log(animatorOverrideController["LoadingChargedAttack_Animation_02"]);
        Debug.Log(animatorOverrideController["ChargedAttack_Animation_01"]);
        Debug.Log(animatorOverrideController["ChargedAttack_Animation_02"]);

    }

    private void PlayerInventory_OnChangeWeapon ( object sender, PlayerInventory.OnChangeWeaponEventArgs e )
    {
        ChangeCurrentCombatAnimations(animatorOverrideController, e.equipmentDataSO);
    }

    private void GetAnimationsToOverride ( )
    {

        animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ChangeCurrentBasicAnimations(animatorOverrideController);
    }

    private void ChangeCurrentCombatAnimations ( AnimatorOverrideController animatorOverrideController, EquipmentDataSO equipmentDataSO )
    {
        if (!PlayerInventory.current.PlayerHasAnyWeapon())
        {
            animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).NoWeaponOverride(animatorOverrideController);
        }
        else
        {
            if(PlayerInventory.current.PlayerHasAnyWeapon())
            {
                animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).WeaponChangeOverride(animatorOverrideController, 
                    PlayerInventory.current.TryGetMainWeapon().equipmentDataSO );
            }
            else if (PlayerInventory.current.PlayerHasDoubleWeapon())
            {
                if (PlayerInventory.current.TryGetSecondaryWeapon().equipmentDataSO.equipmentType == EquipmentType.Sword)
                {
                    animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).DualSwordOverride(animatorOverrideController);
                }
                else if (PlayerInventory.current.TryGetSecondaryWeapon().equipmentDataSO.equipmentType == EquipmentType.Dagger)
                {
                    animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).DualDaggerOverride(animatorOverrideController);
                }
                else if (PlayerInventory.current.TryGetSecondaryWeapon().equipmentDataSO.equipmentType == EquipmentType.Shield)
                {
                    animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ShieldOverride(animatorOverrideController);
                }
            }
        }
    }
}
