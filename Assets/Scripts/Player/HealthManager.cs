using DG.Tweening;
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

    public bool isDead { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
        playerInput = GetComponent<PlayerInput>();
        playerDeadCanvas.alpha = 0f;
    }

    private void Update()
    {
        if (isDead) 
        {
            if (playerInput.JumpButtonDown)
            {
                playerDeadCanvas.alpha = 0f;
                GameObject.FindAnyObjectByType<PlayerSpawner>().Spawn(playerIndex);
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

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
        return bloodHitClips[Random.Range(0, bloodHitClips.Length)];
    }

    private void Die()
    {
        deadAudio.Play();
        isDead = true;
        playerDeadCanvas.alpha = 0.5f;
        Transform cam = GetComponentsInChildren<Transform>()[1];
        if (playerInput.inputDevice == PlayerInput.InputDevice.KeyboardMouse)
        {
            playerDeadCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Presiona espacio en tu teclado para reaparecer.";
        }
        else 
        {
            playerDeadCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "Presiona 'A' en tu mando para reaparecer.";

        }
        GameObject particle = Instantiate(deadBloodVfx, transform.position, Quaternion.identity);
        Destroy(particle, 8);
        playerRenderer.enabled = false;
        cam.transform.DOMoveZ(-25, 2);
    }

    IEnumerator DamageFlash() 
    {
        playerDamageIndicator.DOFade(0.5f, 0.1f);

        yield return new WaitForSeconds(0.1f);

        playerDamageIndicator.DOFade(0, 0.1f);
    }


}
