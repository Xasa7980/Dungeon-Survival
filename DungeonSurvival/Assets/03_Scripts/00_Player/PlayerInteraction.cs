using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction current { get; private set; }

    public event EventHandler OnInteractAnyObject;

    Interactable _possibleInteraction;
    public Interactable possibleInteraction
    {
        get => _possibleInteraction;
        private set
        {
            if (_possibleInteraction == value) return;

            if (_possibleInteraction != null)
                _possibleInteraction.RemoveFocus();

            _possibleInteraction = value;

            if (_possibleInteraction != null)
                _possibleInteraction.SetFocus();
        }
    }

    [SerializeField] AreaDrawer interactionArea;
    [SerializeField] LayerMask interactionMask;

    Animator anim;

    bool interacting;
    private void Awake ( )
    {
        current = this;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ( )
    {

        HandleEscapeKey();
        if (IsInteracting()) return;
        if (PlayerComponents.instance.lockedAll) return;

        UpdateSurroundings();

        if (possibleInteraction != null)
        {
            CheckInteractionValidity();
            HandleInteraction();
        }
        else
        {
            ResetInteraction();
        }
    }
    private bool IsInteracting ( )
    {
        int hashInteracting = Animator.StringToHash("Base Layer.InteraccionItems.Interaction");
        return anim.GetCurrentAnimatorStateInfo(0).fullPathHash == hashInteracting;
    }
    private void HandleEscapeKey ( )
    {
        if (Input.GetKeyDown(KeyCode.Escape) && interacting && possibleInteraction != null)
        {
            StopCurrentInteraction();
        }
    }

    private void CheckInteractionValidity ( )
    {
        if (!possibleInteraction.gameObject.activeInHierarchy || possibleInteraction.interactionHits <= 0)
        {
            StopCurrentInteraction();
        }
    }

    private void HandleInteraction ( )
    {
        OnInteractAnyObject?.Invoke(this, EventArgs.Empty);

        if (Input.GetButtonDown("Interact"))
        {
            StartInteraction();
        }

        if (possibleInteraction.interactionType == Interactable.InteractionType.Continue)
        {
            if (interacting && Input.GetButtonUp("Interact"))
            {
                StopCurrentInteraction();
            }

            if (interacting)
            {
                ContinueInteraction();
            }
        }
    }
    private void StartInteraction ( )
    {
        possibleInteraction.Prepare(this);
        possibleInteraction.StartInteraction();
        StartCoroutine(PlayerLookAtItem(possibleInteraction.transform.position));

        interacting = true;
        anim.SetTrigger("Interact");
        anim.SetBool("ContinousInteraction", possibleInteraction.interactionType == Interactable.InteractionType.Continue);
        anim.SetBool("Interacting", interacting);
        anim.applyRootMotion = !possibleInteraction.disallowRootMotion;
    }

    private void ContinueInteraction ( )
    {
        possibleInteraction.Interact();

        if (possibleInteraction.finished)
        {
            FinishInteraction();
        }
    }

    private void FinishInteraction ( )
    {
        StopCoroutine(PlayerLookAtItem(possibleInteraction.transform.position));
        possibleInteraction.FinishInteraction();

        interacting = false;
        anim.SetBool("Interacting", interacting);
        anim.applyRootMotion = !possibleInteraction.disallowRootMotion;
    }

    private void StopCurrentInteraction ( )
    {
        StopCoroutine(PlayerLookAtItem(possibleInteraction.transform.position));
        possibleInteraction.StopInteraction();
        ResetInteraction();
    }

    private void ResetInteraction ( )
    {
        interacting = false;
        anim.SetBool("Interacting", interacting);
        anim.applyRootMotion = possibleInteraction == null || !possibleInteraction.disallowRootMotion;
    }
    void UpdateSurroundings()
    {
        if (interacting) return;

        Collider[] interactions = Physics.OverlapBox(interactionArea.objectPosition, interactionArea.size,
            interactionArea.rotation, interactionMask);

        //Si hay mas de una interacción habra que decidir cual es la mas adecuada
        if (interactions.Length > 1)
        {
            Interactable nearest = null;
            float minDst = float.MaxValue;

            foreach (Collider c in interactions)
            {
                if (c.TryGetComponent(out Interactable target))
                {
                    if (!target.canInteract) continue;

                    //Si no hay ninguna interaccion asignada se asigna una
                    if (nearest == null)
                    {
                        nearest = target;
                        continue;
                    }

                    //Si la nueva interaccion tiene menor prioridad que la actual se ignora
                    if (nearest.priority < target.priority) continue;

                    //Si la nueva interaccion tiene mas prioridad que la actual se asigna automaticamente
                    if (nearest.priority > target.priority)
                    {
                        nearest = target;
                        continue;
                    }

                    //Si tienen la misma prioridad se selecciona la mas cercana
                    float sqrDst = (transform.position - c.transform.position).sqrMagnitude;
                    if (sqrDst < minDst)
                    {
                        nearest = target;
                        minDst = sqrDst;
                    }
                }
            }

            possibleInteraction = nearest;
        }
        //Si existe solo un objeto interactuable se asigna automaticamente
        else if (interactions.Length == 1 && interactions[0].TryGetComponent(out Interactable target))
        {
            if (target.canInteract)
                possibleInteraction = target;
            else
                possibleInteraction = null;
        }
        else
        {
            possibleInteraction = null;
        }
    }
    IEnumerator PlayerLookAtItem ( Vector3 target )
    {
        Vector3 directionToTarget = target - transform.position;
        directionToTarget.y = 0;  // Opcional, mantiene la rotación solo en el plano horizontal

        // Obtiene la rotación deseada hacia el target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Continúa la rotación mientras la diferencia de ángulo sea significativa
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            // Interpola la rotación actual hacia la rotación objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 8 * Time.deltaTime);

            // Espera hasta el próximo frame antes de continuar el bucle
            yield return null;
        }

        // Opcional: asegura que la rotación sea exactamente la deseada al finalizar
        transform.rotation = targetRotation;
    }
}

