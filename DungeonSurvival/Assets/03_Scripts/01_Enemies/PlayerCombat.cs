using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, ICombatBehaviour
{
    public event EventHandler OnAnimationEventCalls;
    public event EventHandler OnAnimationEventReleaseEffect;

    public event EventHandler OnBasicAttackPerformed;
    public event EventHandler OnChargedAttackPerformed;
    public event EventHandler OnSpecialAttackPerformed;
    public event EventHandler OnSkillAttackPerformed;

    [SerializeField] private LayerMask detectionMask;

    private PlayerStats playerStats;
    private PlayerAnimations playerAnimations;
    public bool hit;

    private void Awake ( )
    {
        playerStats = GetComponent<PlayerStats>();
        playerAnimations = GetComponent<PlayerAnimations>();
    }
    
    private void Start ( )
    {
        OnAnimationEventCalls += PlayerCombat_OnAnimationEventCalls;
        OnAnimationEventReleaseEffect += PlayerCombat_OnAnimationEventReleaseEffect;
    }
    private void PlayerCombat_OnAnimationEventReleaseEffect ( object sender, EventArgs e )
    {
        throw new NotImplementedException();
    }

    private void PlayerCombat_OnAnimationEventCalls ( object sender, EventArgs e )
    {
        if(hit)
        {
            print("unhitted");
            hit = false;
        }
    }

    private void Update ( )
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnBasicAttackPerformed?.Invoke(this, EventArgs.Empty);
        }
        WeaponDetectMonsters();
    }
    private void WeaponDetectMonsters ( )
    {
        Collider[] rightDetection = Physics.OverlapBox(playerStats.RightDetectionArea.objectPosition, playerStats.RightDetectionArea.size,
        playerStats.RightDetectionArea.rotation, detectionMask);
        Collider[] leftDetection = Physics.OverlapBox(playerStats.LeftDetectionArea.objectPosition, playerStats.LeftDetectionArea.size,
        playerStats.LeftDetectionArea.rotation, detectionMask);

        if (playerAnimations.GetCurrentAnimationInfo(playerAnimations.COMBAT_LAYER, playerAnimations.ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME).normalizedTime < 0.85f)
        {
            if(rightDetection.Length > 0)
            {
                if (!hit)
                {
                    foreach (Collider target in rightDetection)
                    {
                        if (target.TryGetComponent<MonsterStats>(out MonsterStats monsterStats))
                        {
                            print("rightHitted");

                            playerStats.TakeDamage(monsterStats, playerStats.EquipmentDataHolder_RightHand);
                            hit = true;
                        }
                    }
                }
            }
            else if (leftDetection.Length > 0)
            {
                if (!hit)
                {
                    foreach (Collider target in leftDetection)
                    {
                        if (target.TryGetComponent<MonsterStats>(out MonsterStats monsterStats))
                        {
                            print("leftHitted");

                            playerStats.TakeDamage(monsterStats, playerStats.EquipmentDataHolder_LeftHand);
                            hit = true;
                        }
                    }
                }
            }
        }
        else
        {
            hit = false;
        }
    }
    public void OnAnimationEvent_AttackCallback ( )
    {
        OnAnimationEventCalls?.Invoke( this,EventArgs.Empty );
    }
    public void OnAnimationEvent_AttackEffectCallback ( )
    {
        OnAnimationEventReleaseEffect?.Invoke( this,EventArgs.Empty );
    }
}
