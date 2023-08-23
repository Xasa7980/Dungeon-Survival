using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SawTrap : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 300;
    [SerializeField] Transform saw;

    [Space(20)]

    [SerializeField] bool move;
    [SerializeField] bool randomOffset = true;
    [SerializeField, ShowIf("@!randomOffset && move")] float timeOffset = 0;
    [SerializeField, ShowIf("move")] float amplitude = 4;
    [SerializeField, ShowIf("move")] float speed = 2;

    private void Start()
    {
        if (randomOffset) timeOffset = Random.Range(-1000, 1000);
    }

    // Update is called once per frame
    void Update()
    {
        saw.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);

        if (move)
        {
            float move = Mathf.Cos((Time.time + timeOffset) * speed) * amplitude;
            saw.localPosition = Vector3.forward * move;
        }
    }

    private void OnDrawGizmos()
    {
        if (move)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + transform.forward * -amplitude + Vector3.up * 2, transform.position + transform.forward * amplitude + Vector3.up * 2);
        }
    }
}
