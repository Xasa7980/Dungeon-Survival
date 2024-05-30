using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;

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

    [SerializeField] int damage;
    [SerializeField] float harmDelay;
    [SerializeField] Vector3 damageArea;
    [SerializeField] LayerMask threatMask;
    [SerializeField] LayerMask threatMask2;

    [SerializeField] GameObject flashHit;

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
        CheckThreats();
    }
    private void CheckThreats()
    {
        LayerMask combinedMasks = threatMask | threatMask2;
        Collider[] threats = Physics.OverlapBox(transform.position, damageArea, Quaternion.identity, threatMask);

        if(threats.Length > 0)
        {
            foreach(Collider threat in threats)
            {
                if(threat.TryGetComponent<iDamageable>(out iDamageable iDamageable))
                {
                    iDamageable.ApplyDamage(damage);
                }
            }
        }
    }
    private IEnumerator ApplyDamage(iDamageable iDamageable )
    {
        yield return new WaitForSeconds(harmDelay);
        iDamageable.ApplyDamage(damage);
    }
    private void OnDrawGizmos()
    {
        if (move)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + transform.forward * -amplitude + Vector3.up * 2, transform.position + transform.forward * amplitude + Vector3.up * 2);
        }
        Matrix4x4 gizmoMatrix = Matrix4x4.TRS(transform.position, transform.localRotation, transform.localScale);
        Gizmos.color = Color.green;
        Gizmos.matrix = gizmoMatrix;
        Gizmos.DrawWireCube(Vector3.zero, damageArea);
    }
}
