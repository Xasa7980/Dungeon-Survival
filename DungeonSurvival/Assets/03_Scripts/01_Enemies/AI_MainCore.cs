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
    private MonsterStats monsterStats;
    private Transform threat;

    private void Awake ( )
    {
        state = State.Idle;
        monsterStats = GetComponent<MonsterStats>();
    }
    private void Start ( )
    {
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
    #region Getters & Setters
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
    public AI_IdleBehaviour GetIdleBehaviour ( )
    {
        return (AI_IdleBehaviour) idleBehaviour;
    }
    public AI_PatrolBehaviour GetPatrolBehaviour ( )
    {
        return (AI_PatrolBehaviour) patrolBehaviour;
    }
    public AI_ChasingBehaviour GetChasingBehaviour ( )
    {
        return (AI_ChasingBehaviour) chasingBehaviour;
    }
    public AI_HostileBehaviour GetHostileBehaviour ( )
    {
        return (AI_HostileBehaviour) hostileBehaviour;
    }
    public AI_NearDieBehaviour GetNearDieBehaviour ( )
    {
        return (AI_NearDieBehaviour) nearDieBehaviour;
    }
    public AI_DeathBehaviour GetDeathBehaviour ( )
    {
        return (AI_DeathBehaviour) deathBehaviour;
    }
    public MonsterStats GetMonsterStats ( )
    {
        return monsterStats;
    }
    #endregion
}
