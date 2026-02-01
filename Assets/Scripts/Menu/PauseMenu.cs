using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup pauseMenu;

    public bool isPaused = false;
    public AudioSource buttonSoundSource;
    public AudioClip buttonClip;

    public AudioSource musicSource;
    public AudioClip pauseMenuClip;
    public AudioClip battleClip;

    public EventSystem eventSystem;

    public CanvasGroup optionsCanvas;
    private M_SettingsMenu m_SettingsMenu;
    private void Start()
    {
        pauseMenu.alpha = 0;
        pauseMenu.gameObject.SetActive(false);
        m_SettingsMenu = GetComponent<M_SettingsMenu>();
        m_SettingsMenu.AudioSettingsPanel.SetActive(false);
        isPaused = false;
        musicSource.clip = battleClip;
        StartCoroutine(RestartTextAnimtion());
        Back();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && optionsCanvas.alpha == 1) 
        {
            Back();
        }

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
                GameObject p = pauseMenu.GetComponentInChildren<Button>().gameObject;
                eventSystem.SetSelectedGameObject(p);
                p.GetComponent<M_ButtonHandler>().SetSelectManual();
                GameObject.FindWithTag("Player1Hand").GetComponent<CanvasGroup>().DOFade(0.3f, 0.2f);
                try
                {
                    GameObject.FindWithTag("Player2Hand").GetComponent<CanvasGroup>().DOFade(0.3f, 0.2f);
                }
                catch (Exception ex) { }

            }
            else
            {
                Back();
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

    public void Options()
    {
        pauseMenu.DOFade(0, 0.3f);
        m_SettingsMenu.AudioSettingsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(optionsCanvas.GetComponentInChildren<Slider>().gameObject);
        optionsCanvas.DOFade(1, 0.3f);
    }


    public void Back()
    {
        pauseMenu.DOFade(1, 0.3f);
        eventSystem.SetSelectedGameObject(pauseMenu.GetComponentInChildren<Button>().gameObject);
        optionsCanvas.DOFade(0, 0.3f);
        m_SettingsMenu.AudioSettingsPanel.SetActive(false);

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
        restartTextCanvas.GetComponent<TextMeshProUGUI>().text = "Espera a que el otro jugador elija opcion";

        while (GameObject.FindAnyObjectByType<PlayerSpawner>().levelCam)
        {
            yield return new WaitForSeconds(0.5f);
            restartTextCanvas.DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.5f);
            restartTextCanvas.DOFade(0, 0.5f);
        }
    }

    public IEnumerator RestartTextAnimtion2()
    {
        CanvasGroup restartTextCanvas = GameObject.FindAnyObjectByType<PlayerSpawner>().levelCam2.GetComponentInChildren<CanvasGroup>();
        restartTextCanvas.GetComponent<TextMeshProUGUI>().text = "Espera a que el otro jugador elija opcion";

        while (GameObject.FindAnyObjectByType<PlayerSpawner>().levelCam2)
        {
            yield return new WaitForSeconds(0.5f);
            restartTextCanvas.DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.5f);
            restartTextCanvas.DOFade(0, 0.5f);
        }
    }
}
