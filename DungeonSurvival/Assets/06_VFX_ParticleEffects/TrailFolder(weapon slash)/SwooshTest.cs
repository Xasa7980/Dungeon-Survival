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
        //_trailRight = playerStats.EquipmentDataHolder_RightHand.GetWeaponTrail();
        //_trailLeft = playerStats.EquipmentDataHolder_LeftHand.GetWeaponTrail();
        DeactivateTrail();
    }

    void Update ( )
    {
        if (playerAnimations != null)
        {
            _animationState = playerAnimations.SelectCurrentAnimatorState(playerAnimations.COMBAT_LAYER);//NEED TO DECLARE A STRING WITH THE HOLDER OF THE BASE LAYER

            if (playerAnimations.IsStateActive(_animationState))
            {
                ActivateTrails();
            }
            else
            {
                DeactivateTrail();
            }
        }
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