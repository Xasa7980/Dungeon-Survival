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
    private bool hit = true;

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
        CheckForEnemies();
    }
    private void CheckForEnemies ( )
    {
        Collider[] rightDetection = GetDetectionColliders(playerStats.RightDetectionArea);
        Collider[] leftDetection = GetDetectionColliders(playerStats.LeftDetectionArea);

        if (playerAnimations.SelectCurrentAnimatorState(playerAnimations.COMBAT_LAYER).normalizedTime < 0.95f)
        {
            if (rightDetection.Length > 0)
            {
                CheckAndDamageEnemies(rightDetection, playerStats.EquipmentDataHolder_RightHand);
            }
            else if (leftDetection.Length > 0)
            {
                CheckAndDamageEnemies(leftDetection, playerStats.EquipmentDataHolder_LeftHand);
            }
        }
        else
        {
            hit = false;
        }
    }
    private Collider[] GetDetectionColliders ( AreaDrawer detectionArea )
    {
        Collider[] detectionColliders = new Collider[0];
        if (detectionArea != null)
        {
            if (detectionArea.drawMode == AreaDrawer.DrawMode.Box)
            {
                detectionColliders = Physics.OverlapBox(detectionArea.objectPosition, detectionArea.size, detectionArea.rotation, detectionMask);
            }
            else if (detectionArea.drawMode == AreaDrawer.DrawMode.Sphere)
            {
                detectionColliders = Physics.OverlapSphere(detectionArea.objectPosition, detectionArea.radius, detectionMask);
            }
        }
        return detectionColliders;
    }
    private void CheckAndDamageEnemies ( Collider[] detectionColliders, EquipmentDataHolder equipmentDataHolder )
    {
        if (!hit)
        {
            foreach (Collider target in detectionColliders)
            {
                if (target.TryGetComponent<MonsterStats>(out MonsterStats monsterStats))
                {
                    playerStats.TakeDamage(monsterStats, equipmentDataHolder);
                    hit = true;
                    break;
                }
            }
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
