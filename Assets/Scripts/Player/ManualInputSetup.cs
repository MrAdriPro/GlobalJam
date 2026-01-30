using UnityEngine;

public class ManualInputSetup : MonoBehaviour
{
    [SerializeField] private PlayerInput player1;
    [SerializeField] private PlayerInput player2;

    void Start()
    {
        // Player 1: Teclado y Ratón
        player1.SetInputDevice(PlayerInput.InputDevice.KeyboardMouse);

        // Player 2: Primer mando conectado
        player2.SetInputDevice(PlayerInput.InputDevice.Joystick1);

        // Si tienes 2 mandos y quieres que cada jugador use uno diferente:
        // player1.SetInputDevice(PlayerInput.InputDevice.Joystick1);
        // player2.SetInputDevice(PlayerInput.InputDevice.Joystick2);
    }
}
