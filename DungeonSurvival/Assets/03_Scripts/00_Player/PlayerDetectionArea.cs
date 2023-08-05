using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetectionArea : MonoBehaviour
{
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
        if (other.GetComponent<PlayerInteraction>())
        {
            playerInside = true;

            onPlayerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInteraction>())
        {
            playerInside = false;

            onPlayerExit.Invoke();
        }
    }
}
