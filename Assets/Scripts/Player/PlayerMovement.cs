using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float walkSpeed = 7f;
    public float runSpeed = 12f;
    public float slideSpeed;
    public float wallRunSpeed;
    public float climbSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public bool sliding;
    public bool wallRunning;
    public bool climbing;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;


    public float groundDrag;

    [Header("Jump")]
    public float gravityMultiplier = 2f;  // 2x más fuerte (ajusta 1.5-3)
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    [SerializeField] bool readyToJump;


    [Header("Ground Check")]
    public float playerHeight = 2;
    public LayerMask groundMask;
    public bool isGrounded = false;

   

    [Header("Slope handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    [Header("References")]
    public Climbing climbScript;
    public PowerUpManager powerUpManager;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    private PlayerInput playerInput;

    public TrailRenderer[] wallRunningMarks;
    public enum MovementState 
    {
        walking,
        sprinting,
        crouching,
        sliding,
        wallRunning,
        climbing,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        playerInput = GetComponent<PlayerInput>();
        startYScale = transform.localScale.y;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        MyInput();
        SpeedControl();
        StateHandler();
        IsWallRunning();

        if (isGrounded)
            rb.linearDamping = groundDrag;
        else
        {
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
            rb.linearDamping = 0;
        }
    }

    private void MyInput() 
    {
        horizontalInput = playerInput.horizontalInput;
        verticalInput = playerInput.verticalInput;
        if (isGrounded && powerUpManager.powerUpData != null)
        {
            if (powerUpManager.currentExtraJumps != powerUpManager.powerUpData.extraJumps)
            {
                powerUpManager.ResetExtraJumps();

            }
        }
        if (playerInput.JumpButtonDown)
        {
            if (isGrounded && readyToJump)
            {
                readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
            }
            else if (!isGrounded && powerUpManager.CanExtraJump())
            {
                print("salto");
                Jump();
                powerUpManager.DoesJumped();
            }
        }

        if (playerInput.CrouchButtonDown)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (playerInput.CrouchButtonUp)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

    }

    public bool isMoving() => horizontalInput != 0 || verticalInput != 0;


    private void IsWallRunning() 
    {
        if (wallRunning || sliding || climbing)
        {
            StartEmmiterTrail();
        }
        else 
        {
            StopEmmiter();
        }
    }

    private void StartEmmiterTrail() 
    {
        foreach (var trail in wallRunningMarks)
        {
            trail.emitting = true;
        }
    }

    private void StopEmmiter()
    {
        foreach (var trail in wallRunningMarks)
        {
            trail.emitting = false;
        }
    }

    private void MovePlayer() 
    {
        if (climbScript.exitingWall) return;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope) 
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * speed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0) 
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if(isGrounded)
        rb.AddForce(moveDirection.normalized * 5 * speed, ForceMode.Force);


        else if (!isGrounded)
            rb.AddForce(moveDirection.normalized * 5 * speed * airMultiplier, ForceMode.Force);

        if(!wallRunning) rb.useGravity = !OnSlope();
    }

    private void StateHandler() 
    {
        if (climbing)
        {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }
        else if (wallRunning)
        {
            state |= MovementState.wallRunning;
            desiredMoveSpeed = wallRunSpeed;
        }
        else if (sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.linearVelocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = runSpeed;
            }
        }
        else if (playerInput.CrouchButton)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (isGrounded && playerInput.RunButton)
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = runSpeed;
        }
        else if (isGrounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 12f && speed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else 
        {
            speed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void SpeedControl()
    {

        if (OnSlope() && !exitingSlope) 
        {
            if(rb.linearVelocity.magnitude > speed) 
            {
                rb.linearVelocity = rb.linearVelocity.normalized * speed;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

            if (flatVelocity.magnitude > speed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * speed;
                rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
            }
        }
    }

    private void Jump() 
    {
        exitingSlope = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (sliding)
        {
            rb.AddForce(transform.up * (jumpForce * 1.5f), ForceMode.Impulse);

        }
        else 
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void ResetJump() 
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope() 
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) 
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private IEnumerator SmoothlyLerpMoveSpeed() 
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - speed);
        float startValue = speed;

        while (time < difference) 
        {
            speed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }

        speed = desiredMoveSpeed;
    }
}
