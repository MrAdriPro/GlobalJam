using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class M_ButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
{
    //Variables

    [SerializeField] private MenuButton b_Settings;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float offset = 450;
    private GameObject hoverObject;
    private RectTransform rt;

    private Vector3 originalPosition;
    private Vector3 endPosition;
    private Coroutine currentCoroutine;
    private TextMeshProUGUI childText = null;
    private Image childImage = null;
    private MainMenuController mainMenuController;

    //Functions
    private void Start()
    {
        SetStartingSettings();
    }

    public void OnSelect(BaseEventData eventData)
    {
        print(rt.name);
        Vector2 pos = rt.anchoredPosition;
        pos.x -= rt.rect.width / 2 + 25;
        Slider isASlider = GetComponent<Slider>();

        if (isASlider) 
        {
            pos.x -= offset;
        }
        mainMenuController.ChangeMainMenuSelectorPosition(pos);

        // Si es un toggle 
        M_ToggleSwitch toggle = GetComponent<M_ToggleSwitch>();
        if (toggle) mainMenuController.SetCurrentToggle(toggle);
        else mainMenuController.SetCurrentToggle(null
            );
        if (mainMenuController.audioSource != null && mainMenuController.selectSound != null)
        {
            mainMenuController.audioSource.PlayOneShot(mainMenuController.selectSound);
        }

        if (childText) childText.color = b_Settings.hoverColor;
        if (childImage) childImage.color = b_Settings.hoverColor;

        if (b_Settings.hasAnim)
        {

            StartSmoothMove(originalPosition + b_Settings.buttonOffsetAnimation);
        }

        if (_audioSource)
            _audioSource.PlayOneShot(b_Settings._OnHoverSound);
    }

    public void OnDeselect(BaseEventData eventData) 
    {
        if (childText) childText.color = b_Settings.noHoverColor;
        if (childImage) childImage.color = b_Settings.noHoverColor;

        if (b_Settings.hasAnim)
        {
            StartSmoothMove(originalPosition);
        }

        if (hoverObject)
            Destroy(hoverObject);
    }

    /// <summary>
    /// Establece la configuracion inicial
    /// </summary>
    private void SetStartingSettings() 
    {
        mainMenuController = GameObject.FindAnyObjectByType<MainMenuController>();

        rt = GetComponent<RectTransform>();


        childText = GetComponentInChildren<TextMeshProUGUI>();
        if (childText) childText.color = b_Settings.noHoverColor;
        childImage = transform.GetChild(0).GetComponent<Image>();
        
        if (childImage) childImage.color = b_Settings.noHoverColor;
        originalPosition = rt.anchoredPosition;

        if(!_audioSource) _audioSource = FindAnyObjectByType<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!this.GetComponent<Button>().interactable) return;

        if (childText) childText.color = b_Settings.hoverColor;
        if (childImage) childImage.color = b_Settings.hoverColor;

        if (b_Settings.hasAnim)
        {

            StartSmoothMove(originalPosition + b_Settings.buttonOffsetAnimation);
        }

        if (b_Settings.buttonType == ButtonType.MenuButton) 
        {
            Vector2 pos = rt.anchoredPosition;
            pos.x -= rt.rect.width / 2 + 25;
            mainMenuController.ChangeMainMenuSelectorPosition(pos);
            ButtonSelectImageCreator();
        }

        if(_audioSource)
        _audioSource.PlayOneShot(b_Settings._OnHoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!this.GetComponent<Button>().interactable) return;

        if (childText) childText.color = b_Settings.noHoverColor;
        if (childImage) childImage.color = b_Settings.noHoverColor;

        if (b_Settings.hasAnim)
        {
            StartSmoothMove(originalPosition);
        }

        if (hoverObject)
        Destroy(hoverObject);

    }

    private void OnDisable()
    {
        if (b_Settings.hasAnim)
        {
            if(rt)
            rt.anchoredPosition = originalPosition;
        }
        if (hoverObject)
            Destroy(hoverObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!this.GetComponent<Button>().interactable) return;

        if (_audioSource)_audioSource.PlayOneShot(b_Settings._OnClickSound);
        if (childText) childText.color = b_Settings.noHoverColor;
        if (childImage) childImage.color = b_Settings.noHoverColor;

        if (hoverObject)
        Destroy(hoverObject);

    }


    /// <summary>
    /// Realiza la animacion del boton
    /// </summary>
    /// <param name="targetPosition"></param>
    private void StartSmoothMove(Vector3 targetPosition)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(MoveToPosition(targetPosition));
    }

    /// <summary>
    /// Button select image instantiator
    /// </summary>
    private void ButtonSelectImageCreator() 
    {
        hoverObject = new GameObject("ImagenUI");
        Image imageComponent = hoverObject.AddComponent<Image>();
        imageComponent.sprite = b_Settings.hoverImage;
        imageComponent.transform.SetParent(this.transform, false);

        // Ajustar posición y tamaño dentro del botón
        RectTransform rectTransform = imageComponent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero; // Centrado
        rectTransform.sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x + b_Settings.hoverImageOffset.x,
                                              GetComponent<RectTransform>().sizeDelta.y + b_Settings.hoverImageOffset.y);
        imageComponent.color = b_Settings.hoverSpriteColor;
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        Vector3 start = rt.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < b_Settings.animationDuration)
        {
            rt.anchoredPosition = Vector3.Lerp(start, target, elapsed / b_Settings.animationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = target;
    }

}
