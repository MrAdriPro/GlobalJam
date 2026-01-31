using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRespawn : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] powerUpPrefabs;
    public float spawnInterval = 20f;
    public int maxActive = 3;

    private List<GameObject> active = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            CleanNulls();
            if (active.Count >= maxActive) continue;
            if (powerUpPrefabs.Length == 0 || spawnPoints.Length == 0) continue;
            int prefabIndex = Random.Range(0, powerUpPrefabs.Length);
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            GameObject go = Instantiate(powerUpPrefabs[prefabIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
            active.Add(go);
        }
    }

    private void CleanNulls()
    {
        for (int i = active.Count - 1; i >= 0; i--)
        {
            if (active[i] == null) active.RemoveAt(i);
        }
    }
}
