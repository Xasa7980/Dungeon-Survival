using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerComponents : MonoBehaviour
{
    public static PlayerComponents instance;

    public StandaloneInputModule standaloneInputModule_EventSystem;
    public bool lockedAll;

    public PlayerStats playerStats;
    public PlayerLocomotion playerLocomotion;
    public PlayerCombat playerCombat;
    public PlayerInteraction playerInteraction;
    public PlayerAnimations playerAnimations;
    public PlayerAnimationHandler animationHandler;
    public PlayerHolsterHandler holsterHandler;
    public Animator anim { get; private set; }
    private void Awake ( )
    {
        instance = this;
        anim = GetComponent<Animator>();
    }
}
