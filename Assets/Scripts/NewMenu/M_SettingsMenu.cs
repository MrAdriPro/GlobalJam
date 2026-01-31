using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class M_SettingsMenu : MonoBehaviour
{
    //Variables


    [Header("Audio")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioMixer audioMixer;

    //Functions

    private void Start()
    {
    }

    private void Update()
    {
        Settings();
    }

    public void Settings() 
    {
        audioMixer.SetFloat("Master", Mathf.Log10(masterSlider.value) * 20);
        audioMixer.SetFloat("Sounds", Mathf.Log10(soundSlider.value) * 20);
        audioMixer.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
        float masterValue = masterSlider.value * 100;
        float soundsValue = soundSlider.value * 100;
        float musicValue = musicSlider.value * 100;

        masterSlider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)masterValue).ToString();
        soundSlider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)soundsValue).ToString();
        musicSlider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)musicValue).ToString();
    }


}
