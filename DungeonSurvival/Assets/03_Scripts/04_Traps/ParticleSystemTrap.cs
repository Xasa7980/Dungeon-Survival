using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleSystemTrap : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particles;
    public bool isOn { get; private set; }

    [SerializeField] bool useTimer = true;
    [SerializeField, ShowIf("useTimer")] float onnTime;
    [SerializeField, ShowIf("useTimer")] float offTime;

    private IEnumerator Start()
    {
        if (useTimer)
        {
            while (true)
            {
                SetActive(true);

                yield return new WaitForSeconds(onnTime);

                SetActive(false);

                yield return new WaitForSeconds(offTime);
            }
        }
    }

    public void SetActive(bool active)
    {
        foreach (ParticleSystem p in particles)
        {
            if (active)
                p.Play();
            else
                p.Stop();
        }
    }

    public void Activate()
    {
        StopAllCoroutines();
        SetActive(false);
        StartCoroutine(Start());
    }

    public void Deactivate()
    {
        StopAllCoroutines();
        SetActive(false);
    }
}
