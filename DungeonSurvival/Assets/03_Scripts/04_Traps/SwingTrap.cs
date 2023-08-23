using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SwingTrap : MonoBehaviour
{
    [SerializeField] bool randomOffset = true;
    [SerializeField, HideIf("randomOffset")] float timeOffset = 0;

    [SerializeField] Transform pendulum;
    [SerializeField] float amplitude = 45;
    [SerializeField] float speed = 2;

    private void Start()
    {
        if (randomOffset) timeOffset = Random.Range(-1000, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Sin((Time.time + timeOffset) * speed) * amplitude;
        pendulum.localEulerAngles = Vector3.forward * angle;
    }
}
