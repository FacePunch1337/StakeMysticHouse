using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public struct SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }

    public SoundAudioClip[] soundAudioClips;

    private Dictionary<Sound, AudioSource> audioSources;
    private Dictionary<Sound, AudioClip> audioClips;

    private float musicVolume = 0.0f;
    private float overallVolume = 1.0f;
    private bool isSoundEnabled = true;

    public enum Sound
    {
        Soundtrack,
        LilithVoice1,
        LilithVoice2,
        ClickSound,
        CraftSound,
        BrewSound,
        GrabSound,
        PlaceSound,
        BuySound,
        SellSound,
        RewardSound
    }

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSources = new Dictionary<Sound, AudioSource>();
        audioClips = new Dictionary<Sound, AudioClip>();

        foreach (SoundAudioClip soundAudioClip in soundAudioClips)
        {
            GameObject soundGameObject = new GameObject("Sound_" + soundAudioClip.sound);
            soundGameObject.transform.SetParent(transform);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = soundAudioClip.audioClip;
            audioSources[soundAudioClip.sound] = audioSource;
            audioClips[soundAudioClip.sound] = soundAudioClip.audioClip;

            // Loop the soundtrack
            if (soundAudioClip.sound == Sound.Soundtrack)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        UpdateVolumes();
    }

    public void PlaySound(Sound sound)
    {
        if (isSoundEnabled && audioSources.ContainsKey(sound))
        {
            audioSources[sound].Play();
        }
        else
        {
            Debug.LogWarning("Sound not found: " + sound);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateVolumes();
    }

    public void SetOverallVolume(float volume)
    {
        overallVolume = volume;
        UpdateVolumes();
    }

    public void SetSoundEnabled(bool isEnabled)
    {
        isSoundEnabled = isEnabled;
        foreach (var source in audioSources.Values)
        {
            source.mute = !isSoundEnabled;
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetOverallVolume()
    {
        return overallVolume;
    }

    public bool IsSoundEnabled()
    {
        return isSoundEnabled;
    }

    private void UpdateVolumes()
    {
        foreach (var pair in audioSources)
        {
            pair.Value.volume = overallVolume;
            if (pair.Key == Sound.Soundtrack)
            {
                pair.Value.volume *= musicVolume;
            }
        }
    }
}
