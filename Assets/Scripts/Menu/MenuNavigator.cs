using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuNavigator : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float navigationDelay = 0.2f; // Delay entre inputs
    [SerializeField] private bool useKeyboard = true;
    [SerializeField] private bool useGamepad = true;

    [Header("Audio (Opcional)")]
    [SerializeField] private AudioClip navigationSound;
    [SerializeField] private AudioClip selectSound;
    private AudioSource audioSource;

    private float lastNavigationTime;
    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = EventSystem.current;
        audioSource = GetComponent<AudioSource>();

        // Seleccionar el primer botón automáticamente
        if (eventSystem.firstSelectedGameObject != null)
        {
            eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
        }
    }


    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
