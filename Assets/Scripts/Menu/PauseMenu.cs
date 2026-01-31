using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup pauseMenu;

    public bool isPaused = false;
    public AudioSource buttonSoundSource;
    public AudioClip buttonClip;

    public AudioSource musicSource;
    public AudioClip pauseMenuClip;
    public AudioClip battleClip;

    private void Start()
    {
        pauseMenu.alpha = 0;
        pauseMenu.gameObject.SetActive(false);
        isPaused = false;
        musicSource.clip = battleClip;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Exit")) 
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.gameObject.SetActive(true);
                pauseMenu.DOFade(1, 0.3f);
                musicSource.clip = pauseMenuClip;
                musicSource.Play();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.gameObject.SetActive(false);
                pauseMenu.DOFade(0, 0.3f);
                musicSource.clip = battleClip;
                musicSource.Play();

            }
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Continue() 
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.gameObject.SetActive(false);
        pauseMenu.DOFade(0, 0.3f);
        musicSource.clip = battleClip;
        musicSource.Play();

    }
}
