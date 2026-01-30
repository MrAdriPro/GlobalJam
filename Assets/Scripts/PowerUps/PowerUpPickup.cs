using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public PowerUpData data;
    private void OnTriggerEnter(Collider other)
    {
        PowerUpManager manager = other.GetComponent<PowerUpManager>();
        if (manager != null)
        {
            manager.ApplyPowerUp(data);
            Destroy(gameObject);
        }
    }
}
