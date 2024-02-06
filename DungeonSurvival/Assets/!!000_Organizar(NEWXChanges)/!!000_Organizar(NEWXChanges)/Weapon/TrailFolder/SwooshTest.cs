using UnityEngine;

public class SwooshTest : MonoBehaviour
{
    private MeleeWeaponTrail _trailRight;
    private MeleeWeaponTrail _trailLeft;
    private PlayerStats playerStats;
    private PlayerAnimations playerAnimations;
    private AnimatorStateInfo _animationState;

    private bool _isAnimationPlaying = false;

    private void Awake ( )
    {
        playerAnimations = GetComponent<PlayerAnimations>();
        playerStats = GetComponent<PlayerStats>();
    }
    void Start ( )
    {
        _trailRight = playerStats.EquipmentDataHolder_RightHand.GetWeaponTrail();
        _trailLeft = playerStats.EquipmentDataHolder_LeftHand.GetWeaponTrail();
        DeactivateTrail();
    }

    void Update ( )
    {
        if (playerAnimations != null)
        {
            _animationState = playerAnimations.GetCurrentAnimationInfo(playerAnimations.COMBAT_LAYER, playerAnimations.ANIMATION_STATE_BASIC_ATTACK_TREE_PERFORMED_NAME);//NEED TO DECLARE A STRING WITH THE HOLDER OF THE BASE LAYER

            AnimatorStateInfo[] states = { playerAnimations.GetCurrentAnimationInfo(playerAnimations.COMBAT_LAYER, playerAnimations.ANIMATION_STATE_BASIC_ATTACK_TREE_PERFORMED_NAME),
                 playerAnimations.GetCurrentAnimationInfo(playerAnimations.COMBAT_LAYER, playerAnimations.ANIMATION_STATE_CHARGED_ATTACK_TREE_PERFORMED_NAME),
                  playerAnimations.GetCurrentAnimationInfo(playerAnimations.COMBAT_LAYER, playerAnimations.ANIMATION_STATE_SPECIAL_ATTACK_TREE_PERFORMED_NAME),
                   playerAnimations.GetCurrentAnimationInfo(playerAnimations.COMBAT_LAYER, playerAnimations.ANIMATION_STATE_SKILL_ATTACK_TREE_PERFORMED_NAME) };

            foreach (AnimatorStateInfo state in states)
            {
                if (IsStateActive(state))
                {
                    ActivateTrails();
                }
                else
                {
                    DeactivateTrail();
                }
            }
        }
    }

    private bool IsStateActive(AnimatorStateInfo state )
    {
        return playerAnimations.GetAnimator().GetCurrentAnimatorStateInfo(playerAnimations.COMBAT_LAYER).fullPathHash == state.fullPathHash;
    }
    private void ActivateTrails ( )
    {
        if (_trailRight != null)
        {
            _trailRight.Emit = true;
        }
        if(_trailLeft != null)
        {
            _trailLeft.Emit = true;
        }
    }

    private void DeactivateTrail ( )
    {
        if (_trailRight != null)
        {
            _trailRight.Emit = false;
        }
        if(_trailLeft != null)
        {
            _trailLeft.Emit = false;
        }
    }
}