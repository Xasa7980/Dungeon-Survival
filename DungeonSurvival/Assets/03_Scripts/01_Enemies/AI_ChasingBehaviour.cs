using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_ChasingBehaviour : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private float speedMultiplier;

    private float normalSpeed = 1f;
    private void OnEnable ( )
    {
        normalSpeed = navAgent.speed;
        navAgent.speed = normalSpeed * speedMultiplier;
        navAgent.SetDestination(GetThreatLocation());
    }
    private void OnDisable ( )
    {
        navAgent.isStopped = false;
        navAgent.speed = normalSpeed;
    }
    private void Update ( )
    {
        if(Vector3.Distance(transform.position, GetThreatLocation()) > 1.5f)
        {
            navAgent.SetDestination(GetThreatLocation());
        }
        if (navAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            navAgent.isStopped = true;
            AI_MainCore.instance.SetState(State.Idle);
            return;
        }
    }
    private Vector3 GetThreatLocation ( )
    {
        Vector3 threatPosition = AI_MainCore.instance.GetThreat() ? AI_MainCore.instance.GetThreat().position : transform.position;
        return threatPosition;
    }
}
