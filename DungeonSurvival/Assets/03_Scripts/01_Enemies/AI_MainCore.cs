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
    public static AI_MainCore instance;

  
    [SerializeField] private MonoBehaviour idleBehaviour;
    [SerializeField] private MonoBehaviour patrolBehaviour;
    [SerializeField] private MonoBehaviour chasingBehaviour;
    [SerializeField] private MonoBehaviour hostileBehaviour;
    [SerializeField] private MonoBehaviour nearDieBehaviour;
    [SerializeField] private MonoBehaviour deathBehaviour;
    [SerializeField] private MonoBehaviour actualBehaviour;

    [SerializeField] private float patrolTimerMax = 10;
    [SerializeField] private float idleTimerMax = 3;
    private float patrolTimer;
    private float idleTimer;
    

    private State state;

    private void Awake ( )
    {
        instance = this;
    }
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
                idleTimer += Time.deltaTime;

                if(idleTimer > idleTimerMax)
                {
                    idleTimer = 0;
                    state = State.Patrol;
                }
                break;
            case State.Patrol:
                //Start Patrolling
                SetNewBehaviour(patrolBehaviour);
                patrolTimer += Time.deltaTime;

                if (patrolTimer > patrolTimerMax)
                {
                    patrolTimer = 0;
                    state = State.Idle;
                }
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
    public void SetNewBehaviour (MonoBehaviour newBehaviour )
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
    public State SetState ( State state )
    {
        return this.state = state;
    }
}
