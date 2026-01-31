using UnityEngine;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;
    public Weapons weapon;

    public PlayerInputSelector playerInputSelector;

    [SerializeField] bool readyToThrow;

    private PlayerInput playerInput;

    private void Start()
    {
        readyToThrow = true;
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (!playerInputSelector.selectedInput || GameObject.FindAnyObjectByType<PauseMenu>().isPaused
            || GameObject.FindAnyObjectByType<LeaderboardManager>().endGame) return;

        if (playerInput.Throw && readyToThrow)
        {
            Throw();
            GetComponent<PlayerAnimations>().ShootAnim();
        }
        else if (playerInput.EasterEgg) 
        {

        }
    }

    private void EatPizza() 
    {
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, attackPoint.rotation);

    }

    private void Throw() 
    {
        readyToThrow = false;

        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, attackPoint.rotation);

        ProjectileAddon pa = projectile.GetComponent<ProjectileAddon>();
        pa.playerBody = gameObject;
        pa.weapon = weapon;
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = attackPoint.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(attackPoint.position, attackPoint.forward, out hit, 500f)) 
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * weapon.throwForce + transform.up * weapon.throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        Invoke(nameof(ResetThrow), weapon.throwCooldown);
    }

    private void ResetThrow() 
    {
        readyToThrow = true;
    }
}
