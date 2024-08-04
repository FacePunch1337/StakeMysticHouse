using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider overallVolumeSlider;
    public Toggle soundToggle;
    public Sprite soundOn;
    public Sprite mute;

    public Image soundToggleImage; // Assign this in the Unity Editor

    void Start()
    {
        // Initialize UI elements with current settings
        musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
        overallVolumeSlider.value = AudioManager.Instance.GetOverallVolume();
        soundToggle.isOn = AudioManager.Instance.IsSoundEnabled();

        // Set the initial sprite based on the current sound setting
        UpdateSoundToggleSprite(soundToggle.isOn);

        // Add listeners to handle changes
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        overallVolumeSlider.onValueChanged.AddListener(SetOverallVolume);
        soundToggle.onValueChanged.AddListener(ToggleSound);
    }

    void SetMusicVolume(float volume)
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        AudioManager.Instance.SetMusicVolume(volume);
    }

    void SetOverallVolume(float volume)
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        AudioManager.Instance.SetOverallVolume(volume);
    }

    void ToggleSound(bool isEnabled)
    {
        AudioManager.Instance.PlaySound(AudioManager.Sound.ClickSound);
        AudioManager.Instance.SetSoundEnabled(isEnabled);
        UpdateSoundToggleSprite(isEnabled);
    }

    void UpdateSoundToggleSprite(bool isEnabled)
    {
        if (isEnabled)
        {
            soundToggleImage.sprite = soundOn;
        }
        else
        {
            soundToggleImage.sprite = mute;
        }
    }
}
