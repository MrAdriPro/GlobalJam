using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    PlayerInput playerInput;

    public CanvasGroup playerDamageIndicator;

    public Renderer playerRenderer;
    public GameObject bloodVfx;
    public GameObject deadBloodVfx;

    public AudioSource deadAudio;
    public AudioSource bloodHitAudio;
    public AudioClip[] bloodHitClips;
    public int playerIndex;
    public CanvasGroup playerDeadCanvas;

    [SerializeField] private bool didDamage = false;
    [SerializeField] private float timer;
    [SerializeField] private float time = 5f;

    public bool isDead { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
        playerInput = GetComponent<PlayerInput>();
        playerDeadCanvas.alpha = 0f;
        timer = time;
        didDamage = false;
    }

    private void Update()
    {
        if (isDead) 
        {
            if (playerInput.JumpButtonDown && !GameObject.FindAnyObjectByType<LeaderboardManager>().endGame)
            {
                playerDeadCanvas.alpha = 0f;
                GameObject.FindAnyObjectByType<PlayerSpawner>().Spawn(playerIndex);
                Destroy(gameObject);
            }
            else if(GameObject.FindAnyObjectByType<LeaderboardManager>().endGame)
            {
                playerDeadCanvas.alpha = 0f;

            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0)
        {
            didDamage = false;

        }
    }

    public void TakeDamage(int amount, bool _didDamage = false)
    {
        if (isDead) return;

        didDamage = _didDamage;
        timer = time;

        currentHealth -= amount;
        GameObject particle = Instantiate(bloodVfx, transform.position, Quaternion.identity);
        bloodHitAudio.PlayOneShot(GetRandomBloodClip());
        Destroy(particle, 8);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            return;
        }

        StopAllCoroutines();
        StartCoroutine(DamageFlash());

    }



    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    private AudioClip GetRandomBloodClip() 
    {
        return bloodHitClips[UnityEngine.Random.Range(0, bloodHitClips.Length)];
    }

    public void Die(bool active = true)
    {
        LeaderboardManager lm = GameObject.FindAnyObjectByType<LeaderboardManager>();

        if (didDamage) 
        {
            if (playerIndex == 0)
            {
                lm.player2Kills++;

            }
            else
            {
                lm.player2Deads++;

            }
        }
        if (playerIndex == 0)lm.player1Deads++;
        else  lm.player2Deads++; 

        


        deadAudio.Play();
        isDead = true;
        try
        {
            if (playerIndex == 0) GameObject.FindGameObjectWithTag("Player1Hand").SetActive(false);
            else GameObject.FindGameObjectWithTag("Player2Hand").SetActive(false);
        }
        catch (Exception ex) { }

        if (active)
        {
            playerDeadCanvas.alpha = 0.5f;
            if (playerInput.inputDevice == PlayerInput.InputDevice.KeyboardMouse)
            {
                playerDeadCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Presiona espacio en tu teclado para reaparecer.";
            }
            else
            {
                playerDeadCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Presiona 'A' en tu mando para reaparecer.";

            }
        }

        GameObject particle = Instantiate(deadBloodVfx, transform.position, Quaternion.identity);
        Destroy(particle, 8);
        playerRenderer.enabled = false;
        Transform cam = GetComponentsInChildren<Transform>()[1];

        //cam.transform.DOMoveZ(-25, 2);
    }

    IEnumerator DamageFlash() 
    {
        playerDamageIndicator.DOFade(0.5f, 0.1f);

        yield return new WaitForSeconds(0.1f);

        playerDamageIndicator.DOFade(0, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lava")
        {
            TakeDamage(1000);
        }
    }

}
