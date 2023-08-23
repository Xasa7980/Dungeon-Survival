using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivableTrap : MonoBehaviour
{
    [SerializeField] float activationDelay = 0;
    protected bool active;

    public virtual void Activate()
    {
        if (active) return;

        StartCoroutine(_activate());
        active = true;
    }

    protected virtual IEnumerator _activate()
    {
        yield return new WaitForSeconds(activationDelay);
    }
}
