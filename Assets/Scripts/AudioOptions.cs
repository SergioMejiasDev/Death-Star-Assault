using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Script from which the sound settings are configured.
/// </summary>
public class AudioOptions : MonoBehaviour
{
    [SerializeField] Slider musicSlider = null, sfxSlider = null;
    [SerializeField] AudioMixerGroup musicMixer = null, sfxMixer = null;
    float musicVolume, sfxVolume;

    private void Start()
    {
        LoadOptions();
    }

    private void Update()
    {
        musicVolume = musicSlider.value;
        sfxVolume = sfxSlider.value;

        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 25);
        sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 25);
    }

    /// <summary>
    /// Function that saves sound settings in PlayerPrefs.
    /// </summary>
    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Function that loads the sound settings of the PlayerPrefs.
    /// </summary>
    void LoadOptions()
    {
        float musicVolumeLoaded = PlayerPrefs.GetFloat("MusicVolume", 1);
        musicSlider.value = musicVolumeLoaded;

        float sfxVolumeLoaded = PlayerPrefs.GetFloat("SFXVolume", 1);
        sfxSlider.value = sfxVolumeLoaded;
    }
}



