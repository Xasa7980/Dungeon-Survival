using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Automatic,
        Single,
        Continue
    }

    [Tooltip("Prioridad de la interaccion. Determina si sera mas deseable para el personaje que otra." +
        "A menor valor mayor prioridad")]
    [SerializeField, Range(0, 100)] int _priority = 0;
    public int priority => _priority;

    [SerializeField] InteractionType _interactionType;
    public InteractionType interactionType => _interactionType;
    [ShowIf("interactionType", InteractionType.Continue), SerializeField] float _interactionTime = 3;
    public float interactionTime => _interactionTime;
    float interactionProgress = 0;
    public bool finished => interactionProgress > interactionTime;

    [FoldoutGroup("Animations"), SerializeField, HideIf("interactionType", InteractionType.Automatic)]
    bool _disallowRootMotion;
    public bool disallowRootMotion => _disallowRootMotion;
    [FoldoutGroup("Animations"), SerializeField, ShowIf("interactionType", InteractionType.Single)]
    AnimationClip interactionClip;
    [FoldoutGroup("Animations"), SerializeField, ShowIf("interactionType", InteractionType.Continue)]
    AnimationClip continuousInteractionClip;
    [FoldoutGroup("Animations"), HideIf("interactionType", InteractionType.Automatic), SerializeField]
    bool hasStartAnimation;
    [FoldoutGroup("Animations"), SerializeField, ShowIf("@this.hasStartAnimation && interactionType != InteractionType.Automatic")]
    AnimationClip interactionEnterClip;
    [FoldoutGroup("Animations"), HideIf("interactionType", InteractionType.Automatic), SerializeField]
    bool hasExitAnimation;
    [FoldoutGroup("Animations"), SerializeField, ShowIf("@hasExitAnimation && interactionType != InteractionType.Automatic")]
    AnimationClip interactionExitClip;

    [FoldoutGroup("HUD"), SerializeField]
    GameObject canvas;
    [FoldoutGroup("HUD"), ShowIf("interactionType", InteractionType.Continue), SerializeField]
    Image interactionProgressBar;
    [FoldoutGroup("HUD"), SerializeField]
    float appearSpeed = 1;

    float appearPercent;

    [FoldoutGroup("Events"), SerializeField]
    UnityEvent OnInteractionStart = new UnityEvent();
    [FoldoutGroup("Events"), ShowIf("interactionType", InteractionType.Continue), SerializeField]
    UnityEvent OnInteractionUpdate = new UnityEvent();
    [FoldoutGroup("Events"), HideIf("interactionType", InteractionType.Automatic), SerializeField]
    UnityEvent OnInteractionStop = new UnityEvent();
    [FoldoutGroup("Events"), HideIf("interactionType", InteractionType.Automatic), SerializeField]
    UnityEvent OnInteractionFinish = new UnityEvent();

    protected virtual void Start()
    {
        canvas.SetActive(false);
    }

    /// <summary>
    /// Inici la interaccion y realiza cambios de primer momento al objeto interactuable
    /// </summary>
    /// <param name="target"></param>
    public virtual void StartInteraction()
    {
        OnInteractionStart.Invoke();

        if (interactionType != InteractionType.Continue)
        {
            FinishInteraction();
        }
    }

    /// <summary>
    /// Desarrolla y actualiza los componentes del objeto durante la interaccion
    /// </summary>
    /// <param name="target">Interactor</param>
    public virtual void Interact()
    {
        OnInteractionUpdate.Invoke();

        if (interactionProgress < interactionTime)
        {
            interactionProgress += Time.deltaTime;
            float percent = interactionProgress / interactionTime;
            interactionProgressBar.fillAmount = percent;
        }
    }

    /// <summary>
    /// Interrumpe la interaccion y reinicia el objeto a su estado original
    /// </summary>
    public virtual void StopInteraction()
    {
        OnInteractionStop.Invoke();

        interactionProgress = 0;
        if (interactionProgressBar)
            interactionProgressBar.fillAmount = 0;
    }

    /// <summary>
    /// Finaliza la interaccion de forma satisfactoria
    /// </summary>
    /// <param name="interactor"></param>
    public virtual void FinishInteraction()
    {
        OnInteractionFinish.Invoke();
        interactionProgress = 0;
        if (interactionProgressBar)
            interactionProgressBar.fillAmount = 0;
    }

    public void Prepare(PlayerInteraction player)
    {
        Animator anim = player.GetComponent<Animator>();
        AnimatorOverrideController controller = (AnimatorOverrideController)anim.runtimeAnimatorController;

        anim.SetBool("HasInteractionIntro", hasStartAnimation);
        anim.SetBool("HasInteractionExit", hasExitAnimation);

        controller["Player_Interaction Start_Animation"] = interactionEnterClip;
        controller["Player_Interaction End_Animation"] = interactionExitClip;

        if (_interactionType == InteractionType.Continue)
        {
            controller["Player_Interaction Continue_Animation"] = continuousInteractionClip;
        }

        if (_interactionType == InteractionType.Single)
        {
            controller["Player_Interaction_Animation"] = interactionClip;
        }
    }

    public virtual void SetFocus()
    {
        StopAllCoroutines();
        StartCoroutine(Appear());
    }

    public virtual void RemoveFocus()
    {
        StopAllCoroutines();
        StartCoroutine(Hide());
    }

    IEnumerator Appear()
    {
        canvas.SetActive(true);
        Image[] images = GetComponentsInChildren<Image>();

        while (appearPercent < 1)
        {
            appearPercent += Time.deltaTime * appearSpeed;

            foreach (Image image in images)
            {
                Color color = image.color;
                color.a = appearPercent;
                image.color = color;
            }

            yield return null;
        }

        foreach (Image image in images)
        {
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
    }

    IEnumerator Hide()
    {
        Image[] images = GetComponentsInChildren<Image>();

        while (appearPercent > 0)
        {
            appearPercent -= Time.deltaTime * appearSpeed;

            foreach (Image image in images)
            {
                Color color = image.color;
                color.a = appearPercent;
                image.color = color;
            }

            yield return null;
        }

        foreach (Image image in images)
        {
            Color color = image.color;
            color.a = 0;
            image.color = color;
        }

        canvas.SetActive(false);
    }
}
