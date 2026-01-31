using DG.Tweening;
using System;
using System.Collections;
using TMPro;
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
        StartCoroutine(RestartTextAnimtion());
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
                GameObject.FindWithTag("Player1Hand").GetComponent<CanvasGroup>().DOFade(0.3f, 0.2f);
                try
                {
                    GameObject.FindWithTag("Player2Hand").GetComponent<CanvasGroup>().DOFade(0.3f, 0.2f);
                }
                catch (Exception ex) { }

            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.gameObject.SetActive(false);
                pauseMenu.DOFade(0, 0.3f);
                musicSource.clip = battleClip;
                musicSource.Play();
                GameObject.FindWithTag("Player1Hand").GetComponent<CanvasGroup>().DOFade(1f, 0.2f);

                try
                {
                    GameObject.FindWithTag("Player2Hand").GetComponent<CanvasGroup>().DOFade(1f, 0.2f);
                }
                catch (Exception ex) { }
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
        GameObject.FindWithTag("Player1Hand").GetComponent<CanvasGroup>().DOFade(1, 0.2f);

        try
        {
            GameObject.FindWithTag("Player2Hand").GetComponent<CanvasGroup>().DOFade(1, 0.2f);
        }
        catch (Exception ex) { }
    }

    IEnumerator RestartTextAnimtion()
    {
        CanvasGroup restartTextCanvas = GameObject.FindAnyObjectByType<PlayerSpawner>().levelCam.GetComponentInChildren<CanvasGroup>();
        restartTextCanvas.GetComponent<TextMeshProUGUI>().text = "Presiona cualquier tecla para reiniciar";

        while (GameObject.FindAnyObjectByType<PlayerSpawner>().levelCam.activeSelf)
        {
            yield return new WaitForSeconds(0.5f);
            restartTextCanvas.DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.5f);
            restartTextCanvas.DOFade(0, 0.5f);
        }
    }
}
