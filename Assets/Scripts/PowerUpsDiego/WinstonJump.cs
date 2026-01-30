using UnityEngine;

public class WinstonJump : MonoBehaviour
{
    [Header("Jump")]
    public float verticalJumpForce;
    public float horizontalJumpForce;
    public float jumpForce;
    public float timeToStomp;
    [SerializeField] private float timerToStomp;
    public float stompForce;
    public float cooldown;
    [SerializeField] private float cooldownTimer;

    [Header("Cam effects")]
    public float camJumpFov = 70f;
    public float camStompFov = 100f;
    public float camShakeIntensity = 1f;
    public float camShakeDuration = 0.5f;

    [Header("Explosive")]
    public bool isExplosive;
    public float _explosionRadius = 5;
    public float _explosionForce = 500;
    public GameObject _particles;

    private Rigidbody rb;
    private PlayerMovement pm;
    private PlayerInput playerInput;
    private CameraController cam;
    private FirstPersonCameraBob camBob;

    private bool canDoAbility = true;
    private bool didJump = false;
    private bool canDoExplosion = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        cam = GetComponentInChildren<CameraController>();
        camBob = GetComponent<FirstPersonCameraBob>();
        canDoAbility = true;
        didJump = false;
        canDoExplosion = false;
    }

    private void Update()
    {
        if (playerInput.Ability && (pm.isGrounded || pm.wallRunning || pm.climbing) && canDoAbility)
        {
            StartJump();
        }
        else if (didJump) 
        {
            DoStomp();
        }
        else
        {
            if (cooldownTimer < 0) canDoAbility = true;
            if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
        }
    }

    private void StartJump() 
    {
        cam.DoFov(camJumpFov);
        canDoAbility = false;
        didJump = true;
        cooldownTimer = cooldown;
        timerToStomp = timeToStomp;

        Vector3 jumpDirection = transform.forward * horizontalJumpForce + transform.up * verticalJumpForce;
        rb.AddForce((jumpDirection * jumpForce), ForceMode.Impulse);
    }

    private void DoStomp() 
    {
        timerToStomp -= Time.deltaTime;
        if (timerToStomp < 0)
        {
            cam.DoFov(camStompFov);
            Vector3 stompDirection = transform.forward * horizontalJumpForce + -transform.up * stompForce;
            rb.AddForce((stompDirection * jumpForce), ForceMode.Impulse);
            didJump = false;
            canDoExplosion = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (isExplosive && canDoExplosion)
        {
            var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

            foreach (var obj in surroundingObjects)
            {
                var rb = obj.GetComponent<Rigidbody>();
                if (rb == null) continue;

                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }

            GameObject particles = Instantiate(_particles, transform.position, Quaternion.identity);

            canDoExplosion = false;
            cam.DoFov(80);
            camBob.ShakeCamera(camShakeIntensity, camShakeDuration);
            Destroy(particles, 1);
        }

        
    }

}
