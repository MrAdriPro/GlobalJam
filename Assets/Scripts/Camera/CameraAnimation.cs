using DG.Tweening;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    public Transform[] cameraPoints;  // Array de puntos donde irá la cámara
    public float moveDuration = 3f;   // Duración del movimiento entre puntos
    public float waitTime = 1f;       // Tiempo de pausa en cada punto
    public Ease easeType = Ease.InOutSine; // Tipo de suavizado

    void Start()
    {
        if (cameraPoints.Length > 0)
        {
            AnimateCameraLoop();
        }
        else
        {
            Debug.LogWarning("¡No hay puntos de cámara asignados!");
        }
    }

    void AnimateCameraLoop()
    {
        Sequence cameraSequence = DOTween.Sequence();

        foreach (Transform point in cameraPoints)
        {
            // Mover a la posición del punto
            cameraSequence.Append(transform.DOMove(point.position, moveDuration).SetEase(easeType));

            // Rotar a la rotación del punto (al mismo tiempo que se mueve)
            cameraSequence.Join(transform.DORotateQuaternion(point.rotation, moveDuration).SetEase(easeType));

            // Pausa en el punto
            cameraSequence.AppendInterval(waitTime);
        }

        // Loop infinito
        cameraSequence.SetLoops(-1, LoopType.Restart);
    }
}
