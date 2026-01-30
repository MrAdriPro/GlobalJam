using DG.Tweening;
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

    [Header("Referencias")]
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private Transform cameraTransform;

    private Vector3 originalPosition;
    private float timer = 0f;
    private float currentBobIntensity = 0f;
    private Sequence bobSequence;

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
        bool isMoving = pm.isMoving() &&
                         pm.isGrounded;

        // Suavizar la intensidad del bob
        float targetIntensity = isMoving ? 1f : 0f;
        currentBobIntensity = Mathf.Lerp(
            currentBobIntensity,
            targetIntensity,
            Time.deltaTime * transitionSpeed
        );

        if (currentBobIntensity > 0.01f)
        {
            timer += Time.deltaTime * (bobFrequency + pm.speed);
            ApplyBobbing();
        }
        else
        {
            // Volver suavemente a la posición original
            cameraTransform.DOLocalMove(
                originalPosition,
                smoothTime
            ).SetEase(Ease.OutQuad);
        }
    }

    private void ApplyBobbing()
    {
        float verticalOffset = Mathf.Sin(timer) * bobAmplitude * currentBobIntensity;
        float horizontalOffset = Mathf.Cos(timer * 0.5f) *
                                 bobHorizontalAmount * currentBobIntensity;

        Vector3 targetPosition = originalPosition +
            new Vector3(horizontalOffset, verticalOffset, 0f);

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetPosition,
            Time.deltaTime / smoothTime
        );
    }

    private void OnDestroy()
    {
        bobSequence?.Kill();
        cameraTransform?.DOKill();
    }
}
