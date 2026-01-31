using System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("SpawnPositions")]
    public Transform[] spawnPositions;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    private void Start()
    {
        GameObject player1 = SpawnPlayerStart(0);
        GameObject player2 = SpawnPlayerStart(1);

        Rotator[] player1Rotators = player1.GetComponentsInChildren<Rotator>();
        Rotator[] player2Rotators = player2.GetComponentsInChildren<Rotator>();

        for (int i = 0; i < player1Rotators.Length; i++)
        {
            player1Rotators[i].SetRotatorTarget(player2);
        }

        for (int i = 0; i < player2Rotators.Length; i++)
        {
            player2Rotators[i].SetRotatorTarget(player1);
        }
    }

    public void Spawn(int index) 
    {
        if (index == 0) Instantiate(player1Prefab, RandomPosition(), Quaternion.identity);
        else if (index > 0) Instantiate(player2Prefab, RandomPosition(), Quaternion.identity);

    }

    public GameObject SpawnPlayerStart(int index) 
    {
        GameObject player = null;

        if (index == 0) player = Instantiate(player1Prefab, RandomPosition(), Quaternion.identity);
        else if (index > 0) player = Instantiate(player2Prefab, RandomPosition(), Quaternion.identity);

        return player;
    }

    private Vector3 RandomPosition() 
    {
        return spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position;
    }
}
