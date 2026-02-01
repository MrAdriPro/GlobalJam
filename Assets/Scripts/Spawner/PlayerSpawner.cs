using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("SpawnPositions")]
    public Transform[] spawnPositions;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    public bool player1Spawned = false;
    public bool player2Spawned = false;

    public PlayerInput.InputDevice player1Device;
    public PlayerInput.InputDevice player2Device;

    public GameObject levelCam;
    public GameObject levelCam2;

    private int lastSpawnPosP1 = -1;
    private int lastSpawnPosP2 = -1;


    private void Start()
    {
        player1Spawned = false;
        player2Spawned = false;
        levelCam2.SetActive(false);
        StartCoroutine(SpawnPlayers());
    }

    IEnumerator SpawnPlayers()
    {
        GameObject player1 = SpawnPlayerStart(0);

        PlayerInputSelector player1inputSelector = player1.GetComponentInChildren<PlayerInputSelector>();

        GameObject.FindWithTag("Player1Hand").GetComponent<CanvasGroup>().alpha = 0;


        int frameCount = 0;

        while (player1inputSelector.selectedInput == false)
        {
            frameCount++;
            yield return null;
        }
        player1Spawned = true;
        player1Device = player1inputSelector.inputDevice;

        GameObject.FindWithTag("Player1Hand").GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        levelCam2.SetActive(true);
        StartCoroutine(GameObject.FindAnyObjectByType<PauseMenu>().RestartTextAnimtion2());
        Destroy(levelCam);


        GameObject player2 = SpawnPlayerStart(1);

        PlayerInputSelector player2inputSelector = player2.GetComponentInChildren<PlayerInputSelector>();

        GameObject.FindWithTag("Player2Hand").GetComponent<CanvasGroup>().alpha = 0;


        while (!player2inputSelector.selectedInput)
        {
            frameCount++;
            yield return null;
        }
        Destroy(levelCam2);
        player2Spawned = true;
        player2Device = player2inputSelector.inputDevice;

        GameObject.FindWithTag("Player2Hand").GetComponent<CanvasGroup>().DOFade(1, 0.5f);


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
        if (index == 0) Instantiate(player1Prefab, RandomPosition(index), Quaternion.identity);
        else if (index > 0) Instantiate(player2Prefab, RandomPosition(index), Quaternion.identity);

    }

    public GameObject SpawnPlayerStart(int index)
    {
        GameObject player = null;

        if (index == 0) player = Instantiate(player1Prefab, RandomPosition(index), Quaternion.identity);
        else if (index > 0) player = Instantiate(player2Prefab, RandomPosition(index), Quaternion.identity);

        return player;
    }

    private Vector3 RandomPosition(int index)
    {

        while (true)
        {
            int random = UnityEngine.Random.Range(0, spawnPositions.Length);

            if (index == 0)
            {
                if (random != lastSpawnPosP2)
                {
                    lastSpawnPosP1 = random;
                    return spawnPositions[random].position;
                }
            }
            else
            {
                if (random != lastSpawnPosP1)
                {
                    lastSpawnPosP2 = random;
                    return spawnPositions[random].position;
                }
            }
        }
        
    }
}