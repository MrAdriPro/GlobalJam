using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wall running")]
    public LayerMask wallMask;
    public LayerMask groundMask;
    public float wallRunForce;
    public float wallClimbSpeed;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    public float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;
    private bool upwardsRunning;
    private bool downwardsRunning;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    public bool wallLeft;
    public bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("References")]
    public Transform orientation;
    public PlayerMovement pm;
    private Rigidbody rb;
    public CameraController cam;
    private PlayerInput playerInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallRunning) WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wallMask);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wallMask);

    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundMask);
    }

    private void StateMachine() 
    {
        horizontalInput = playerInput.horizontalInput;
        verticalInput = playerInput.verticalInput;

        upwardsRunning = playerInput.UpwardsRunKey;
        downwardsRunning = playerInput.DownwardsRunKey;


        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallRunning) StartWallRun();

            if(wallRunTimer > 0) wallRunTimer -= Time.deltaTime;
            if (wallRunTimer <= 0 && pm.wallRunning) 
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if (playerInput.JumpButtonDown) WallJump();
        }
        else if (exitingWall) 
        {
            if (pm.wallRunning) StopWallRun();

            if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if (exitWallTimer <= 0) exitingWall = false;
        }
        else
        {
            if (pm.wallRunning) StopWallRun();
        }
    }

    private void StartWallRun() 
    {
        pm.wallRunning = true;

        wallRunTimer = maxWallRunTime;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        cam.DoFov(90f);

        if (wallLeft) cam.DoTilt(-5f);
        if (wallRight) cam.DoTilt(5);

    }

    private void WallRunningMovement() 
    {

        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude) 
        {
            wallForward = -wallForward;
        }

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if (upwardsRunning) rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbSpeed, rb.linearVelocity.z);
        if (downwardsRunning) rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallClimbSpeed, rb.linearVelocity.z);

        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0)) 
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        if (useGravity) rb.AddForce(transform.up * gravityCounterForce,ForceMode.Force);

    }

    private void StopWallRun() 
    {
        pm.wallRunning = false;

        cam.DoFov(80);
        cam.DoTilt(0f);
    }

    private void WallJump() 
    {
        pm.onJump?.Invoke();
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
