using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum State
{
    Idle,
    Patrol,
    Chasing,
    Hostile,
    HalfDeath,
    Death
}
public class AI_MainCore : MonoBehaviour
{
    [SerializeField] private MonoBehaviour idleBehaviour;
    [SerializeField] private MonoBehaviour patrolBehaviour;
    [SerializeField] private MonoBehaviour chasingBehaviour;
    [SerializeField] private MonoBehaviour hostileBehaviour;
    [SerializeField] private MonoBehaviour nearDieBehaviour;
    [SerializeField] private MonoBehaviour deathBehaviour;
    [SerializeField] private MonoBehaviour actualBehaviour;
    

    private State state;
    private Transform threat;
    private void Start ( )
    {
        state = State.Idle;
    }
    private void Update ( )
    {
        switch ( state )
        {
            case State.Idle:
                //Do Nothing
                SetNewBehaviour(idleBehaviour);
                break;
            case State.Patrol:
                //Start Patrolling
                SetNewBehaviour(patrolBehaviour);
                break;
            case State.Chasing:
                //Start chasing the player
                SetNewBehaviour(chasingBehaviour);
                break;
            case State.Hostile:
                //Attacks player if it's on range (bas. atk / sp atk)
                SetNewBehaviour(hostileBehaviour);
                break;
            case State.HalfDeath:
                //Depending on the mob it raises fury/cobardy...
                SetNewBehaviour(nearDieBehaviour);
                break;
            case State.Death:
                //Dies
                SetNewBehaviour(deathBehaviour);

                break;
            default:
                break;
        }
    }
    public MonoBehaviour GetActualBehaviur { get { return actualBehaviour; } }
    private void SetNewBehaviour (MonoBehaviour newBehaviour )
    {
        if(actualBehaviour != newBehaviour)
        {
            if (actualBehaviour != null) actualBehaviour.enabled = false;
            actualBehaviour = newBehaviour;
            actualBehaviour.enabled = true;
        }
        else
        {
            return;
        }
    }
    public void SetState ( State state )
    {
        this.state = state;
    }
    public State GetCurrentState ( )
    {
        return state;
    }

    public void SetThreat ( Transform threatTransform )
    {
        threat = threatTransform;
    }
    public Transform GetThreat ( )
    {
        return threat;
    }
}
