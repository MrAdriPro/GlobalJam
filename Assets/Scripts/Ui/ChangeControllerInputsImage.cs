using UnityEngine;
using UnityEngine.UI;

public class ChangeControllerInputsImage : MonoBehaviour
{
    public Image targetImage;
    public Sprite targetSprite;

    public void Change() 
    {
        targetImage.sprite = targetSprite;
    }
}
