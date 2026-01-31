using UnityEngine;

public class BlindnessPowerUp : MonoBehaviour
{
    public int playerId;
    public PowerUpManager powerUpManager;
    public Renderer playerRenderer;
    public KeyCode useKey = KeyCode.E;

    private BlindnessManager ui;
    private PowerUpData data;
    private float charge;

    private bool isUsing;
    private bool isBlind; 

    private void Start()
    {
        ui = BlindnessManager.Instance;
    }

    private void Update()
    {
        if (ui == null) return;
        if (powerUpManager.powerUpData == null) return;
        if (powerUpManager.powerUpData.type != PowerUpType.Blindness) return;

        data = powerUpManager.powerUpData;

        Regenerate();
        HandleUse();

        ui.SetCharge(playerId, charge / data.maxCharge);
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
        if (Input.GetKey(useKey) && charge >= data.minChargeToUse)
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
        else
        {
            if (isBlind) 
                StopBlind();
        }
    }

    private void StartBlind()
    {
        isBlind = true;

        ui.SetBlind(playerId, true);
        playerRenderer.enabled = false;
    }

    private void StopBlind()
    {
        isUsing = false;
        isBlind = false;

        ui.SetBlind(playerId, false);
        playerRenderer.enabled = true;
    }
}
