using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

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
        return anim.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash(PlayerAnimations.ANIMATOR_TAG_IS_INTERACTING);
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
        interacting = false;
        anim.SetBool("Interacting", interacting);
        anim.applyRootMotion = !possibleInteraction.disallowRootMotion;
        possibleInteraction.FinishInteraction();
    }

    private void StopCurrentInteraction ( )
    {
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
}
