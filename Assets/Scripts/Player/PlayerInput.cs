using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public enum InputDevice { KeyboardMouse, Joystick1, Joystick2 }

    [Header("Configuración del Jugador")]
    public InputDevice inputDevice = InputDevice.KeyboardMouse;
    [SerializeField] private int playerIndex = 0;

    public bool invertAllControllers = false;

    // Propiedades públicas - Compatible con tus scripts
    public float horizontalInput { get; private set; }
    public float verticalInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    // Botones
    public bool JumpButton { get; private set; }
    public bool JumpButtonDown { get; private set; }
    public bool CrouchButton { get; private set; }
    public bool CrouchButtonDown { get; private set; }
    public bool CrouchButtonUp { get; private set; }
    public bool RunButton { get; private set; }

    // Keys específicas (para WallRunning y Climbing)
    public bool UpwardsRunKey { get; private set; }
    public bool DownwardsRunKey { get; private set; }
    public bool ForwardKey { get; private set; }
    public bool Throw { get; private set; }
    public bool Ability { get; private set; }
    public bool EasterEgg { get; private set; }

    void Update()
    {
        switch (inputDevice)
        {
            case InputDevice.KeyboardMouse:
                ReadKeyboardMouseInput();
                break;
            case InputDevice.Joystick1:
                ReadJoystickInput(1);
                break;
            case InputDevice.Joystick2:
                ReadJoystickInput(2);
                break;
        }

        string[] joystickNames = Input.GetJoystickNames();

        // Muestra todos los mandos conectados
        for (int i = 0; i < joystickNames.Length; i++)
        {
            Debug.Log($"Joystick {i + 1}: {joystickNames[i]}");
            // Ej: "Xbox Wireless Controller", "Wireless Controller", "PLAYSTATION(R)3 Controller"
        }

        // ¿Hay al menos 1 mando?
        if (joystickNames.Length > 0)
        {
            Debug.Log($"Mando activo: {joystickNames[0]} (Joystick 1)");
        }
    }

    void ReadKeyboardMouseInput()
    {
        // Movimiento (GetAxisRaw para respuesta instantánea)
        if (!invertAllControllers)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        else 
        {
            horizontalInput = -Input.GetAxisRaw("Horizontal");
            verticalInput = -Input.GetAxisRaw("Vertical");
        }

        if (!invertAllControllers)
        {
            // Ratón
            LookInput = new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            );
        }
        else 
        {
            // Ratón
            LookInput = new Vector2(
                -Input.GetAxis("Mouse X"),
                -Input.GetAxis("Mouse Y")
            );
        }



        // Botones
        JumpButton = Input.GetButton("Jump");
        JumpButtonDown = Input.GetButtonDown("Jump");
        CrouchButton = Input.GetButton("Crouch");
        CrouchButtonDown = Input.GetButtonDown("Crouch");
        CrouchButtonUp = Input.GetButtonUp("Crouch");
        RunButton = Input.GetButton("Run");
        Throw = Input.GetButtonDown("Fire1");
        Ability = Input.GetButtonDown("Fire2");
        // Keys específicas
        UpwardsRunKey = Input.GetKey(KeyCode.LeftShift);
        DownwardsRunKey = Input.GetKey(KeyCode.LeftControl);
        ForwardKey = Input.GetKey(KeyCode.W);
        EasterEgg = Input.GetButtonDown("Fire3");
    }

    void ReadJoystickInput(int joystickNumber)
    {
        string prefix = $"Joystick{joystickNumber}_";

        // Movimiento (GetAxisRaw para respuesta instantánea)
        if (!invertAllControllers)
        {
            horizontalInput = Input.GetAxis(prefix + "Horizontal Controller");
            verticalInput = Input.GetAxis(prefix + "Vertical Controller");
        }
        else
        {
            horizontalInput = -Input.GetAxis(prefix + "Horizontal Controller");
            verticalInput = -Input.GetAxis(prefix + "Vertical Controller");
        }


        if (!invertAllControllers)
        {
            // Cámara (stick derecho)
            LookInput = new Vector2(
                Input.GetAxis(prefix + "Joystick X"),
                Input.GetAxis(prefix + "Joystick Y")
            );
        }
        else
        {
            // Cámara (stick derecho)
            LookInput = new Vector2(
                
                -Input.GetAxis(prefix + "Joystick X"),
                -Input.GetAxis(prefix + "Joystick Y")

            );
        }


        // Aplicar deadzone
        if (Mathf.Abs(horizontalInput) < 0.2f) horizontalInput = 0f;
        if (Mathf.Abs(verticalInput) < 0.2f) verticalInput = 0f;
        if (LookInput.magnitude < 0.2f) LookInput = Vector2.zero;

        // Botones del mando
        int offset = (joystickNumber - 1) * 20;

        JumpButton = Input.GetButton(prefix + "Jump Controller");
        JumpButtonDown = Input.GetButtonDown(prefix + "Jump Controller");
        CrouchButton = Input.GetButton(prefix + "Crouch Controller");
        CrouchButtonDown = Input.GetButtonDown(prefix + "Crouch Controller");
        CrouchButtonUp = Input.GetButtonUp(prefix + "Crouch Controller");
        RunButton = Input.GetButton(prefix + "Run Controller");
        float trigger = Input.GetAxis(prefix + "Fire1");
        Throw = trigger > 0.1;
        float trigger2 = Input.GetAxis(prefix + "Fire2");
        Ability = trigger2 > 0.1;
        //EasterEgg = Input.GetButtonDown(prefix + "Fire3");

        UpwardsRunKey = RunButton;
        DownwardsRunKey = CrouchButton;
        ForwardKey = verticalInput > 0.5f;
    }

    public void SetInputDevice(InputDevice device)
    {
        inputDevice = device;
        Debug.Log($"Player {playerIndex + 1} usa: {device}");
    }
}
