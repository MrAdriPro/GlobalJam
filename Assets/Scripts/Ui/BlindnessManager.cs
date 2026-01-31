using UnityEngine;
using UnityEngine.UI;

public class BlindnessManager : MonoBehaviour
{
    public static BlindnessManager Instance;

    [Header("Player 1 UI")]
    public Image p1Black;
    public Image p1Charge;

    [Header("Player 2 UI")]
    public Image p2Black;
    public Image p2Charge;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetBlind(int playerId, bool active)
    {
        GetBlack(playerId).gameObject.SetActive(active);
    }

    public void SetCharge(int playerId, float normalized)
    {
        GetCharge(playerId).fillAmount = normalized;
    }

    private Image GetBlack(int id) => id == 1 ? p1Black : p2Black;
    private Image GetCharge(int id) => id == 1 ? p1Charge : p2Charge;
}
