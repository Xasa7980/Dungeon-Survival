using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private const string IS_WALKING_BOOL = "IsWalking";
    private const string IS_RUNNING_BOOL = "IsRunning";
    private const string ATTACK_TRIGGER_FLOAT = "Attack";
    private const string ATTACK_INDEX = "AttackIndex";
    private const string DEATH_TRIGGER = "Death";

    private const int BASE_LAYER_INDEX = 0;
    private const int ATTACK_LAYER_INDEX = 1;
    private const int ADDITIVE_EFFECTS_LAYER_INDEX = 2;

    [SerializeField] private int numberOfAttacks = 4;
    private bool isWalking;
    private bool isRunning;
    private float basicAttackIndex;
    private Animator animator;
    private void Awake ( )
    {
        animator = GetComponent<Animator>();
    }

    private void Start ( )
    {
        AI_PatrolBehaviour.OnWalkAction += AI_PatrolBehaviour_OnWalkAction;
        AI_ChasingBehaviour.OnRunAction += AI_ChasingBehaviour_OnRunAction;
        AI_HostileBehaviour.OnBasicAttack += AI_HostileBehaviour_OnBasicAttack;
    }

    private void AI_PatrolBehaviour_OnWalkAction ( object sender, AI_PatrolBehaviour.OnWalkActionEventArgs e )
    {
        isWalking = e.isWalking;
    }

    private void AI_ChasingBehaviour_OnRunAction ( object sender, AI_ChasingBehaviour.OnRunActionEventArgs e )
    {
        isRunning = e.isRunning;
        isWalking = e.isWalking;
    }

    private void AI_HostileBehaviour_OnBasicAttack ( object sender, System.EventArgs e )
    {
        basicAttackIndex = (int)UnityEngine.Random.Range(1, numberOfAttacks - 1);

        if (basicAttackIndex > numberOfAttacks -1)
        {
            basicAttackIndex = 0;
        }

        animator.SetLayerWeight(ATTACK_LAYER_INDEX, 1);

        animator.SetTrigger(ATTACK_TRIGGER_FLOAT);
        animator.SetFloat(ATTACK_INDEX, basicAttackIndex);

        basicAttackIndex += 1;
    }

    private void Update ( )
    {
        if(animator.GetCurrentAnimatorStateInfo(ATTACK_LAYER_INDEX).IsTag("AttackCombo"))
        {
            if(animator.GetCurrentAnimatorStateInfo(ATTACK_LAYER_INDEX).normalizedTime > 1f)
            {
                animator.SetLayerWeight(ATTACK_LAYER_INDEX, 0);
            }
            print("animacionTerminada");
        }
        animator.SetBool(IS_WALKING_BOOL, isWalking);
        animator.SetBool(IS_RUNNING_BOOL, isRunning);
    }
}
