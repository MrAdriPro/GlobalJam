using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public RectTransform mainMenuSelector;
    public AudioClip selectSound;
    public AudioSource audioSource;

    public void PlayGame()
    {
        Debug.Log("Play Game!");
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenOptions()
    {
        Debug.Log("Open Options");
        // Cambiar al panel de opciones
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void ChangeMainMenuSelectorPosition(Vector2 newPosition) 
    {
        mainMenuSelector.DOAnchorPos(newPosition, 0.5f);
    }

}
