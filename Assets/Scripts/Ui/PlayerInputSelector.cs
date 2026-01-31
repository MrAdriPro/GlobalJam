using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerInputSelector : MonoBehaviour
{
    [Header("PlayerInput")]
    public CanvasGroup inputSelectorPanel;
    public TextMeshProUGUI inputText;
    public int playerIndex;
    public PlayerInput playerInput;
    string[] controllers;
    private PlayerSpawner playerSpawner;
    public PlayerInput.InputDevice inputDevice;

    int currentController = 0;
    public bool selectedInput = false;
    private void Start()
    {
        GetComponentInChildren<M_ArrowController>().notSelectedNeeded = true;
        playerSpawner = GameObject.FindAnyObjectByType<PlayerSpawner>();

        if (playerIndex == 0)
            selectedInput = playerSpawner.player1Spawned;
        else selectedInput = playerSpawner.player2Spawned;

        if (selectedInput)
        {
            inputSelectorPanel.DOFade(0, 0f);
            if (playerIndex == 0)
            {
                inputDevice = playerSpawner.player1Device;
            }
            else
            {
                inputDevice = playerSpawner.player2Device;
            }

            playerInput.inputDevice = inputDevice;
        }

        else inputSelectorPanel.DOFade(1, 1f);

        string[] c = new string[Input.GetJoystickNames().Length + 1];
        string[] j = Input.GetJoystickNames();
        c[0] = "Teclado y raton";
        for (int i = 0; i < j.Length; i++)
        {
            c[i + 1] = j[i];
        }
        controllers = c;

        if (controllers.Length > 1)
        {
            inputText.text = controllers[playerIndex] + "(" + currentController + ")";

        }

        GameObject player = null;
        if (playerIndex == 1) player = GameObject.FindGameObjectWithTag("Player1");

        if (player)
        {
            PlayerInputSelector otherPlayerInputSelector = player.GetComponentInChildren<PlayerInputSelector>();


            if ((int)otherPlayerInputSelector.inputDevice == 0)
            {
                if (controllers.Length > 1)
                    currentController = 1;
                else currentController = 0;
            }
            else if ((int)otherPlayerInputSelector.inputDevice == 1)
            {
                if (controllers.Length > 2)
                    currentController = 2;
                else currentController = 0;
            }
            else if ((int)otherPlayerInputSelector.inputDevice == 2)
            {
                if (controllers.Length > 2)
                    currentController = 1;
                else currentController = 0;
            }

            inputText.text = controllers[currentController] + "(" + currentController + ")";
        }

    }

    void Update()
    {

        string[] c = new string[Input.GetJoystickNames().Length + 1];
        string[] j = Input.GetJoystickNames();
        c[0] = "Teclado y raton";
        for (int i = 0; i < j.Length; i++)
        {
            c[i + 1] = j[i];
        }
        controllers = c;
    }

    public void ChangeSelector(int index)
    {
        if (!selectedInput)
        {

            currentController = currentController + index;


            if (currentController < 0) currentController = controllers.Length - 1;
            else if (currentController > controllers.Length - 1) currentController = 0;

            GameObject player = null;
            if (playerIndex == 1) player = GameObject.FindGameObjectWithTag("Player1");

            if (player)
            {
                PlayerInputSelector otherPlayerInputSelector = player.GetComponentInChildren<PlayerInputSelector>();


                if ((int)otherPlayerInputSelector.inputDevice == 0)
                {
                    if (controllers.Length > 1)
                        currentController = 1;
                    else currentController = 0;
                }
                else if ((int)otherPlayerInputSelector.inputDevice == 1)
                {
                    if (controllers.Length > 2)
                        currentController = 2;
                    else currentController = 0;
                }
                else if ((int)otherPlayerInputSelector.inputDevice == 2)
                {
                    if (controllers.Length > 2)
                        currentController = 1;
                    else currentController = 0;
                }

            }

            inputText.text = controllers[currentController] + "(" + currentController + ")";
        }
    }

    public void SelectController()
    {
        if (!selectedInput)
        {
            playerInput.inputDevice = (PlayerInput.InputDevice)currentController;
            inputDevice = (PlayerInput.InputDevice)currentController;
            inputSelectorPanel.DOFade(0, 0.5f);
            selectedInput = true;
        }
    }
}
