using UnityEngine;



public enum PowerUpType
{
    None, SpeedAndDoubleJump, WinstonJump
    
}
[CreateAssetMenu(menuName = "PowerUpData/New PowerUp Data", fileName = "New PowerUp Data")]

public class PowerUpData : ScriptableObject
{
   
    public PowerUpType type;
    public float speedMultiplier = 16f;
    public int extraJumps = 1;
    public float winstonJumpMultiplier = 1f;

}
