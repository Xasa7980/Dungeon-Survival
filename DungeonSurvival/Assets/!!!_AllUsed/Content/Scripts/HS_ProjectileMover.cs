using Sirenix.OdinInspector;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public class HS_ProjectileMover : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float hitOffset = 0f;
    [SerializeField] private bool isDestroyable = false;
    [SerializeField] private bool useFirePointRotation;
    [SerializeField, ShowIf("@useFirePointRotation")] private Vector3 rotationOffset = Vector3.zero;
    [SerializeField] private int damage;
    [SerializeField] private float checkThreshold = 0.2f;
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private LayerMask impactMask;

    private Collider projectileCollider;
    private Collider parentCollider;
    private bool isRecharged = false;

    [Header("MainEffect")]
    [SerializeField] private ParticleSystem projectilePS;
    [SerializeField] private ParticleSystem[] childParticles => transform.GetComponentsInChildren<ParticleSystem>();

    [Header("SubEffects")]
    
    [SerializeField] private ParticleSystem hitPS;
    [SerializeField] private GameObject flashEffect;
    [SerializeField] private Light lightSource;

    private void Start ( )
    {
        projectileCollider = GetComponent<Collider>();
        parentCollider = transform.parent?.GetComponentInParent<Collider>();

        // Ignorar colisiones entre el proyectil y el padre
        if (parentCollider != null && projectileCollider != null)
        {
            Physics.IgnoreCollision(projectileCollider, parentCollider);
            Physics.IgnoreCollision(projectileCollider, parentCollider.transform.GetChild(0)?.GetComponent<Collider>());
        }

        // Desacoplar el efecto de destello si está presente
        if (flashEffect != null)
        {
            flashEffect.transform.parent = null;
        }

        // Iniciar temporizador para la desactivación o autodestrucción
        if (!isDestroyable)
        {
            StartCoroutine(DisableTimer(5));
        }
        else
        {
            Destroy(gameObject, 5);
        }
    }

    // Desactivar el proyectil después de un tiempo si no ha colisionado con nada
    private IEnumerator DisableTimer ( float time )
    {
        yield return new WaitForSeconds(time);
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable ( )
    {
        isRecharged = false;

        if (flashEffect != null)
        {
            flashEffect.transform.parent = null;
        }

        if (lightSource != null)
        {
            lightSource.enabled = true;
        }

        projectileCollider.enabled = true;

        // Iniciar el sistema de partículas del proyectil
        projectilePS.Play();
        StartCoroutine(DisableTimer(3));
    }

    private void FixedUpdate ( )
    {
        if (!isRecharged) return;

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter ( Collision collision )
    {
        // Evitar colisiones con objetos con el layer default, obstaculos o IgnoreChildrenCollider
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 0 || collision.gameObject.layer == 8) return;

        // Apagar la luz y desactivar el collider del proyectil
        if (lightSource != null) lightSource.enabled = false;

        projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // Obtener información sobre el contacto
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;
        if (collision.gameObject.TryGetComponent(out iDamageable damageable))
        {
            damageable.ApplyDamage(damage);
        }

        // Generar el efecto de impacto
        if (hitPS != null)
        {
            hitPS.transform.SetPositionAndRotation(pos, rot);

            if (useFirePointRotation)
            {
                hitPS.transform.rotation = transform.rotation * Quaternion.Euler(0, 180f, 0);
            }
            else if (rotationOffset != Vector3.zero)
            {
                hitPS.transform.rotation = Quaternion.Euler(rotationOffset);
            }
            else
            {
                hitPS.transform.LookAt(contact.point + contact.normal);
            }
            hitPS?.Play();
        }

        // Detener las partículas secundarias
        //foreach (var childParticle in childParticles)
        //{
        //    childParticle?.Stop();
        //}

        // Destruir o desactivar el proyectil según si es destruible
        if (!isDestroyable)
        {
            if (hitPS != null) StartCoroutine(DisableTimer(hitPS.main.duration));
            else StartCoroutine(DisableTimer(1));
        }
        else
        {
            if (hitPS != null) Destroy(gameObject, hitPS.main.duration);
            else Destroy(gameObject, 1);
        }
    }

    // Configurar propiedades adicionales para el proyectil
    public HS_ProjectileMover SetProperties ( Transform shootPoint, int additionalDamage = 0 )
    {
        damage += additionalDamage;
        isRecharged = false;
        return this;
    }

    // Método para lanzar la flecha/proyectil
    public void ReleaseArrow ( )
    {
        isRecharged = true;
        projectilePS.Play();
    }
}