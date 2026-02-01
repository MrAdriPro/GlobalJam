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
            Debug.Log($"Frame {frameCount}: selectedInput = {player1inputSelector.selectedInput}");
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
            //Debug.Log($"Frame {frameCount}: selectedInput = {player1inputSelector.selectedInput}");
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