using UnityEngine;

public class WinstonJump : MonoBehaviour
{
    public float verticalJumpForce;
    public float horizontalJumpForce;
    public float jumpForce;
    public float timeToStomp;
    [SerializeField] private float timerToStomp;
    public float stompForce;
    public float cooldown;
    [SerializeField] private float cooldownTimer;

    public float camJumpFov = 70f;
    public float camStompFov = 100f;
    public float camShakeIntensity = 1f;
    public float camShakeDuration = 0.5f;

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
        bool hasPower = pm != null && pm.powerUpManager != null && pm.powerUpManager.powerUpData != null && pm.powerUpManager.powerUpData.type == PowerUpType.WinstonJump;

        if (playerInput.Ability && (pm.isGrounded || pm.wallRunning || pm.climbing) && canDoAbility && hasPower)
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

        float mult = 1f;
        if (pm != null && pm.powerUpManager != null && pm.powerUpManager.powerUpData != null)
            mult = pm.powerUpManager.powerUpData.winstonJumpMultiplier;

        Vector3 jumpDirection = transform.forward * (horizontalJumpForce * mult) + transform.up * (verticalJumpForce * mult);
        rb.AddForce((jumpDirection * (jumpForce * mult)), ForceMode.Impulse);
    }

    private void DoStomp()
    {
        timerToStomp -= Time.deltaTime;
        if (timerToStomp < 0)
        {
            cam.DoFov(camStompFov);
            float mult = 1f;
            if (pm != null && pm.powerUpManager != null && pm.powerUpManager.powerUpData != null)
                mult = pm.powerUpManager.powerUpData.winstonJumpMultiplier;

            Vector3 stompDirection = transform.forward * (horizontalJumpForce * mult) + -transform.up * (stompForce * mult);
            rb.AddForce((stompDirection * (jumpForce * mult)), ForceMode.Impulse);
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
                var r = obj.GetComponent<Rigidbody>();
                if (r == null) continue;

                r.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }

            GameObject particles = Instantiate(_particles, transform.position, Quaternion.identity);

            canDoExplosion = false;
            cam.DoFov(80);
            camBob.ShakeCamera(camShakeIntensity, camShakeDuration);
            Destroy(particles, 1);
        }
    }
}
