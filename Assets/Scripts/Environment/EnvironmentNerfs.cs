using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EnvironmentNerfs : MonoBehaviour
{
    //Que salgan distintos tipos de adversidades: gravedad mas alta, viento al movimiento, controles invertidos

    private GameObject player1;
    private GameObject player2;
    public float timeToCheckNerf;
    public float timeNerf;

    private float startGravityMultiplier;
    private PlayerDialogueMessages playerDialogueMessages;

    [SerializeField] private float fadeInDuration = 0.4f;
    [SerializeField] private float fadeOutDuration = 0.4f;
    [SerializeField] private float stayDuration = 5f;
    [SerializeField] private float letterSpeed = 0.01f;




    private void Start()
    {
        StartCoroutine(RandomNerfs());
    }

    IEnumerator RandomNerfs()
    {
        yield return new WaitForSeconds(5);

        playerDialogueMessages = GameObject.FindAnyObjectByType<PlayerDialogueMessages>();


        while (true)
        {
            yield return new WaitForSeconds(timeToCheckNerf);
            int random = Random.Range(0, 2);

            player1 = GameObject.FindGameObjectWithTag("Player1");
            player2 = GameObject.FindGameObjectWithTag("Player2");

            startGravityMultiplier = player1.GetComponent<PlayerMovement>().gravityMultiplier;

            if (random == 0)
                StartCoroutine(GravityNerf());
            if (random == 1)
                StartCoroutine(InvertedControllers());

        }

    }

    IEnumerator GravityNerf()
    {
        playerDialogueMessages.FadeInOutInvoke("Se ha iniciado gravedad alterada.", fadeInDuration, fadeOutDuration, stayDuration, letterSpeed, new List<ColorWords>());

        PlayerMovement pm1 = player1.GetComponent<PlayerMovement>();
        PlayerMovement pm2 = player2.GetComponent<PlayerMovement>();

        pm1.gravityMultiplier = startGravityMultiplier + 1f;
        pm2.gravityMultiplier = startGravityMultiplier + 1f;

        yield return new WaitForSeconds(timeNerf);

        pm1.gravityMultiplier = startGravityMultiplier;
        pm2.gravityMultiplier = startGravityMultiplier;
    }

    IEnumerator InvertedControllers()
    {
        playerDialogueMessages.FadeInOutInvoke("Se ha iniciado controles alterados.", fadeInDuration, fadeOutDuration, stayDuration, letterSpeed, new List<ColorWords>());


        PlayerInput pi1 = player1.GetComponent<PlayerInput>();
        PlayerInput pi2 = player2.GetComponent<PlayerInput>();

        pi1.invertAllControllers = true;
        pi2.invertAllControllers = true;

        yield return new WaitForSeconds(timeNerf);

        pi1.invertAllControllers = false;
        pi2.invertAllControllers = false;

    }

}
