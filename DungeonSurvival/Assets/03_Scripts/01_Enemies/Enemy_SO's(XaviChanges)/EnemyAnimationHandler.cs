using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    [SerializeField] private AnimationClipContainerSO animationClipContainerSO;

    private Animator animator;
    private MonsterStats monsterStats;
    private AnimatorOverrideController animatorOverrideController;
    private void Awake ( )
    {
        animator = GetComponent<Animator>();
        monsterStats = GetComponent<MonsterStats>();
    }
    private void Start ( )
    {
        GetAnimationsToOverride();
    }
    private bool noWeaponsEquiped => monsterStats.EquipmentDataSO_RightHand == null ? true : false;
    private void GetAnimationsToOverride ( )
    {
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        animator.runtimeAnimatorController = animatorOverrideController;

        animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ChangeCurrentBasicAnimations(animatorOverrideController);

        ChangeCurrentCombatAnimations(animatorOverrideController);
    }

    private void ChangeCurrentCombatAnimations ( AnimatorOverrideController animatorOverrideController )
    {
        if (noWeaponsEquiped)
        {
            animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).NoWeaponOverride(animatorOverrideController);
        }
        else
        {
            if (monsterStats.IsDualWeaponWielding)
            {
                if (monsterStats.EquipmentDataHolder_RightHand.GetEquipmentType() == EquipmentType.Sword)
                {
                    animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).DualSwordOverride(animatorOverrideController);
                }
                else if (monsterStats.EquipmentDataHolder_RightHand.GetEquipmentType() == EquipmentType.Dagger)
                {
                    animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).DualDaggerOverride(animatorOverrideController);
                }
                else if (monsterStats.EquipmentDataHolder_LeftHand.GetEquipmentType() == EquipmentType.Shield)
                {
                    animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ShieldOverride(animatorOverrideController);
                }
            }
            else
            {
                animationClipContainerSO.GetPlayerAnimationContainer(animationClipContainerSO).ChangeCurretCombatAnimations(animatorOverrideController,
                monsterStats.EquipmentDataHolder_RightHand.GetEquipmentDataSO());
            }
        }
        Debug.Log(monsterStats.EquipmentDataHolder_RightHand + " = " + animationClipContainerSO.attackBasicClip_01);
    }
}
