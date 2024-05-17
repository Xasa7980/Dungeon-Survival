using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class PlayerDetectionArea : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] Color gizmoColor;
    BoxCollider detector;
#endif

    public bool playerInside { get; private set; }

    [SerializeField] UnityEvent onPlayerEnter = new UnityEvent();
    [SerializeField] UnityEvent onPlayerStay = new UnityEvent();
    [SerializeField] UnityEvent onPlayerExit = new UnityEvent();

    private void Update()
    {
        if(playerInside)
            onPlayerStay.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<iDamageable>() != null)
        {
            playerInside = true;

            onPlayerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<iDamageable>() != null)
        {
            playerInside = false;

            onPlayerExit.Invoke();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!detector)
            detector = GetComponent<BoxCollider>();

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(detector.center, detector.size);

        if (!detector.isTrigger)
            detector.isTrigger = true;
    }
#endif
}
