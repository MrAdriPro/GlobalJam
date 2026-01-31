using UnityEngine;
using UnityEngine.UI;

public class BlindnessManager : MonoBehaviour
{
    public static BlindnessManager Instance;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


}
