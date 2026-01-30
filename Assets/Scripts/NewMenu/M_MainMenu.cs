using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class M_MainMenu : MonoBehaviour
{
    //Variables
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _mainMenu;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _settingsMenu;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _displaySettings;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _audioSettings;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _inputSettings;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _fileSelectorMenu;
    [BoxGroup("Menus")]
    public Dictionary<int, GameObject> _menus = new Dictionary<int, GameObject>();
    

    [BoxGroup("Button Selector Config")]
    [SerializeField] private Vector2 selectorOffset = new Vector2(-30f, 0f);
    [BoxGroup("Button Selector Config")]
    [SerializeField][Range(0, 20)] private float selectorVelocity = 0.01f;
    [BoxGroup("Button Selector Config")]
    [SerializeField] private AnimationCurve curve;

    [BoxGroup("Loading configs")]
    [SerializeField] private GameObject LoadingScreen;
    [BoxGroup("Loading configs")]
    [SerializeField] private GameObject LoadingIcon;
    [BoxGroup("Loading configs")]
    [SerializeField] private Vector3 RotateAmount;
    [SerializeField] private RectTransform _selector;


    [Header("Private variables")]
    private bool isSelectorMoving;
    private Vector3 targetPos;
    private Vector3 currentPos;
    private Vector3 startPos;
    private Canvas canvas;
    private float elapsedTime = 0f;


    public enum Menus
    {
        Main,
        Configuration,
        FileSelector,
        DisplaySettings,
        AudioSettings,
        InputSettings,
        Off
    }

    //Functions
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        _selector.SetParent(this.transform);
        ChangeMainMenuSelectorPosition(GetFirstMenuButton(Menus.Main));
        SetMenus();
        SetActive((int)Menus.Main);
    }


    public void ChangeMainMenuSelectorPosition(RectTransform rt)
    {
        _selector.parent = rt;
        _selector.DOAnchorPos(rt.anchoredPosition, 0.5f);
    }

    public void SetTargetPost(Vector2 _targetPost) 
    {
        elapsedTime = 0f;
        targetPos = _targetPost;
        currentPos = _selector.GetComponent<RectTransform>().anchoredPosition;
        isSelectorMoving = true;
    }

    private void Update()
    {
        if (isSelectorMoving)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / selectorVelocity;

            Vector2 nuevaPos = Vector2.Lerp(currentPos, targetPos, curve.Evaluate(percentageComplete));
            _selector.GetComponent<RectTransform>().anchoredPosition = nuevaPos;

            // Si está muy cerca del destino, terminar
            if (Vector2.Distance(nuevaPos, targetPos) < 1f)
            {
                _selector.GetComponent<RectTransform>().anchoredPosition = targetPos;
                isSelectorMoving = false;
            }
        }
    }


    /*
     * Metodo que se encarga de Activar un menu
     * */
    private Configuration.Exceptions SetActive(int menu)
    {
        try
        {
            foreach (var item in _menus)
            {
                if (item.Key.Equals(menu))
                {
                    item.Value.gameObject.SetActive(true);
                }
                else
                {
                    item.Value.gameObject.SetActive(false);
                }
            }

            int aux = 0;
            foreach (var item in _menus)
            {
                aux += 1;

                if (item.Value.gameObject.activeSelf) break;

                if (_menus.Count == aux) return Configuration.Exceptions.MenuException;
            }
        }
        catch (Exception) { return Configuration.Exceptions.None; }
        return Configuration.Exceptions.None;
    }


    /*
     * Metodo ejecutado tras pulsar el boton de New Game
     * */
    public void OnMouseButton_NewGame()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    /*
     * Metodo ejecutado tras pulsar el boton de Continuar en el menu de pausa
     * */
    public void OnMouseButton_ContinuePauseMenu()
    {
        try
        {
            var pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
            if (pauseMenu) pauseMenu.SetActive(false);
            int value = (int)SetActive((int)Menus.Main);
            ExceptionController(value);
            Cursor.lockState = CursorLockMode.Locked;
            StaticVariables.isPauseMenuActive = false;
        }
        catch (Exception) { ExceptionController((int)Menus.Main); }
    }
    /*
     * Metodo ejecutado tras pulsar el boton de Configuracion
     * */
    public void OnMouseButton_Configuration()
    {
        ChangeMainMenuSelectorPosition(GetFirstMenuButton(Menus.Configuration));
        int value = (int)SetActive((int)Menus.Configuration);
        ExceptionController(value);
    }

    /*
     * Metodo ejecutado tras pulsar el boton de Cargar
     * */
    public void OnMouseButton_FileSelector()
    {
        ChangeMainMenuSelectorPosition(GetFirstMenuButton(Menus.FileSelector));
        int value = (int)SetActive((int)Menus.FileSelector);
        ExceptionController(value);
    }

    /*
     * Metodo ejecutado tras pulsar el boton de Cargar
     * */
    public void OnMouseButton_MainMenu()
    {
        ChangeMainMenuSelectorPosition(GetFirstMenuButton(Menus.Main));
        int value = (int)SetActive((int)Menus.Main);
        ExceptionController(value);
    }

    /*
 * Metodo ejecutado tras pulsar el boton de Display Settings
 * */
    public void OnMouseButton_DisplaySettings()
    {
        ChangeMainMenuSelectorPosition(GetFirstMenuButton(Menus.DisplaySettings));
        int value = (int)SetActive((int)Menus.DisplaySettings);
        ExceptionController(value);
    }
    /*
    * Metodo ejecutado tras pulsar el boton de Audio Settings
    * */
    public void OnMouseButton_AudioSettings()
    {
        ChangeMainMenuSelectorPosition(GetFirstMenuButton(Menus.AudioSettings));
        int value = (int)SetActive((int)Menus.AudioSettings);
        ExceptionController(value);
    }
    /*
    * Metodo ejecutado tras pulsar el boton de Input Settings
    * */
    public void OnMouseButton_InputSettings()
    {
        ChangeMainMenuSelectorPosition(GetFirstMenuButton(Menus.InputSettings));
        int value = (int)SetActive((int)Menus.InputSettings);
        ExceptionController(value);
    }
    /*
     * Metodo ejecutado tras pulsar el boton de Salir
     * */
    public void OnMouseButton_Exit()
    {
        Application.Quit();
    }

    /*
     * Metodo ejecutado tras pulsar el boton de Salir al menu
     * */
    public void OnMouseButton_ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    /*
     * Metodo qe se ejecuta si ha ocurrido una excepcion
     */
    private void ExceptionController(int value)
    {
        if (Configuration.Exceptions.MenuException.Equals(value)) 
        {
            print(Configuration.ToString((int)Configuration.Exceptions.MenuException));
        }
    }

    public void SetMenus() 
    {
        _menus.Add(0,_mainMenu);
        _menus.Add(1, _settingsMenu);
        _menus.Add(2, _fileSelectorMenu);
        _menus.Add(3, _displaySettings);
        _menus.Add(4, _audioSettings);
        _menus.Add(5, _inputSettings);

    }


    /// <summary>
    /// Gets the first menu button
    /// </summary>
    /// <returns></returns>
    public RectTransform GetFirstMenuButton(Menus menu) 
    {
        RectTransform firstButton = new RectTransform();
        switch (menu)
        {
            case Menus.Main:
                firstButton = _mainMenu.GetComponentsInChildren<Button>()[0].GetComponent<RectTransform>();
                break;
            case Menus.Configuration:
                firstButton = _settingsMenu.GetComponentsInChildren<Button>()[0].GetComponent<RectTransform>();
                break;
            case Menus.FileSelector:
                firstButton = _fileSelectorMenu.GetComponentsInChildren<Button>()[0].GetComponent<RectTransform>();
                break;
            case Menus.DisplaySettings:
                firstButton = _displaySettings.GetComponentsInChildren<Button>()[0].GetComponent<RectTransform>();
                break;
            case Menus.AudioSettings:
                firstButton = _audioSettings.GetComponentsInChildren<Button>()[0].GetComponent<RectTransform>();
                break;
            case Menus.InputSettings:
                firstButton = _inputSettings.GetComponentsInChildren<Button>()[0].GetComponent<RectTransform>();
                break;
            case Menus.Off:
                break;
            default:
                break;
        }

        return firstButton;
    }

    IEnumerator LoadSceneAsync(int sceneId) 
    {
        LoadingScreen.SetActive(true);
        int i = 0;
        while(i < 400)
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
}
