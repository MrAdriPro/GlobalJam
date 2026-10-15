using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PlayerInputSelector : MonoBehaviour
{
    [Header("PlayerInput")]
    public CanvasGroup inputSelectorPanel;
    public TextMeshProUGUI inputText;
    public int playerIndex;
    public PlayerInput playerInput; // ¡Tu script personalizado!

    // Listas para emparejar el texto del UI con el dispositivo real
    private List<string> displayNames = new List<string>();
    private List<PlayerInput.InputDevice> realDevices = new List<PlayerInput.InputDevice>();

    private PlayerSpawner playerSpawner;
    public PlayerInput.InputDevice inputDevice;

    int currentController = 0;
    public bool selectedInput = false;

    private void Start()
    {
        GetComponentInChildren<M_ArrowController>().notSelectedNeeded = true;
        playerSpawner = GameObject.FindAnyObjectByType<PlayerSpawner>();

        if (playerIndex == 0) selectedInput = playerSpawner.player1Spawned;
        else selectedInput = playerSpawner.player2Spawned;

        if (selectedInput)
        {
            inputSelectorPanel.DOFade(0, 0f);
            inputDevice = playerIndex == 0 ? playerSpawner.player1Device : playerSpawner.player2Device;
            playerInput.inputDevice = inputDevice;
        }
        else
        {
            inputSelectorPanel.DOFade(1, 1f);
        }

        UpdateControllersList();

        GameObject player1 = null;
        if (playerIndex == 1)
        {
            player1 = GameObject.FindGameObjectWithTag("Player1");
            if (player1 != null)
            {
                PlayerInputSelector p1Selector = player1.GetComponentInChildren<PlayerInputSelector>();
                
                // Buscamos en qué posición de nuestra lista está el mando que eligió P1
                int p1Index = realDevices.IndexOf(p1Selector.inputDevice);
                if (p1Index != -1 && currentController == p1Index)
                {
                    currentController++;
                    WrapIndex();
                }
            }
        }

        UpdateUI();
    }

    void Update()
    {
        if (selectedInput) inputSelectorPanel.alpha = 0;
        
        UpdateControllersList(); // Actualizamos por si se desconecta un mando
    }

    private void UpdateControllersList()
    {
        displayNames.Clear();
        realDevices.Clear();

        // 0 = Teclado y ratón (asumiendo que en tu Enum el 0 es el teclado)
        displayNames.Add("Teclado y raton");
        realDevices.Add((PlayerInput.InputDevice)0);

        string[] joysticks = Input.GetJoystickNames();
        for (int i = 0; i < joysticks.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(joysticks[i]))
            {
                displayNames.Add(joysticks[i]);
                // La variable 'i' es 0, pero para Unity es Joystick 1.
                // Sumamos i + 1 para que el índice coincida exactamente con tu Enum
                realDevices.Add((PlayerInput.InputDevice)(i + 1));
            }
        }

        // Si se desconecta un mando y el cursor se queda fuera de rango, lo reiniciamos
        if (currentController >= displayNames.Count) currentController = 0;
    }

    private void WrapIndex()
    {
        if (currentController < 0) currentController = displayNames.Count - 1;
        else if (currentController >= displayNames.Count) currentController = 0;
    }

    public void ChangeSelector(int index)
    {
        if (!selectedInput && !GameObject.FindAnyObjectByType<PauseMenu>().isPaused)
        {
            currentController += index;
            WrapIndex();

            if (playerIndex == 1)
            {
                GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
                if (player1 != null)
                {
                    PlayerInputSelector p1Selector = player1.GetComponentInChildren<PlayerInputSelector>();
                    int p1Index = realDevices.IndexOf(p1Selector.inputDevice);

                    // Si el J2 elige el mismo mando físico que el J1, saltamos uno más
                    if (p1Index != -1 && currentController == p1Index)
                    {
                        currentController += index;
                        WrapIndex();
                    }
                }
            }
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (displayNames.Count > 0)
        {
            inputText.text = $"{currentController + 1}\n{displayNames[currentController]}";
        }
    }

    public void SelectController()
    {
        if (!selectedInput && !GameObject.FindAnyObjectByType<PauseMenu>().isPaused)
        {
            // ASIGNACIÓN CLAVE: Le pasamos a tu script el puerto real, no la posición del UI
            playerInput.SetInputDevice(realDevices[currentController]);
            inputDevice = realDevices[currentController];
            
            inputSelectorPanel.DOFade(0, 0.5f);
            selectedInput = true;
        }
    }
}