using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerDetectionArea))]
public class ArrowThrowerTrap : ActivableTrap
{
    [SerializeField] Transform shootPoint;
    [SerializeField] Projectile projectile;
    bool inUse;
    [SerializeField] float preparationTime = 1;
    [SerializeField] float fireInterval = 2;

    protected override IEnumerator _activate()
    {
        active = true;

        Projectile arrow = projectile.Create(shootPoint);
        arrow.transform.parent = shootPoint;

        arrow.transform.localPosition = -Vector3.forward;

        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime;

            arrow.transform.localPosition = Vector3.Lerp(-Vector3.forward * 0.5f, -Vector3.forward * 0.15f, percent);

            yield return null;
        }

        yield return new WaitForSeconds(preparationTime);

        arrow.transform.parent = null;
        arrow.Release();

        yield return new WaitForSeconds(fireInterval);

        active = false;
    }
}
