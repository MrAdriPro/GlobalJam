using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class M_InputSettings : MonoBehaviour
{
    //Variables

    [Header("Mouse Sens")]
    [SerializeField] private Slider mouseXSlider;
    [SerializeField] private Slider mouseYSlider;

    [SerializeField] private TextMeshProUGUI mouseX_text;
    [SerializeField] private TextMeshProUGUI mouseY_text;

    [SerializeField] private float mouse_SensX = 1f;
    public float Mouse_SensX { get { return mouse_SensX; } }
    [SerializeField] private float mouse_SensY = 1f;
    public float Mouse_SensY { get { return mouse_SensY; } }
    //Functions

    private void Start() 
    {
        mouseXSlider.value = 1;
        mouseYSlider.value = 1;

        mouseX_text.text = (mouseXSlider.value).ToString("F1");
        mouseY_text.text = (mouseYSlider.value).ToString("F1");
    }
    public void OnMouseSensXChange(Slider slider)
    {
        Configuration.mouse_SensX = slider.value * 100;
        mouseX_text.text = (slider.value).ToString("F1");
        print(Mouse_SensX);
    }
    public void OnMouseSensYChange(Slider slider)
    {
         Configuration.mouse_SensY = slider.value * 100;
        mouseY_text.text = (slider.value).ToString("F1");
        print(Mouse_SensY);

    }

}
