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

    [SerializeField] int damage;
    [SerializeField] float harmDelay;
    [SerializeField] Vector3 damageArea;
    [SerializeField] Vector3 position;
    [SerializeField] LayerMask threatMask;
    [SerializeField] LayerMask threatMask2;
    [SerializeField] GameObject flashHit;

    private TimerClock timer;

    private void Start()
    {
        if (randomOffset) timeOffset = Random.Range(-1000, 1000);
        timer = new TimerClock(harmDelay, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Sin((Time.time + timeOffset) * speed) * amplitude;
        pendulum.localEulerAngles = Vector3.forward * angle;
        CheckThreats();
    }
    private void CheckThreats ( )
    {
        LayerMask combinedMasks = threatMask | threatMask2;
        Collider[] threats = Physics.OverlapBox(transform.position, damageArea, Quaternion.identity, threatMask);

        if (threats.Length > 0)
        {
            foreach (Collider threat in threats)
            {
                if (threat.TryGetComponent<iDamageable>(out iDamageable iDamageable))
                {
                    iDamageable.ApplyDamage(damage);
                }
            }
        }
    }
    private IEnumerator ApplyDamage ( iDamageable iDamageable )
    {
        yield return new WaitForSeconds(harmDelay);
        iDamageable.ApplyDamage(damage);
    }
    private void OnDrawGizmos ( )
    {
        Matrix4x4 gizmoMatrix = Matrix4x4.TRS(transform.position, transform.localRotation, transform.localScale);
        Gizmos.color = Color.green;
        Gizmos.matrix = gizmoMatrix;
        Gizmos.DrawWireCube(Vector3.zero + position, damageArea);
    }
}
