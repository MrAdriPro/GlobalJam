using UnityEngine;

public class ChangePowerMask : MonoBehaviour
{
    public SpriteRenderer[] childrenSprites;
    public Sprite[] originalSprites;
    public PowerUpManager powerUpManager;

    private Sprite appliedSprite;

    private void Awake()
    {
        childrenSprites = GetComponentsInChildren<SpriteRenderer>(true);
        originalSprites = new Sprite[childrenSprites.Length];
        for (int i = 0; i < childrenSprites.Length; i++)
        {
            originalSprites[i] = childrenSprites[i].sprite;
        }

        if (powerUpManager == null)
        {
            powerUpManager = GetComponentInParent<PowerUpManager>();
        }
    }

    private void Update()
    {
        if (powerUpManager == null)
            return;

        var data = powerUpManager.powerUpData;
        Sprite target = (data != null && data.maskSprite != null) ? data.maskSprite : null;

        if (appliedSprite == target) return;

        if (target == null)
        {
            RestoreOriginalSprites();
            appliedSprite = null;
        }
        else
        {
            ApplySpriteToChildren(target);
            appliedSprite = target;
        }
    }

    private void ApplySpriteToChildren(Sprite sprite)
    {
        for (int i = 0; i < childrenSprites.Length; i++)
        {
            if (childrenSprites[i] != null)
                childrenSprites[i].sprite = sprite;
        }
    }

    private void RestoreOriginalSprites()
    {
        for (int i = 0; i < childrenSprites.Length; i++)
        {
            if (childrenSprites[i] != null && i < originalSprites.Length)
                childrenSprites[i].sprite = originalSprites[i];
        }
    }
}
