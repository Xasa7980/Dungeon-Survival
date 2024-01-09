using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_PatrolBehaviour : MonoBehaviour
{
    public event EventHandler OnDestinationReached;
    public static event EventHandler<OnWalkActionEventArgs> OnWalkAction;
    public class OnWalkActionEventArgs : EventArgs
    {
        public bool isWalking;
    }

    [SerializeField] private float positionRange = 30f;
    
    private NavMeshAgent navAgent;
    private AI_MainCore ai_MainCore;
    private bool isWalking;
    private void Awake ( )
    {
        ai_MainCore = GetComponent<AI_MainCore>();
        navAgent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable ( )
    {
        SettingDestiny();
    }
    private void Update ( )
    {
        if(navAgent.isPathStale )
        {
            print("IsPathStale");
            //Path is invalid
            SettingDestiny();
        }
        if (navAgent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            print("Path Invalid");
            //Path is invalid
            ai_MainCore.SetState(State.Idle);
            return;
        }
        else
        {
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                print("PathCompleted");
                //Path completed
                OnDestinationReached?.Invoke(this, EventArgs.Empty);
                ai_MainCore.SetState(State.Idle);
            }
            else
            {
                print("Doing Path");
                if(!isWalking)
                {
                    isWalking = true;
                }
                //Walking throught the path
                OnWalkAction?.Invoke(this, new OnWalkActionEventArgs
                {
                    isWalking = isWalking,
                });
            }
        }
    }
    private void SettingDestiny()
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
    private void OnDisable ( )
    {
        isWalking = false;
        OnWalkAction?.Invoke(this, new OnWalkActionEventArgs
        {
            isWalking = isWalking,
        });
    }
}
