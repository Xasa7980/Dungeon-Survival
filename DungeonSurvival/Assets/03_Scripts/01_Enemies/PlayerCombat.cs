using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public event EventHandler OnBasicAttack;

    [SerializeField] private GameObject rightWeaponHandler;
    [SerializeField] private GameObject leftWeaponHandler;
    [SerializeField] private LayerMask detectionMask;

    private EquipmentDataHolder equipmentDataHolder_LeftHand;
    private EquipmentDataHolder equipmentDataHolder_RightHand;
    private EquipmentDataSO equipmentDataSO_RightHand;
    private EquipmentDataSO equipmentDataSO_LeftHand;
    private AreaDrawer leftDetectionArea;
    private AreaDrawer rightDetectionArea;
    private PlayerStats playerStats;
    private PlayerAnimations playerAnimations;
    private bool hit;

    private int timesHit;
    private int totalAttackHits = 1; //Cantidad de veces que la habilidad toca al jugador, ejemplo combo de 2 slashes
    private void Awake ( )
    {
        playerStats = GetComponent<PlayerStats>();
        playerAnimations = GetComponent<PlayerAnimations>();

        equipmentDataHolder_RightHand = rightWeaponHandler.transform.GetChild(0).GetComponent<EquipmentDataHolder>();
        equipmentDataHolder_LeftHand = leftWeaponHandler.transform.GetChild(0).GetComponent<EquipmentDataHolder>();

        equipmentDataSO_RightHand = equipmentDataHolder_RightHand.GetEquipmentDataSO(); //HACER UNO PARA LOS RANGES QUE NO TENDRÁN AREA DRAWER EN EL ARCO SI NO EN LA FLECHA, LA FLECHA CALCULA DISTANCIAS
        equipmentDataSO_LeftHand = equipmentDataHolder_LeftHand.GetEquipmentDataSO();
        rightDetectionArea = equipmentDataHolder_RightHand.GetDetectionArea();
        leftDetectionArea = equipmentDataHolder_LeftHand.GetDetectionArea();
    }
    private void Start ( )
    {
        
    }
    private void Update ( )
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnBasicAttack?.Invoke(this, EventArgs.Empty);
        }
        WeaponDetectMonsters();
    }
    private void WeaponDetectMonsters ( )
    {
        
        Collider[] rightDetection = Physics.OverlapBox(rightDetectionArea.objectPosition, rightDetectionArea.size,
        rightDetectionArea.rotation, detectionMask);
        Collider[] leftDetection = Physics.OverlapBox(leftDetectionArea.objectPosition, leftDetectionArea.size,
        leftDetectionArea.rotation, detectionMask);

        if (playerAnimations.GetCurrentAnimationInfo(playerAnimations.ANIMATION_ATTACK_BASIC_TREE_PERFORMED_NAME).normalizedTime < 1f)
        {
            if(!hit)
            {
                if (rightDetection.Length > 0)
                {
                    foreach (Collider target in rightDetection)
                    {
                        if (target.TryGetComponent<MonsterStats>(out MonsterStats monsterStats))
                        {
                            print("rightHitted");
                            timesHit ++;
                            playerStats.TakeDamage(monsterStats);
                            if (timesHit >= totalAttackHits)
                            {
                                timesHit = 0;
                                hit = true;
                            }
                        }
                    }
                }
                if (leftDetection.Length > 0)
                {
                    foreach (Collider target in leftDetection)
                    {
                        if (target.TryGetComponent<MonsterStats>(out MonsterStats monsterStats))
                        {
                            print("leftHitted");
                            timesHit++;
                            playerStats.TakeDamage(monsterStats);
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
