using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/New Weapon", fileName = "New Weapon")]
public class Weapons : ScriptableObject
{
    [Header("Settings")]
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce;
    public float throwUpwardForce;

    [Header("Explosive")]
    public bool isExplosive;
    public float _triggerRoce = 0.5f;
    public float _explosionRadius = 5;
    public float _explosionForce = 500;
    public GameObject _particles;

    [Header("Easter Egg")]
    public bool isEdible = false;
}
