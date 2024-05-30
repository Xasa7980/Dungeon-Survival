using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    public AnimationClipContainerSO AnimationClipContainerSO => animationClipContainerSO;
    [SerializeField] private AnimationClipContainerSO animationClipContainerSO;

    private Animator animator;
    private MonsterStats monsterStats;
    private AnimatorOverrideController animatorOverrideController;
    private void Awake ( )
    {
        animator = GetComponent<Animator>();
        monsterStats = GetComponentInParent<MonsterStats>();

        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
    }
    private void Start ( )
    {
        GetAnimationsToOverride();
        ChangeCurrentCombatAnimations(animatorOverrideController);
        monsterStats.OnWeaponChanged += MonsterStats_OnWeaponChanged;

    }
    private void MonsterStats_OnWeaponChanged ( object sender, System.EventArgs e )
    {
        ChangeCurrentCombatAnimations(animatorOverrideController);
    }

    private bool noWeaponsEquiped => monsterStats.EquipmentDataSO_RightHand == null ? true : false;
    private void GetAnimationsToOverride ( )
    {
        animationClipContainerSO.GetEnemyAnimationClipContainer(animationClipContainerSO).ChangeCurrentBasicAnimations(animatorOverrideController, monsterStats);

    }

    private void ChangeCurrentCombatAnimations ( AnimatorOverrideController animatorOverrideController )
    {
        if (noWeaponsEquiped)
        {
            animationClipContainerSO.GetEnemyAnimationClipContainer(animationClipContainerSO).NoWeaponOverride(animatorOverrideController);
        }
        else
        {
            if (monsterStats.IsDualWeaponWielding)
            {
                if (monsterStats.EquipmentDataHolder_RightHand.GetEquipmentType() == EquipmentType.Sword)
                {
                    animationClipContainerSO.GetEnemyAnimationClipContainer(animationClipContainerSO).DualSwordOverride(animatorOverrideController);
                }
                else if (monsterStats.EquipmentDataHolder_RightHand.GetEquipmentType() == EquipmentType.Dagger)
                {
                    animationClipContainerSO.GetEnemyAnimationClipContainer(animationClipContainerSO).DualDaggerOverride(animatorOverrideController);
                }
                else if (monsterStats.EquipmentDataHolder_LeftHand.GetEquipmentType() == EquipmentType.Shield)
                {
                    animationClipContainerSO.GetEnemyAnimationClipContainer(animationClipContainerSO).ShieldOverride(animatorOverrideController);
                }
            }
            else
            {
                animationClipContainerSO.GetEnemyAnimationClipContainer(animationClipContainerSO).WeaponChangeOverride(animatorOverrideController,
                monsterStats.EquipmentDataHolder_RightHand.GetEquipmentDataSO());
            }
        }
        Debug.Log(monsterStats.EquipmentDataHolder_RightHand.name);
    }
}
