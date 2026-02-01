using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlindnessPowerUp : MonoBehaviour
{
    public PowerUpManager powerUpManager;
    public Renderer playerRenderer;
    public GameObject powerUpMasks;
    private HealthManager healthManager;
    private PlayerInput playerInput;
    private List<TrailRenderer> trailRenderers;
    private PlayerMovement playerMovement;
    private int playerId;

    [Header("Player UI")]
    public CanvasGroup pBlack;
    public Image pCharge;

    private PowerUpData data;
    private float charge;

    private bool isUsing;
    private bool isBlind;
    private bool usedBlind;
    private void Start()
    {
        healthManager = GetComponent<HealthManager>();
        playerId = healthManager.playerIndex;
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        trailRenderers = playerMovement.wallRunningMarks.ToList();
        trailRenderers.Add(GetComponentInChildren<TrailRenderer>());
        StopBlind();
    }

    private void Update()
    {
        if (powerUpManager.powerUpData == null || powerUpManager.powerUpData.type != PowerUpType.Blindness)
        {
            SetCharge(0);
            return;
        }


        data = powerUpManager.powerUpData;

        Regenerate();
        HandleUse();

        SetCharge(charge / data.maxCharge);
    }

    private void Regenerate()
    {
        if (!isUsing)
        {
            charge += data.regenPerSecond * Time.deltaTime;
            charge = Mathf.Clamp(charge, 0, data.maxCharge);
        }
    }

    private void HandleUse()
    {
        if (usedBlind && charge >= data.minChargeToUse) 
        {
            usedBlind = false;
        }

        if (playerInput.AbilityStay)
        {
            if (!usedBlind)
            {
                isUsing = true;
                charge -= data.drainPerSecond * Time.deltaTime;

                if (!isBlind)
                    StartBlind();

                if (charge <= 0)
                {
                    charge = 0;
                    StopBlind();
                }
            }

        }
        else
        {
            if (isBlind) 
                StopBlind();
        }
    }

    private void StartBlind()
    {
        isBlind = true;
        SetBlind(true);
        playerRenderer.enabled = false;
        powerUpMasks.SetActive(false);
        SetTrails(false);
    }

    private void StopBlind()
    {
        isUsing = false;
        isBlind = false;
        usedBlind = true;

        SetBlind(false);
        SetTrails(true);
        playerRenderer.enabled = true;
        powerUpMasks.SetActive(true);

    }

    public void SetBlind(bool active)
    {
        if (active)
        {
            pBlack.DOFade(1f, 0.5f);
        }
        else 
        {
            pBlack.DOFade(0f, 0.5f);
        }
    }

    public void SetCharge(float normalized)
    {
        pCharge.fillAmount = normalized;
    }

    private void SetTrails(bool set) 
    {
        foreach (var trail in trailRenderers)
        {
            trail.enabled = set;
        }
    }

}
