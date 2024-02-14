using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class UI_Animations : MonoBehaviour
{
    public bool enableAnimation = true;
    public float animationSpeed = 1f;
    public float startTime;
    public Vector3 originalPosition;
    [SerializeField] private Transform uiTarget;
    public Vector3 targetPosition;
    public float fadeDuration = 1f; // Duración del efecto fade in y fade out
    public AnimationCurve scaleCurve;
    public AnimationCurve alphaCurve;
    public float lifeTime = 5f; // Tiempo total de vida del objeto

    public bool enableLateralJump = false; // Controla si el salto incluirá un movimiento lateral
    public bool enableSpiralMovement = false; // Controla si el objeto realizará un movimiento en espiral
    public float lateralJumpIntensity = 2f; // Intensidad del salto lateral
    public float jumpHeight = 2f; // Altura máxima del salto
    public float spiralIntensity = 5f; // Intensidad del movimiento espiral
    public AnimationCurve shrinkCurve; // Curva para el efecto de encogimiento

    [SerializeField] private TextMeshProUGUI textMesh;
    private float currentLifeTime = 0f; // Contador de tiempo de vida actual

    public Vector2 freeMovement = new(0,0);
    private void OnEnable ( )
    {
        ResetValues();
        originalPosition = transform.position;
    }

    void Start ( )
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        InitFadeIn();
    }

    void Update ( )
    {
        transform.forward = Camera.main.transform.forward;
        if (!enableAnimation) return;

        currentLifeTime += Time.deltaTime;
        float timeSinceStart = Time.time - startTime;
        AnimatePosition(timeSinceStart);
        AnimateScaleAndShrink(timeSinceStart);
        AnimateFade(currentLifeTime);

        if (currentLifeTime >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTargetValue ( Vector3 target )
    {
        targetPosition = target;
    }
    void ResetValues ( )
    {
        startTime = Time.time;
        transform.position = originalPosition;
        currentLifeTime = 0f;
        InitFadeIn();
        enableAnimation = true;
    }

    void AnimatePosition ( float time )
    {
        Vector3 finalTarget = uiTarget != null ? uiTarget.position : targetPosition; // Determine el objetivo final

        float normalizedTime = time / lifeTime;
        float verticalMovement = jumpHeight * Mathf.Sin(Mathf.PI * normalizedTime);

        // Calcula la posición horizontal solo si el salto lateral está habilitado
        Vector3 spiralMovement = Vector3.zero;
        float horizontalMovement = 0f;
        if (enableLateralJump)
        {
            // Mapea el tiempo normalizado a la posición horizontal para hacer un arco
            horizontalMovement = lateralJumpIntensity * Mathf.Sin(Mathf.PI * normalizedTime);
        }

        float spiralMovementX = 0f;
        float spiralMovementY = 0f;
        if (enableSpiralMovement)
        {
            float spiralProgress = normalizedTime * 2 * Mathf.PI * spiralIntensity;
            spiralMovementX = Mathf.Sin(spiralProgress) * spiralIntensity;
            spiralMovementY = Mathf.Cos(spiralProgress) * spiralIntensity * (normalizedTime <= 0.5f ? 1 : -1);
        }

        Vector3 newPosition = originalPosition + new Vector3(horizontalMovement + spiralMovementX + freeMovement.x, freeMovement.y + verticalMovement + spiralMovementY, 0);
        transform.position = newPosition;
        if (normalizedTime >= 1f)
        {
            gameObject.SetActive(false);
        }
    }

    void AnimateScaleAndShrink ( float time )
    {
        // Usa la curva de encogimiento para ajustar la escala del objeto durante su tiempo de vida
        float scaleMultiplier = shrinkCurve.Evaluate(currentLifeTime / lifeTime);
        float scale = scaleCurve.Evaluate(time) * scaleMultiplier;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void AnimateFade ( float time )
    {
        float alpha = time < fadeDuration ? time / fadeDuration : time > (lifeTime - fadeDuration) ? (lifeTime - time) / fadeDuration : 1f;
        Color color = textMesh.color;
        color.a = alphaCurve.Evaluate(alpha);
        textMesh.color = color;
    }

    public void UpdateTarget ( Vector3 newTargetPosition, float newAnimationSpeed )
    {
        targetPosition = newTargetPosition;
        animationSpeed = newAnimationSpeed;
        ResetValues();
    }

    public void SetScaleCurve ( AnimationCurve newScaleCurve )
    {
        scaleCurve = newScaleCurve;
    }

    public void SetAlphaCurve ( AnimationCurve newAlphaCurve )
    {
        alphaCurve = newAlphaCurve;
    }

    public void SetJumpCurve ( AnimationCurve newJumpCurve )
    {
        // Este método se mantiene por compatibilidad, aunque no se use directamente en AnimatePosition
    }

    public void SetShrinkCurve ( AnimationCurve newShrinkCurve )
    {
        shrinkCurve = newShrinkCurve;
    }

    void InitFadeIn ( )
    {
        Color color = textMesh.color;
        color.a = 0f;
        textMesh.color = color;
    }
}