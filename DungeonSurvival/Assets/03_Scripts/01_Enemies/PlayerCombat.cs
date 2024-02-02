using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public event EventHandler OnBasicAttackPerformed;
    public event EventHandler OnChargedAttackPerformed;
    public event EventHandler OnSpecialAttackPerformed;
    public event EventHandler OnSkillAttackPerformed;

    [SerializeField] private LayerMask detectionMask;

    private PlayerStats playerStats;
    private PlayerAnimations playerAnimations;
    private bool hit;

    private int timesHit;
    private int totalAttackHits = 1; //Cantidad de veces que la habilidad toca al jugador, ejemplo combo de 2 slashes
    private void Awake ( )
    {
        playerStats = GetComponent<PlayerStats>();
        playerAnimations = GetComponent<PlayerAnimations>();
    }
    
    private void Start ( )
    {
        
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
                            timesHit ++;

                            playerStats.TakeDamage(monsterStats, playerStats.EquipmentDataHolder_RightHand);

                            if (timesHit >= totalAttackHits)
                            {
                                timesHit = 0;
                                hit = true;
                            }
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
                            timesHit++;
                            playerStats.TakeDamage(monsterStats, playerStats.EquipmentDataHolder_LeftHand);
                            if (timesHit >= totalAttackHits)
                            {
                                timesHit = 0;
                                hit = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //PLAYER NOT ATTACKED OR ANIMATION ISN'T PERFORMED
            hit = false;
        }
    }
}
