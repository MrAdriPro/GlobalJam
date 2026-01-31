using DG.Tweening;
using System;
using UnityEngine;

public class FirstPersonCameraBob : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float bobFrequency = 2.0f;
    [SerializeField] private float bobAmplitude = 0.05f;
    [SerializeField] private float bobHorizontalAmount = 0.03f;

    [Header("Velocidad de Animación")]
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private float transitionSpeed = 5f;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDefaultIntensity = 0.1f;
    [SerializeField] private float shakeDefaultDuration = 0.3f;

    [Header("Referencias")]
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private Transform cameraTransform;

    public event Action OnFootstep;

    private Vector3 originalPosition;
    private float timer = 0f;
    private float currentBobIntensity = 0f;
    private Sequence bobSequence;
    private Sequence shakeSequence;

    private float previousVerticalOffset = 0f;
    private bool hasTriggeredStep = false;
    private bool isWallRunning = false;
    private void Awake()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        if (pm == null)
            pm = GetComponent<PlayerMovement>();

        originalPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (!GetComponentInChildren<PlayerInputSelector>().selectedInput || GameObject.FindAnyObjectByType<PauseMenu>().isPaused) return;

        bool isMoving = false;

        if (pm.isMoving())
        {
            if (pm.isGrounded || pm.wallRunning || pm.climbing)
            {
                isMoving = true;
                isWallRunning = pm.wallRunning || pm.climbing;
            }
        }        

        // Suavizar la intensidad del bob
        float targetIntensity = isMoving ? 1f : 0f;
        targetIntensity = isWallRunning ? targetIntensity * 2 : targetIntensity;
        currentBobIntensity = Mathf.Lerp(
            currentBobIntensity,
            targetIntensity ,
            Time.deltaTime * transitionSpeed
        );

        if (currentBobIntensity > 0.01f)
        {
            timer += Time.deltaTime * (bobFrequency + pm.speed);
            ApplyBobbing();
        }
        else
        {
            cameraTransform.DOLocalMove(
                originalPosition,
                smoothTime
            ).SetEase(Ease.OutQuad);

            hasTriggeredStep = false;
        }
    }

    private void ApplyBobbing()
    {
        float verticalOffset = Mathf.Sin(timer) * bobAmplitude * currentBobIntensity;
        float horizontalOffset = Mathf.Cos(timer * 0.5f) *
                                 bobHorizontalAmount * currentBobIntensity;

        if (previousVerticalOffset > 0f && verticalOffset <= 0f && !hasTriggeredStep)
        {
            if(!pm.sliding)
            OnFootstep?.Invoke(); 

            hasTriggeredStep = true;
        }
        else if (verticalOffset > 0f)
        {
            hasTriggeredStep = false;
        }

        previousVerticalOffset = verticalOffset;

        Vector3 targetPosition = originalPosition +
            new Vector3(horizontalOffset, verticalOffset, 0f);

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetPosition,
            Time.deltaTime / smoothTime
        );
    }

    /// <summary>
    /// Dispara un camera shake desde cualquier script
    /// </summary>
    /// <param name="intensity">Fuerza del shake (0.05f suave, 0.3f fuerte)</param>
    /// <param name="duration">Duración en segundos</param>
    public void ShakeCamera(float intensity = 0f, float duration = 0f)
    {
        // Usa valores por defecto si no se especifican
        intensity = intensity <= 0 ? shakeDefaultIntensity : intensity;
        duration = duration <= 0 ? shakeDefaultDuration : duration;

        // Mata shake anterior
        shakeSequence?.Kill();

        shakeSequence = DOTween.Sequence();

        shakeSequence.Append(
            cameraTransform.DOShakePosition(duration, intensity)
                .SetRelative()
                .SetEase(Ease.InOutSine)
        );

        shakeSequence.Append(
            cameraTransform.DOLocalMove(originalPosition, 0.2f)
                .SetEase(Ease.OutElastic)
        );
    }

    /// <summary>
    /// Shake rápido para disparos/impactos (preset)
    /// </summary>
    public void QuickShake() => ShakeCamera(0.08f, 0.15f);

    /// <summary>
    /// Shake fuerte para explosiones
    /// </summary>
    public void ExplosionShake() => ShakeCamera(0.25f, 0.5f);

    private void OnDestroy()
    {
        bobSequence?.Kill();
        shakeSequence?.Kill();
        cameraTransform?.DOKill();
    }
}
