using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    PlayerInput playerInput;
    public Renderer playerRenderer;

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

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    private void Die()
    {
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
        playerRenderer.enabled = false;
        cam.transform.DOMoveZ(-25, 2);
    }
}
