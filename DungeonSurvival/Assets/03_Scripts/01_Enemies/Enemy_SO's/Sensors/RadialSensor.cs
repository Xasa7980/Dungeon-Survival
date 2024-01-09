﻿using System;
using System.Collections;
using UnityEngine;

public class RadialSensor : Sensor
{
    [SerializeField, Range(0, 360)] float detectionAngle = 60;
    public float DetectionRadius => detectionRadius;
    [SerializeField] private float rotationSpeed = 5f;

    private AI_MainCore ai_MainCore;
    private void Awake ( )
    {
        ai_MainCore = GetComponent<AI_MainCore>();
    }

    private void Update ( )
    {
        if (ThreatsDetected())
        {
            Vector3 directionToThreat = ai_MainCore.GetThreat().position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(directionToThreat);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            ai_MainCore.SetState(State.Hostile);
            OnThreatIsInAttackRange.Invoke();
            OnThreatIsDetected?.Invoke();
            print("atkrange");
        }
        else
        {

            if (ThreatsOutOfRangeDetected())
            {
                Vector3 directionToThreat = ai_MainCore.GetThreat().position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(directionToThreat);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                ai_MainCore.SetState(State.Chasing);
                OnThreatIsDetected?.Invoke();
                print("outOfRange");
            }
        }
    }
    public override bool ThreatsDetected ( )
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

        foreach (Collider threat in threats)
        {
            Vector3 threatDir = (threat.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(threatDir, transform.forward);
            if (angle <= detectionAngle / 2)
            {
                if (DirectLineToTarget(threat.transform.position))
                {
                    ai_MainCore.SetThreat(threat.transform);
                    return true;
                }
            }
        }
        return false;
    }

    public bool ThreatsOutOfRangeDetected()
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, detectionRadius * 2, detectionMask);

        foreach (Collider threat in threats)
        {
            Vector3 threatDir = (threat.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(threatDir, transform.forward);

            if (angle <= 360)
            {
                if (DirectLineToTarget(threat.transform.position))
                {
                    ai_MainCore.SetThreat(threat.transform);
                    return true;
                }
            }
        }
        return false;
    }

    public override Transform GetNearestThreat()
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, detectionRadius, detectionMask);

        float minDst = float.MaxValue;
        Transform nearest = null;

        foreach (Collider threat in threats)
        {
            Vector3 threatDir = (threat.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(threatDir, transform.forward);
            if (angle <= detectionAngle / 2)
            {
                float sqrDst = (threat.transform.position - transform.position).sqrMagnitude;
                if(sqrDst < minDst && DirectLineToTarget(threat.transform.position))
                {
                    minDst = sqrDst;
                    nearest = threat.transform;
                }
            }
        }

        return nearest;
    }
    public Transform GetOutOfRangeThreat()
    {
        if(GetNearestThreat() != null)
        {
            return null;
        }

        Collider[] threats = Physics.OverlapSphere(transform.position, detectionRadius * 2, detectionMask);

        float minDst = float.MaxValue;
        Transform nearest = null;

        foreach (Collider threat in threats)
        {
            Vector3 threatDir = (threat.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(threatDir, transform.forward);
            if (angle >= detectionAngle / 2)
            {
                float sqrDst = (threat.transform.position - transform.position).sqrMagnitude;
                if (sqrDst < minDst && DirectLineToTarget(threat.transform.position))
                {
                    minDst = sqrDst;
                    nearest = threat.transform;
                }
            }
        }

        return nearest;
    }

    public override bool InAttackRange(Vector3 targetPosition)
    {
        float sqrDst = (targetPosition - transform.position).sqrMagnitude;
        return sqrDst <= Mathf.Pow(detectionRadius, 2);
    }
    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, detectionRadius);
        UnityEditor.Handles.DrawLine(transform.position, transform.position + MathOps.PlaneDirFromAngle((-detectionAngle / 2 + transform.rotation.eulerAngles.y)) * detectionRadius);
        UnityEditor.Handles.DrawLine(transform.position, transform.position + MathOps.PlaneDirFromAngle((detectionAngle / 2 + transform.rotation.eulerAngles.y)) * detectionRadius);
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, detectionRadius * 2);
#endif
    }
}