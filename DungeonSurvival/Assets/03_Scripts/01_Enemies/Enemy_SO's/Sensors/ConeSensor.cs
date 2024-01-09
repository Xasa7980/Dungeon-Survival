using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ConeSensor : Sensor
{
    [SerializeField] Light sightField;

    public override Transform GetNearestThreat()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Vector3 center = transform.position + transform.forward * sightField.range;

        if (Physics.Raycast(ray, out RaycastHit hit, sightField.range))
        {
            center = hit.point;
        }

        Collider[] possibleTargets = Physics.OverlapSphere(center, 20, detectionMask);
        Transform nearest = null;
        float minAngle = sightField.spotAngle / 2;

        foreach (Collider target in possibleTargets)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dir);
            if (angle <= minAngle && DirectLineToTarget(target.transform.position))
            {
                nearest = target.transform;
                minAngle = angle;
            }
        }

        if (nearest)
        {
            OnThreatIsDetected.Invoke();
        }

        return nearest;
    }

    public override bool InAttackRange(Vector3 position)
    {
        Vector3 dir = (position - transform.position).normalized;
        float angle = Vector3.Angle(dir, transform.forward);
        return angle <= sightField.spotAngle / 2;
    }

    public override bool ThreatsDetected()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        Vector3 center = transform.position + transform.forward * sightField.range;

        if(Physics.Raycast(ray,out RaycastHit hit, sightField.range))
        {
            center = hit.point;
        }

        Collider[] possibleTargets = Physics.OverlapSphere(center, 20, detectionMask);

        foreach(Collider target in possibleTargets)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward,dir) <= sightField.spotAngle / 2 && DirectLineToTarget(target.transform.position))
            {
                return true;
            }
        }

        return false;
    }
}
