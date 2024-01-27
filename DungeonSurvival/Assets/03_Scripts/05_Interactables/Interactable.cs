using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    protected bool _canInteract = true;
    public bool canInteract => _canInteract;

    public enum InteractionObject
    {
        Item,
        ItemContainer,
        EnviromentItem,
        Npc
    }
    public enum InteractionType
    {
        Single,
        Continue
    }

    [PropertyOrder(100)]
    [Tooltip("Prioridad de la interaccion. Determina si sera mas deseable para el personaje que otra." +
        "A menor valor mayor prioridad")]
    [FoldoutGroup("Interactable"), SerializeField, Range(0, 100)] int _priority = 0;
    public int priority => _priority;

    [PropertyOrder(100),PropertySpace(SpaceBefore = 10)]
    [FoldoutGroup("Interactable"), SerializeField] InteractionObject _interactionObject;
    public InteractionObject interactionObject => _interactionObject;

    [PropertyOrder(100)]
    [FoldoutGroup("Interactable"), SerializeField] InteractionType _interactionType;
    public InteractionType interactionType => _interactionType;

    [PropertyOrder(100)]
    [FoldoutGroup("Interactable"), ShowIf("interactionType", InteractionType.Continue), SerializeField] float _interactionTime = 3; //Tiempo para realizar la interaccion
    public float interactionTime => _interactionTime; //Getter del tiempo para realizar la interaccion
    private float interactionProgress = 0; //Temporizador
    public bool finished => interactionProgress > interactionTime;

    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Animations"), SerializeField]
    bool _disallowRootMotion;
    public bool disallowRootMotion => _disallowRootMotion;
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Animations"), SerializeField, ShowIf("interactionType", InteractionType.Single)]
    AnimationClip interactionClip;
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Animations"), SerializeField, ShowIf("interactionType", InteractionType.Continue)]
    AnimationClip continuousInteractionClip;
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Animations"), SerializeField]
    bool hasStartAnimation;
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Animations"), SerializeField, ShowIf("hasStartAnimation")]
    AnimationClip interactionEnterClip;
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Animations"), SerializeField]
    bool hasExitAnimation;
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Animations"), SerializeField, ShowIf("hasExitAnimation")]
    AnimationClip interactionExitClip;

    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/HUD"), SerializeField]
    GameObject canvas;
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/HUD"), ShowIf("interactionType", InteractionType.Continue), SerializeField]
    Image interactionProgressBar;
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/HUD"), SerializeField]
    float appearSpeed = 1;
    Coroutine HUDCoroutine;

    float appearPercent;

    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Events"), SerializeField]
    UnityEvent OnInteractionStart = new UnityEvent();
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Events"), ShowIf("interactionType", InteractionType.Continue), SerializeField]
    UnityEvent OnInteractionUpdate = new UnityEvent();
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Events"), ShowIf("interactionType", InteractionType.Continue), SerializeField]
    UnityEvent OnInteractionStop = new UnityEvent();
    [PropertyOrder(100)]
    [FoldoutGroup("Interactable/Events"), SerializeField]
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
        _canInteract = false;
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
        if (HUDCoroutine != null)
            StopCoroutine(HUDCoroutine);

        HUDCoroutine = StartCoroutine(Appear());
    }

    public virtual void RemoveFocus()
    {
        if (HUDCoroutine != null)
            StopCoroutine(HUDCoroutine);

        HUDCoroutine = StartCoroutine(Hide());
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
