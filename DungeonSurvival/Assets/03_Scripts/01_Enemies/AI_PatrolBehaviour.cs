using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_PatrolBehaviour : MonoBehaviour
{
    public event EventHandler OnDestinationReached;

    [SerializeField] private float positionRange = 30f;
    [SerializeField] private NavMeshAgent navAgent;
    private void OnEnable ( )
    {
        Patrolling();
    }
    private void Update ( )
    {
        if(navAgent.isPathStale )
        {
            print("IsPathStale");
            Patrolling();
        }
        if (navAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            print("Path Invalid");
            AI_MainCore.instance.SetState(State.Idle);
            return;
        }
        else
        {
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                print("PathCompleted");
                //Path completed
                OnDestinationReached?.Invoke(this, EventArgs.Empty);
                AI_MainCore.instance.SetState(State.Idle);
            }
            else
            {
                print("Doing Path");
            }
        }
    }
    private void Patrolling()
    {
        navAgent.SetDestination(GetRandomLocation());
    }
    private Vector3 GetRandomLocation ( )
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * positionRange; /* Genera una dirección aleatoria dentro de una esfera de radio
                                                                                        * "positionRange" dandole una distribucion uniforme de los 
                                                                                        * puntos en todas direcciones */
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if(NavMesh.SamplePosition(randomDirection,out hit,positionRange,NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        else
        {
            finalPosition = transform.position;
        }

        return finalPosition;
    }
}
