using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playeObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    private float horizontalInput;
    private float verticalInput;

    private PlayerInput playerInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        startYScale = playeObj.localScale.y;
    }

    private void Update()
    {
        if (!GetComponentInChildren<PlayerInputSelector>().selectedInput || GameObject.FindAnyObjectByType<PauseMenu>().isPaused) return;

        horizontalInput = playerInput.horizontalInput;
        verticalInput = playerInput.verticalInput;

        if (playerInput.CrouchButtonDown && (horizontalInput != 0 || verticalInput != 0)) 
        {
            StartSlide();
        }

        if (playerInput.CrouchButtonUp && pm.sliding) 
        {
            StopSlide();
        }

    }

    private void FixedUpdate()
    {
        if (!GetComponentInChildren<PlayerInputSelector>().selectedInput || GameObject.FindAnyObjectByType<PauseMenu>().isPaused) return;

        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide() 
    {
        pm.sliding = true;
        playeObj.localScale = new Vector3(playeObj.localScale.x, slideYScale, playeObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void StopSlide() 
    {
        pm.sliding = false;

        playeObj.localScale = new Vector3(playeObj.localScale.x, startYScale, playeObj.localScale.z);
    }

    private void SlidingMovement() 
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (!pm.OnSlope() || rb.linearVelocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        else 
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }


        if (slideTimer <= 0) StopSlide();
    }


}
