using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Sensor : MonoBehaviour
{
    [SerializeField] protected float detectionRadius;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] protected LayerMask detectionMask;

    public abstract bool ThreatsDetected();

    public abstract Transform GetNearestThreat();

    public abstract bool InAttackRange(Vector3 position);

    public bool DirectLineToTarget(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        Ray ray = new Ray(transform.position, dir);
        if (Physics.SphereCast(ray, 0.2f, detectionRadius, obstacleMask))
        {
            return false;
        }

        return true;
    }

    [SerializeField] protected UnityEvent OnThreatIsDetected = new UnityEvent();
    [SerializeField] protected UnityEvent OnThreatIsInAttackRange = new UnityEvent();
}
