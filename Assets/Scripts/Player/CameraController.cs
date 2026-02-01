using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float mouseSensitivity = 2f; 

    private float xRotation = 0f;
    private float yRotation = 0f;

    public Transform camHolder;
    public Transform orientation;

    private PlayerInput playerInput;
    private M_SettingsMenu m_SettingsMenu;
    public PlayerInputSelector playerInputSelector;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        playerInput = GetComponentInParent<PlayerInput>();
        m_SettingsMenu = GameObject.FindAnyObjectByType<M_SettingsMenu>();
    }

    void Update()
    {
        if (!playerInput)
        {
            playerInput = GetComponentInParent<PlayerInput>();
            return;
        }

        if (!m_SettingsMenu)
        {
            m_SettingsMenu = GameObject.FindAnyObjectByType<M_SettingsMenu>();
            return;
        }

        if (playerInput.inputDevice == PlayerInput.InputDevice.KeyboardMouse)
        {
            mouseSensitivity = m_SettingsMenu.mouseSensSlider.value * 20;
        }
        else if (playerInput.inputDevice == PlayerInput.InputDevice.Joystick1)
        {
            mouseSensitivity = m_SettingsMenu.joystick1SensSlider.value * 20;
        }
        else if (playerInput.inputDevice == PlayerInput.InputDevice.Joystick2)
        {
            mouseSensitivity = m_SettingsMenu.joystick2SensSlider.value * 20;
        }

        if (GetComponentInParent<HealthManager>().isDead || !playerInputSelector.selectedInput || GameObject.FindAnyObjectByType<PauseMenu>().isPaused
            || GameObject.FindAnyObjectByType<LeaderboardManager>().endGame) return;

        float mouseX = playerInput.LookInput.x * Time.deltaTime * mouseSensitivity;
        float mouseY = playerInput.LookInput.y * Time.deltaTime * mouseSensitivity;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0f);
    }

    public void DoFov(float endValue) 
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt) 
    {
        transform.DOLocalRotate(new Vector3(0,0,zTilt), 0.25f);
    }
}
