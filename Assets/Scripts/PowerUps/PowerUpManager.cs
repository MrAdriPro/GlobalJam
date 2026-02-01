using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public PlayerMovement movement;

    private PowerUpType currentPowerUp = PowerUpType.None;

    private float baseWalkSpeed;
    private float baseRunSpeed;
    private int baseExtraJumps;
    public PowerUpData powerUpData;
    public SpriteRenderer[] powerMasks;

    public int currentExtraJumps;


    private void Start()
    {
        baseWalkSpeed = movement.walkSpeed;
        baseRunSpeed = movement.runSpeed;
        baseExtraJumps = 0;
    }

    public void ApplyPowerUp(PowerUpData data)
    {
        RemoveCurrentPowerUp();
        powerUpData = data;

        currentPowerUp = data.type;

        if (data.referencedWeapon != null)
        {
            GetComponent<Throwing>().weapon = data.referencedWeapon;
        }

        if (currentPowerUp == PowerUpType.SpeedAndDoubleJump)
        {
            movement.walkSpeed = data.speedMultiplier;
            movement.runSpeed = data.speedMultiplier;
            currentExtraJumps = data.extraJumps;

        }
    }

    private void RemoveCurrentPowerUp()
    {
        powerUpData = null;
        movement.walkSpeed = baseWalkSpeed;
        movement.runSpeed = baseRunSpeed;
        currentExtraJumps = baseExtraJumps;

        GetComponent<Throwing>().weapon = GetComponent<Throwing>().defaultWeapon;
        currentPowerUp = PowerUpType.None;

    }

    public bool CanExtraJump()
    {
        if (currentExtraJumps > 0)
        {
            return true;
        }
        return false;
    }

    public void ResetExtraJumps()
    {
        if (powerUpData != null)
        {
            currentExtraJumps = powerUpData.extraJumps;
            return;
        }
    }

    public void DoesJumped()
    {
        currentExtraJumps--;
    }
}
