using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public int player1Kills;
    public int player2Kills;

    public int player1Deads;
    public int player2Deads;

    public CanvasGroup player1End;
    public CanvasGroup player2End;

    public CanvasGroup restartTextCanvas;

    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;


    public int playerKillsNeeded = 30;

    public bool endGame = false;

    private bool canRestartGame = false;
    private bool flag = false;

    private void Start()
    {
        restartTextCanvas.DOFade(0, 0); ;
    }

    private void Update()
    {
        try
        {
            if (!player1End)
            {
                player1End = GameObject.FindGameObjectWithTag("Player1End").GetComponent<CanvasGroup>();
                return;
            }
            if (!player2End)
            {
                player2End = GameObject.FindGameObjectWithTag("Player2End").GetComponent<CanvasGroup>();
                return;
            }
        }
        catch (Exception ex) { }

        player1Text.text = $"Jugador 1: {player1Kills} <color=yellow>A</color> | {player1Deads} <color=red>M</color>.";
        player2Text.text = $"Jugador 2: {player2Kills} <color=yellow>A</color> | {player2Deads} <color=red>M</color>.";


        if (endGame && Input.anyKeyDown && canRestartGame) 
        {
            ResetGame();
        }


        if (player1Kills >= playerKillsNeeded && !flag)
        {
            endGame = true;
            StartCoroutine(Player1Wins());
            flag = true;
        }
        else if (player2Kills >= playerKillsNeeded && !flag) 
        {
            endGame = true;
            StartCoroutine(Player2Wins());
            flag = true;
        }
    }

    private void ResetGame() 
    {
        player1Kills = 0;
        player2Kills = 0;
        player1Deads = 0;
        player2Deads = 0;
        PlayerSpawner ps = GameObject.FindAnyObjectByType<PlayerSpawner>();

        ps.Spawn(0);
        ps.Spawn(1);

        endGame = false;

        HealthManager h1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<HealthManager>();
        HealthManager h2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<HealthManager>();
        Destroy(h1.gameObject);
        Destroy(h2.gameObject);

        flag = false;
        canRestartGame = false;
        StopAllCoroutines();
        restartTextCanvas.DOFade(0,0.5f);
    }


    IEnumerator Player1Wins() 
    {
        player1End.DOFade(0.5f, 0.5f);
        player2End.DOFade(0.5f, 0.5f);

        player2End.GetComponent<Image>().color = Color.green;
        player1End.GetComponent<Image>().color = Color.red;


        player2End.GetComponentInChildren<TextMeshProUGUI>().text = " GANASTE!";
        player1End.GetComponentInChildren<TextMeshProUGUI>().text = " PERDISTE!";


        HealthManager h1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<HealthManager>();
        HealthManager h2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<HealthManager>();

        yield return new WaitForSeconds(1f);
        h1.Die(false);
        canRestartGame = true;
        StartCoroutine(RestartTextAnimtion());
        restartTextCanvas.DOFade(0, 0);
    }

    IEnumerator Player2Wins()
    {
        player1End.DOFade(0.5f, 0.5f);
        player2End.DOFade(0.5f, 0.5f);

        player1End.GetComponent<Image>().color = Color.green;
        player2End.GetComponent<Image>().color = Color.red;

        player1End.GetComponentInChildren<TextMeshProUGUI>().text = " GANASTE!";
        player2End.GetComponentInChildren<TextMeshProUGUI>().text = " PERDISTE!";


        HealthManager h1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<HealthManager>();
        HealthManager h2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<HealthManager>();

        yield return new WaitForSeconds(1f);
        h2.Die(false);
        canRestartGame = true;
        StartCoroutine(RestartTextAnimtion());
    }

    IEnumerator RestartTextAnimtion() 
    {
        restartTextCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Presiona cualquier tecla para reiniciar";

        while (endGame) 
        {
            yield return new WaitForSeconds(0.5f);
            restartTextCanvas.DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.5f);
            restartTextCanvas.DOFade(0, 0.5f);
        }
    }

}
