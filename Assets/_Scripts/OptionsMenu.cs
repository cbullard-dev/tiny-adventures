using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;


public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle hardcoreToggle,fullscreenToggle;
    [SerializeField] private TMP_Text volumeNumberText;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] _resolutions;
    private void OnEnable()
    {
        _resolutions = Screen.resolutions;
        volumeSlider.value = (AudioListener.volume*100);
        hardcoreToggle.isOn = GameManager.Instance.IsHardcore;
        fullscreenToggle.isOn = Screen.fullScreen;
        volumeNumberText.text = volumeSlider.value.ToString();
        
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i ++ )
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            resolutionOptions.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    
    public void AdjustVolume(float volume)
    {
        AudioListener.volume = (volume/100);
        volumeNumberText.text = volume.ToString();
    }

    public void ToggleHardcore(bool isHardcore)
    {
        GameManager.Instance.IsHardcore = isHardcore;
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }
}
