using UnityEngine;



public enum PowerUpType
{
    None, SpeedAndDoubleJump, WinstonJump, Blindness, Grenade

}
[CreateAssetMenu(menuName = "PowerUpData/New PowerUp Data", fileName = "New PowerUp Data")]

public class PowerUpData : ScriptableObject
{
   
    public PowerUpType type;
    public float speedMultiplier = 16f;
    public int extraJumps = 1;
    public float winstonJumpMultiplier = 1f;
    [Header("Blindness PowerUp Settings")]
    public float maxCharge = 5f;
    public float drainPerSecond = 1f;
    public float regenPerSecond = 0.5f;
    public float minChargeToUse = 1f;
    public Weapons referencedWeapon;

}
