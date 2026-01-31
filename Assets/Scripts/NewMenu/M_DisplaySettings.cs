using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class M_DisplaySettings : MonoBehaviour
{
    [Header("Display")]
    private bool isFullscreen = true;
    private bool auxIsFullScreen;
    private bool isVsyncOn = true;
    private bool auxIsVsyncOn;
    private int antialiasingValue = 0;
    [SerializeField] private Slider vSyncSlider;
    [SerializeField] private Slider fullscreenSlider;
    [SerializeField] private TextMeshProUGUI vSyncText;
    [SerializeField] private TextMeshProUGUI fullscreenText;
    [SerializeField] private TextMeshProUGUI antialiasingText;


    [Header("Display/Resolutions")]
    [SerializeField] private TextMeshProUGUI resolutionText;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private List<string> ResolutionOption = new List<string>();
    private RefreshRate currentRefreshRate;
    private int currentResolutionIndex;
    private int auxResolutionIndex;


    //Others
    //[SerializeField] private HDAdditionalCameraData hdac;
    //Functions

    private void Awake()
    {
        print("Se inicia");
        OnSettingsOn();
        VsyncToggle(true);
        GetResolutions();
        
    }
    private void Update()
    {
    }


    private void OnSettingsOn()
    {

        vSyncSlider.value = (QualitySettings.vSyncCount == 1) ? 1 : 0;
        VsyncToggle((QualitySettings.vSyncCount == 1) ? true : false);
        auxIsVsyncOn = isVsyncOn;
        fullscreenSlider.value = (Screen.fullScreen) ? 1 : 0;
        FullscreenToggle(Screen.fullScreen);
        auxIsFullScreen = isFullscreen;
        //antialiasingValue = (int)hdac.antialiasing;
        Antialiasing(antialiasingValue);
    }

    public void OnApplyButton()
    {
        SetResolution(auxResolutionIndex);
        SetTogglesOnApply();
        SetAntialiasing(antialiasingValue);
    }

    public void OnBackButton()
    {
        auxResolutionIndex = currentResolutionIndex;
        ChangeResolutionIndex(currentResolutionIndex);
        VsyncToggle(auxIsVsyncOn);
        FullscreenToggle(auxIsFullScreen);
        //SetAntialiasingOnBackButton((int)hdac.antialiasing);
        vSyncSlider.value = auxIsVsyncOn ? 1 : 0;
        fullscreenSlider.value = auxIsFullScreen ? 1 : 0;

    }

    public void VsyncToggle(bool value)
    {
        vSyncSlider.GetComponent<M_ToggleSwitch>().UpdateCurrentValue(value);
        vSyncText.text = value ? "ON" : "OFF";
        isVsyncOn = value;
    }

    public void FullscreenToggle(bool value)
    {
        fullscreenSlider.GetComponent<M_ToggleSwitch>().UpdateCurrentValue(value);
        isFullscreen = value;
        fullscreenText.text = value ? "ON" : "OFF";

    }

    public void Antialiasing(int index)
    {

        if (antialiasingValue + index <= 3 && antialiasingValue + index >= 0)
        {
            antialiasingValue += index;
        }



        switch (antialiasingValue)
        {
            case 0:
                antialiasingText.text = "OFF";
                break;
            case 1:
                antialiasingText.text = "FXAA";
                break;
            case 2:
                antialiasingText.text = "TXAA";
                break;
            case 3:
                antialiasingText.text = "SMAA";
                break;
            default:
                antialiasingText.text = "OFF";
                break;
        }

    }

    private void SetAntialiasingOnBackButton(int index)
    {
        switch (index)
        {
            case 0:
                antialiasingText.text = "OFF";
                break;
            case 1:
                antialiasingText.text = "FXAA";
                break;
            case 2:
                antialiasingText.text = "TXAA";
                break;
            case 3:
                antialiasingText.text = "SMAA";
                break;
            default:
                antialiasingText.text = "OFF";
                break;
        }
    }

    private void SetTogglesOnApply()
    {
        auxIsFullScreen = isFullscreen;
        auxIsVsyncOn = isVsyncOn;
        Screen.fullScreen = isFullscreen;
        QualitySettings.vSyncCount = isVsyncOn ? 1 : 0;
    }

    private void SetAntialiasing(int index)
    {
        switch (index)
        {
            case 0:
                antialiasingText.text = "OFF";
                //hdac.antialiasing = HDAdditionalCameraData.AntialiasingMode.None;
                break;
            case 1:
                antialiasingText.text = "FXAA";
                //hdac.antialiasing = HDAdditionalCameraData.AntialiasingMode.FastApproximateAntialiasing;
                break;
            case 2:
                antialiasingText.text = "TXAA";
                //hdac.antialiasing = HDAdditionalCameraData.AntialiasingMode.TemporalAntialiasing;
                break;
            case 3:
                antialiasingText.text = "SMAA";
                //hdac.antialiasing = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                break;
            default:
                antialiasingText.text = "OFF";
                //hdac.antialiasing = HDAdditionalCameraData.AntialiasingMode.None;
                break;
        }
    }

    private void GetResolutions()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.Equals(currentRefreshRate))
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio + "Hz";
            ResolutionOption.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
                auxResolutionIndex = i;
                resolutionText.text = resolutionOption;
            }
        }

    }

    public void ChangeResolution(int index)
    {

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (auxResolutionIndex == i)
            {
                if (auxResolutionIndex + index < filteredResolutions.Count && auxResolutionIndex + index >= 0)
                {
                    ChangeResolutionIndex(auxResolutionIndex + index);
                    break;
                }

            }
        }
    }

    private void ChangeResolutionIndex(int resolutionIndex)
    {
        auxResolutionIndex = resolutionIndex;
        string resolutionOption = filteredResolutions[auxResolutionIndex].width + "x" + filteredResolutions[auxResolutionIndex].height + " " + filteredResolutions[auxResolutionIndex].refreshRateRatio + "Hz";
        resolutionText.text = resolutionOption;
    }

    private void SetResolution(int resolutionIndex)
    {
        auxResolutionIndex = resolutionIndex;
        Resolution resolution = filteredResolutions[resolutionIndex];
        currentResolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
        string resolutionOption = filteredResolutions[resolutionIndex].width + "x" + filteredResolutions[resolutionIndex].height + " " + filteredResolutions[resolutionIndex].refreshRateRatio + "Hz";
        resolutionText.text = resolutionOption;
        MainMenuController m_MainMenu = GetComponentInParent<MainMenuController>();
        m_MainMenu.OpenOptions();
    }

}
