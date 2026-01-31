using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    private Rigidbody rb;
    public Weapons weapon;

    public GameObject playerBody;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HealthManager health = null;

        if (!weapon.isExplosive)
        {
            health = collision.gameObject.GetComponent<HealthManager>();
        }

        if (playerBody != collision.gameObject)
        {
            if (weapon.isExplosive)
            {
                var surroundingObjects = Physics.OverlapSphere(transform.position, weapon._explosionRadius);

                foreach (var obj in surroundingObjects)
                {
                    health = obj.GetComponent<HealthManager>();
                    var rb = obj.GetComponent<Rigidbody>();
                    if (rb == null) continue;

                    rb.AddExplosionForce(weapon._explosionForce, transform.position, weapon._explosionRadius);
                    if (health != null && health.playerIndex != playerBody.GetComponent<HealthManager>().playerIndex)
                    {
                        health.TakeDamage(weapon.damage);
                    }
                }

                GameObject particles = Instantiate(weapon._particles, transform.position, Quaternion.identity);

                Destroy(particles, 2);
            }

        }
        Destroy(gameObject);
    }
}
