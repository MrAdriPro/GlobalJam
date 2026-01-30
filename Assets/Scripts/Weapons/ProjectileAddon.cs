using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    private Rigidbody rb;
    private bool targetHit;
    public Weapons weapon;

    public GameObject playerBody;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (playerBody != collision.gameObject)
        {
            if (weapon.isExplosive)
            {
                var surroundingObjects = Physics.OverlapSphere(transform.position, weapon._explosionRadius);

                foreach (var obj in surroundingObjects)
                {
                    var rb = obj.GetComponent<Rigidbody>();
                    if (rb == null) continue;

                    rb.AddExplosionForce(weapon._explosionForce, transform.position, weapon._explosionRadius);
                }

                Instantiate(weapon._particles, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }

        }
        Destroy(gameObject);
    }
}
