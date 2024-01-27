using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed = 10;
    [SerializeField] Transform checkPoint;
    [SerializeField] float checkTreshold = 0.2f;
    [SerializeField] LayerMask impactMask;
    [SerializeField] float lifeTime = 10;

    bool update = false;

    private void Start()
    {
        StartCoroutine(DelayDestroy());
    }

    private void Update()
    {
        if (!update) return;

        Ray ray = new Ray(checkPoint.position, checkPoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, speed * Time.deltaTime + checkTreshold, impactMask))
        {
            if (hit.collider.TryGetComponent(out iDamageable damageable))
            {
                damageable.ApplyDamage(damage);                
            }

            transform.position = hit.point - transform.forward * checkPoint.localPosition.z;

            StopAllCoroutines();
            Destroy(gameObject, 7);
            Destroy(this);
            return;
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public Projectile Create(Transform shootPoint, int additionalDamage = 0)
    {
        Projectile projectile = Instantiate(this, shootPoint.position, shootPoint.rotation);

        projectile.damage += additionalDamage;
        projectile.update = false;

        return projectile;
    }

    public void Release()
    {
        update = true;
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
}
