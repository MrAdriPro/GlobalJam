using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 2f; 

    private float xRotation = 0f;
    private float yRotation = 0f;

    public Transform camHolder;
    public Transform orientation;

    private PlayerInput playerInput;
    public PlayerInputSelector playerInputSelector;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        playerInput = GetComponentInParent<PlayerInput>();
    }

    void Update()
    {
        if (GetComponentInParent<HealthManager>().isDead || !playerInputSelector.selectedInput) return;

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
