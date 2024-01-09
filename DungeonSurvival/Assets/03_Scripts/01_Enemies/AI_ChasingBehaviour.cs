using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_ChasingBehaviour : MonoBehaviour
{
    public static event EventHandler<OnRunActionEventArgs> OnRunAction;
    public class OnRunActionEventArgs : EventArgs
    {
        public bool isRunning;
        public bool isWalking;
    }
    [SerializeField] private float speedMultiplier;

    private NavMeshAgent navAgent;
    private AI_MainCore ai_MainCore;
    private bool isRunning;
    private bool isWalking;
    private float normalSpeed = 1f;

    private void OnEnable ( )
    {
        ai_MainCore = GetComponent<AI_MainCore>();
        navAgent = GetComponent<NavMeshAgent>();
        normalSpeed = navAgent.speed;
        navAgent.speed = normalSpeed * speedMultiplier;
        navAgent.SetDestination(GetThreatLocation());
    }
    private void OnDisable ( )
    {
        isRunning = false;
        isWalking = false;
        OnRunAction?.Invoke(this, new OnRunActionEventArgs
        {
            isRunning = isRunning,
            isWalking = isWalking,
        });
        navAgent.isStopped = false;
        navAgent.speed = normalSpeed;
    }
    private void Update ( )
    {
        if(Vector3.Distance(transform.position, GetThreatLocation()) > 1.5f)
        {
            if(!isRunning)
            {
                isRunning = true;
                isWalking = true;
            }
            OnRunAction?.Invoke(this, new OnRunActionEventArgs
            {
                isRunning = isRunning,
                isWalking = isWalking,
            });
            navAgent.SetDestination(GetThreatLocation());
        }
        else if (navAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            navAgent.isStopped = true;
            ai_MainCore.SetState(State.Idle);
            return;
        }
    }
    private Vector3 GetThreatLocation ( )
    {
        if (ai_MainCore.GetThreat() != null)
        {
            Vector3 threatPos = ai_MainCore.GetThreat().position;
            return threatPos;
        }
        else
        {
            ai_MainCore.SetState (State.Patrol);
            return transform.position;
        }
    }
}
