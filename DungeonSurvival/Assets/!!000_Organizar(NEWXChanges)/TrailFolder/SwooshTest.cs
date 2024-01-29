using UnityEngine;

public class SwooshTest : MonoBehaviour
{
    //Gettear las animaciones del override, y añadir un switch para que hacer una maquina de estados simple para que verifique si es ataque habilidad, carga, etc
    [SerializeField]
    private AnimationClip _animation;

    [SerializeField]
    private MeleeWeaponTrail _trail;

    private Animator _animator;
    private AnimatorStateInfo _animationState;

    private bool _isAnimationPlaying = false;

    void Start ( )
    {
        _animator = GetComponent<Animator>();
        if (_trail != null)
        {
            _trail.Emit = false;
        }
    }

    void Update ( )
    {
        if (_animator != null)
        {
            _animationState = _animator.GetCurrentAnimatorStateInfo(1);//NEED TO DECLARE A STRING WITH THE HOLDER OF THE BASE LAYER

            if (_animationState.IsName("AttackBasicTree"))
            {
                if (!_isAnimationPlaying)
                {
                    _isAnimationPlaying = true;
                    ActivateTrail();
                }
            }
            else
            {
                if (_isAnimationPlaying)
                {
                    _isAnimationPlaying = false;
                    DeactivateTrail();
                }
            }
        }
    }

    private void ActivateTrail ( )
    {
        if (_trail != null)
        {
            _trail.Emit = true;
        }
    }

    private void DeactivateTrail ( )
    {
        if (_trail != null)
        {
            _trail.Emit = false;
        }
    }
}