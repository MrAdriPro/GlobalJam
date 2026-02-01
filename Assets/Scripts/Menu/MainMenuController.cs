using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public RectTransform mainMenuSelector;
    public AudioClip selectSound;
    public AudioSource audioSource;
    public float selectorVelocity = 0.25f;

    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject displayPanel;
    public GameObject audioPanel;
    public GameObject inputsPanel;

    public EventSystem eventSystem;
    private Menus currentMenu;
    private Vector2 startPos;

    [BoxGroup("Loading configs")]
    [SerializeField] private GameObject LoadingScreen;
    [BoxGroup("Loading configs")]
    [SerializeField] private GameObject LoadingIcon;
    [BoxGroup("Loading configs")]
    [SerializeField] private Vector3 RotateAmount;

    M_ToggleSwitch currentToggle = null;
    private void Start()
    {
        currentMenu = ChangeMenu(Menus.MainMenu);
        if (mainMenuSelector)
            startPos = mainMenuSelector.anchoredPosition;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (currentMenu == Menus.OptionsMenu) currentMenu = ChangeMenu(Menus.MainMenu);
            if (currentMenu == Menus.InputsMenu) currentMenu = ChangeMenu(Menus.OptionsMenu);
            if (currentMenu == Menus.DisplayMenu) currentMenu = ChangeMenu(Menus.OptionsMenu);
            if (currentMenu == Menus.AudioMenu) currentMenu = ChangeMenu(Menus.OptionsMenu);
        }
        else if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Joystick1_Submit") || Input.GetButtonDown("Joystick2_Submit")) 
        {
            if (currentToggle) 
            {
                currentToggle.Toggle();
            }
        }
    }

    public void PlayGame()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    public void OpenOptions() => currentMenu = ChangeMenu(Menus.OptionsMenu);
    
    public void OpenDisplayMenu() => currentMenu = ChangeMenu(Menus.DisplayMenu);

    public void OpenAudioMenu() => currentMenu = ChangeMenu(Menus.AudioMenu);

    public void OpenInputsMenu() => currentMenu = ChangeMenu(Menus.InputsMenu);

    public void OpenMainMenu() => currentMenu = ChangeMenu(Menus.MainMenu);

    public void QuitGame()
    {
        Debug.Log("Quit Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private Menus ChangeMenu(Menus menu)
    {
        try
        {
            // Oculta todos
            CanvasGroup[] allGroups = new[] {
        mainMenuPanel.GetComponent<CanvasGroup>(),
        optionsPanel.GetComponent<CanvasGroup>(),
        displayPanel.GetComponent<CanvasGroup>(),
        audioPanel.GetComponent<CanvasGroup>(),
        inputsPanel.GetComponent<CanvasGroup>()
    };

            foreach (var group in allGroups)
            {
                group.alpha = 0f;
                group.interactable = false;
                group.blocksRaycasts = false;
            }

            // Muestra el activo
            CanvasGroup activeGroup;
            Button firstButton = null;
            Slider firstSlider = null;
            switch (menu)
            {
                case Menus.MainMenu:
                    activeGroup = mainMenuPanel.GetComponent<CanvasGroup>();
                    firstButton = mainMenuPanel.GetComponentsInChildren<Button>()[0];
                    break;
                case Menus.OptionsMenu:
                    activeGroup = optionsPanel.GetComponent<CanvasGroup>();
                    firstButton = optionsPanel.GetComponentsInChildren<Button>()[0];
                    break;
                case Menus.DisplayMenu:
                    activeGroup = displayPanel.GetComponent<CanvasGroup>();
                    firstSlider = displayPanel.GetComponentsInChildren<Slider>()[0];
                    break;
                case Menus.AudioMenu:
                    activeGroup = audioPanel.GetComponent<CanvasGroup>();
                    firstSlider = audioPanel.GetComponentsInChildren<Slider>()[0];
                    break;
                case Menus.InputsMenu:
                    activeGroup = inputsPanel.GetComponent<CanvasGroup>();
                    firstButton = inputsPanel.GetComponentsInChildren<Button>()[0];
                    break;
                default: return menu;
            }

            activeGroup.alpha = 1f;
            activeGroup.interactable = true;
            activeGroup.blocksRaycasts = true;

            if (firstButton)
                eventSystem.SetSelectedGameObject(firstButton.gameObject);
            else if (firstSlider)
            {
                eventSystem.SetSelectedGameObject(firstSlider.gameObject);
            }

            return menu;
        }
        catch (Exception ex) { return Menus.MainMenu; }
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        LoadingScreen.SetActive(true);
        int i = 0;
        while (i < 400)
        {
            i++;
            yield return new WaitForSeconds(0.001f);
            LoadingIcon.transform.Rotate(RotateAmount * Time.deltaTime);

        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);


        while (!operation.isDone)
        {
            LoadingIcon.transform.Rotate(RotateAmount * Time.deltaTime);

            yield return null;
        }

    }

    public void SetCurrentToggle(M_ToggleSwitch toggle) => currentToggle = toggle;

    public void ChangeMainMenuSelectorPosition(Vector2 newPosition) 
    {
        if(mainMenuSelector)
        mainMenuSelector.DOAnchorPos(newPosition, selectorVelocity);
    }

    public enum Menus 
    {
        MainMenu,
        OptionsMenu,
        DisplayMenu,
        AudioMenu,
        InputsMenu,
    }

}
