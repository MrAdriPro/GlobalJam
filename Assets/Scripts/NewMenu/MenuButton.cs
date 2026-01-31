using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "New Button", menuName = "Button/New Button")]
public class MenuButton : ScriptableObject
{
    //Variables

    [BoxGroup("Audio Settings")]
    public AudioClip _OnHoverSound;
    [BoxGroup("Audio Settings")]
    public AudioClip _OnClickSound;

    [BoxGroup("Color Settings")]
    public Color hoverColor;
    [BoxGroup("Color Settings")]
    public Color hoverSpriteColor;
    [BoxGroup("Color Settings")]
    public Color noHoverColor;

    [BoxGroup("Image Settings")]
    public Vector2 hoverImageOffset;
    [BoxGroup("Image Settings")]
    public Sprite hoverImage;
    [BoxGroup("Image Settings")]
    public Color hoverImageColor;

    [Header("Button type")]
    public ButtonType buttonType = ButtonType.InventoryButton;
    public bool hasAnim = true;
    public bool doesHoverImageEffect = true;

    [BoxGroup("Animation Settings")]
    public Vector3 buttonOffsetAnimation;
    [BoxGroup("Animation Settings")]
    public float animationDuration = 0.3f;


    //Functions
}

public enum ButtonType 
{
    InventoryButton,
    MenuButton,
    NoAnimButton
}
