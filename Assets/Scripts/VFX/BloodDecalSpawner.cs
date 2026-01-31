using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BloodDecalSpawner : MonoBehaviour
{
    public Material decalMaterial;
    public float decalSize = 0.2f;

    private ParticleSystem ps;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Vector3 normal = collisionEvents[i].normal;

            GameObject decalObj = new GameObject("BloodDecal");

            // Añade un offset hacia arriba siguiendo la normal
            decalObj.transform.position = pos + normal * 0.05f; // Ajusta el valor 0.05f

            decalObj.transform.rotation = Quaternion.FromToRotation(Vector3.back, normal);

            DecalProjector decalProjector = decalObj.AddComponent<DecalProjector>();
            decalProjector.material = decalMaterial;
            decalProjector.size = new Vector3(decalSize, decalSize, 0.5f);

            Destroy(decalObj, 10f);
        }
    }
}
