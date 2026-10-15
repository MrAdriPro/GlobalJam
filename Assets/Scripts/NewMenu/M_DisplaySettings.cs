using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class M_DisplaySettings : MonoBehaviour
{
    public PersistentData data; // Se mantiene como referencia pero usamos PlayerPrefs para la Build

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
    private List<Resolution> filteredResolutions = new List<Resolution>();
    private List<string> ResolutionOption = new List<string>();
    private int currentResolutionIndex;
    private int auxResolutionIndex;

    private void Awake()
    {
        GetResolutions();
        LoadSettings(); // Carga real mediante PlayerPrefs
        OnSettingsOn();
    }

    private void LoadSettings()
    {
        // PlayerPrefs guarda la configuración en el registro/disco duro del jugador
        isFullscreen = PlayerPrefs.GetInt("PP_Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        isVsyncOn = PlayerPrefs.GetInt("PP_VSync", QualitySettings.vSyncCount > 0 ? 1 : 0) == 1;
        antialiasingValue = PlayerPrefs.GetInt("PP_AA", 0);

        auxIsFullScreen = isFullscreen;
        auxIsVsyncOn = isVsyncOn;

        QualitySettings.vSyncCount = isVsyncOn ? 1 : 0;
        SetAntialiasing(antialiasingValue);

        int savedWidth = PlayerPrefs.GetInt("PP_ResW", Screen.width);
        int savedHeight = PlayerPrefs.GetInt("PP_ResH", Screen.height);

        int resIndex = GetResolutionIndexByDimensions(savedWidth, savedHeight);
        if (resIndex != -1)
        {
            SetResolution(resIndex);
        }
        else if (filteredResolutions.Count > 0)
        {
            SetResolution(currentResolutionIndex);
        }
    }

    private void OnSettingsOn()
    {
        if (vSyncSlider != null)
        {
            vSyncSlider.value = isVsyncOn ? 1 : 0;
            VsyncToggle(isVsyncOn);
        }

        if (fullscreenSlider != null)
        {
            fullscreenSlider.value = isFullscreen ? 1 : 0;
            FullscreenToggle(isFullscreen);
        }

        Antialiasing(0);
    }

    public void OnApplyButton()
    {
        SetResolution(auxResolutionIndex);
        SetTogglesOnApply();
        SetAntialiasing(antialiasingValue);

        // Guardado persistente garantizado en la Build
        if (filteredResolutions.Count > 0 && currentResolutionIndex < filteredResolutions.Count)
        {
            PlayerPrefs.SetInt("PP_ResW", filteredResolutions[currentResolutionIndex].width);
            PlayerPrefs.SetInt("PP_ResH", filteredResolutions[currentResolutionIndex].height);
        }

        PlayerPrefs.SetInt("PP_VSync", isVsyncOn ? 1 : 0);
        PlayerPrefs.SetInt("PP_Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.SetInt("PP_AA", antialiasingValue);
        PlayerPrefs.Save();

        if (data != null && filteredResolutions.Count > 0)
        {
            data.vSync = isVsyncOn;
            data.fullScreen = isFullscreen;
            data.antialiasing = antialiasingValue;
            data.resolution = new CustomRes(
                filteredResolutions[currentResolutionIndex].width,
                filteredResolutions[currentResolutionIndex].height,
                filteredResolutions[currentResolutionIndex].refreshRateRatio.value
            );
        }
    }

    public void OnBackButton()
    {
        auxResolutionIndex = currentResolutionIndex;
        ChangeResolutionIndex(currentResolutionIndex);
        VsyncToggle(auxIsVsyncOn);
        FullscreenToggle(auxIsFullScreen);

        if (vSyncSlider != null) vSyncSlider.value = auxIsVsyncOn ? 1 : 0;
        if (fullscreenSlider != null) fullscreenSlider.value = auxIsFullScreen ? 1 : 0;
    }

    // Sobrecargas para permitir que el Slider de Unity envíe floats (0.0 - 1.0)
    public void VsyncToggle(float value) => VsyncToggle(value > 0.5f);
    
    public void VsyncToggle(bool value)
    {
        if (vSyncSlider != null)
        {
            var toggleSwitch = vSyncSlider.GetComponent<M_ToggleSwitch>();
            if (toggleSwitch != null) toggleSwitch.UpdateCurrentValue(value);
        }

        if (vSyncText != null) vSyncText.text = value ? "ON" : "OFF";
        isVsyncOn = value;
    }

    public void FullscreenToggle(float value) => FullscreenToggle(value > 0.5f);

    public void FullscreenToggle(bool value)
    {
        if (fullscreenSlider != null)
        {
            var toggleSwitch = fullscreenSlider.GetComponent<M_ToggleSwitch>();
            if (toggleSwitch != null) toggleSwitch.UpdateCurrentValue(value);
        }

        isFullscreen = value;
        if (fullscreenText != null) fullscreenText.text = value ? "ON" : "OFF";
    }

    public void Antialiasing(int index)
    {
        antialiasingValue = Mathf.Clamp(antialiasingValue + index, 0, 3);

        switch (antialiasingValue)
        {
            case 0: antialiasingText.text = "OFF"; break;
            case 1: antialiasingText.text = "FXAA"; break;
            case 2: antialiasingText.text = "TXAA"; break;
            case 3: antialiasingText.text = "SMAA"; break;
            default: antialiasingText.text = "OFF"; break;
        }
    }

    private void SetTogglesOnApply()
    {
        auxIsFullScreen = isFullscreen;
        auxIsVsyncOn = isVsyncOn;
        QualitySettings.vSyncCount = isVsyncOn ? 1 : 0;
    }

    private void SetAntialiasing(int index)
    {
        var pipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (pipelineAsset == null) return;

        switch (index)
        {
            case 0:
                if (antialiasingText != null) antialiasingText.text = "OFF";
                pipelineAsset.msaaSampleCount = 0;
                break;
            case 1:
                if (antialiasingText != null) antialiasingText.text = "FXAA";
                pipelineAsset.msaaSampleCount = 1;
                break;
            case 2:
                if (antialiasingText != null) antialiasingText.text = "TXAA";
                pipelineAsset.msaaSampleCount = 2;
                break;
            case 3:
                if (antialiasingText != null) antialiasingText.text = "SMAA";
                pipelineAsset.msaaSampleCount = 3;
                break;
            default:
                if (antialiasingText != null) antialiasingText.text = "OFF";
                pipelineAsset.msaaSampleCount = 0;
                break;
        }
    }

    private void GetResolutions()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        ResolutionOption = new List<string>();

        double currentHz = Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            // Tolerancia flexible para evitar que la lista quede vacía en la Build por decimales de Hz
            if (Mathf.Abs((float)(resolutions[i].refreshRateRatio.value - currentHz)) < 1.0f)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        // Seguro de vida: si no coincide ninguna tasa de refresco, añade todas para no romper el menú
        if (filteredResolutions.Count == 0)
        {
            filteredResolutions.AddRange(resolutions);
        }

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = $"{filteredResolutions[i].width}x{filteredResolutions[i].height} {Mathf.RoundToInt((float)filteredResolutions[i].refreshRateRatio.value)}Hz";
            ResolutionOption.Add(resolutionOption);

            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
                auxResolutionIndex = i;
                if (resolutionText != null) resolutionText.text = resolutionOption;
            }
        }
    }

    public void ChangeResolution(int index)
    {
        if (filteredResolutions.Count == 0) return;

        int newIndex = auxResolutionIndex + index;
        if (newIndex >= 0 && newIndex < filteredResolutions.Count)
        {
            ChangeResolutionIndex(newIndex);
        }
    }

    private int GetResolutionIndexByDimensions(int width, int height)
    {
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (filteredResolutions[i].width == width && filteredResolutions[i].height == height)
            {
                return i;
            }
        }
        return -1;
    }

    private void ChangeResolutionIndex(int resolutionIndex)
    {
        if (filteredResolutions.Count == 0 || resolutionIndex < 0 || resolutionIndex >= filteredResolutions.Count) return;

        auxResolutionIndex = resolutionIndex;
        string resolutionOption = $"{filteredResolutions[auxResolutionIndex].width}x{filteredResolutions[auxResolutionIndex].height} {Mathf.RoundToInt((float)filteredResolutions[auxResolutionIndex].refreshRateRatio.value)}Hz";
        if (resolutionText != null) resolutionText.text = resolutionOption;
    }

    private void SetResolution(int resolutionIndex)
    {
        if (filteredResolutions.Count == 0 || resolutionIndex < 0 || resolutionIndex >= filteredResolutions.Count) return;

        auxResolutionIndex = resolutionIndex;
        Resolution resolution = filteredResolutions[resolutionIndex];
        currentResolutionIndex = resolutionIndex;

        // Cambio directo a pantalla completa/ventana en la Build
        FullScreenMode mode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(resolution.width, resolution.height, mode);

        string resolutionOption = $"{resolution.width}x{resolution.height} {Mathf.RoundToInt((float)resolution.refreshRateRatio.value)}Hz";
        if (resolutionText != null) resolutionText.text = resolutionOption;

        MainMenuController m_MainMenu = GetComponentInParent<MainMenuController>();
        if (m_MainMenu != null) m_MainMenu.OpenOptions();
    }
}
