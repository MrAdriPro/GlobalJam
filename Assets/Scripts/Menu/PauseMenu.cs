using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup pauseMenu;

    public bool isPaused = false;

    private void Start()
    {
        isPaused = false;
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
                pauseMenu.DOFade(1, 0.3f);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.DOFade(0, 0.3f);
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
        pauseMenu.DOFade(0, 0.3f);
        
    }
}
