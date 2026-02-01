using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class M_SettingsMenu : MonoBehaviour
{
    //Variables

    public PersistentData data;

    public GameObject AudioSettingsPanel;

    [Header("Audio")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioMixer audioMixer;
    public Slider mouseSensSlider;
    public Slider joystick1SensSlider;
    public Slider joystick2SensSlider;

    //Functions

    private void Start()
    {
        try
        {
            if (data) 
            {
                float masterLinear = Mathf.Pow(10f, data.master / 20f);
                masterSlider.value = masterLinear;

                float soundsLinear = Mathf.Pow(10f, data.sounds / 20f);
                soundSlider.value = soundsLinear;

                float musicLinear = Mathf.Pow(10f, data.music / 20f);
                musicSlider.value = musicLinear;

                mouseSensSlider.value = data.mouseSens;
                joystick1SensSlider.value = data.joystick1Sens;
                joystick2SensSlider.value = data.joystick2Sens;
            }


        }
        catch (Exception Ex) { }
    }

    private void Update()
    {
        Settings();
    }

    public void Settings() 
    {
        try
        {
            float masterValueAudio = Mathf.Log10(masterSlider.value) * 20;
            float soundsValueAudio = Mathf.Log10(soundSlider.value) * 20;
            float musicValueAudio = Mathf.Log10(musicSlider.value) * 20;

            audioMixer.SetFloat("Master", masterValueAudio);
            audioMixer.SetFloat("Sounds", soundsValueAudio);
            audioMixer.SetFloat("Music", musicValueAudio);

            data.master = masterValueAudio;
            data.sounds = soundsValueAudio;
            data.music = musicValueAudio;

            float masterValue = masterSlider.value * 100;
            float soundsValue = soundSlider.value * 100;
            float musicValue = musicSlider.value * 100;

            masterSlider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)masterValue).ToString();
            soundSlider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)soundsValue).ToString();
            musicSlider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)musicValue).ToString();

            mouseSensSlider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)mouseSensSlider.value).ToString();
            joystick1SensSlider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)joystick1SensSlider.value).ToString();
            joystick2SensSlider.GetComponentInChildren<TextMeshProUGUI>().text = ((int)joystick2SensSlider.value).ToString();

            data.mouseSens = (int)mouseSensSlider.value;
            data.joystick1Sens = (int)joystick1SensSlider.value;
            data.joystick2Sens = (int)joystick2SensSlider.value;

        }
        catch (Exception ex) { }
    }


}
