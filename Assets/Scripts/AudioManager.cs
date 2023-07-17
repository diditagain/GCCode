using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider backgroundMusicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider voiceVolumeSlider;
    [SerializeField] private AudioMixer mixer;

    private string _backgrounMusicVolume = "BackgroundMusicVolume";
    private string _sfxVolume = "SFXVolume";
    private string _voiceVolume = "VoiceVolume";
    private string _masterVolume = "MasterVolume";

    [SerializeField] private AudioSource backgrounAudioSource;
    [SerializeField] private AudioSource voiceNarrationAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    private AudioClip _backgroundMusic;

    public void SetBackgroundMusic(AudioClip backgroundMusic)
    {
        if (this._backgroundMusic == backgroundMusic)
            return;

        backgrounAudioSource.clip = backgroundMusic;
        backgrounAudioSource.Play();
    }

    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {        
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    

    private void OnEnable()
    {
        backgroundMusicVolumeSlider.onValueChanged.AddListener(OnBackgroundMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        voiceVolumeSlider.onValueChanged.AddListener(OnVoiceVolumeChanged);
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        mixer.GetFloat(_masterVolume, out float volume);
        masterVolumeSlider.SetValueWithoutNotify(volume);

        mixer.GetFloat(_backgrounMusicVolume, out volume);
        backgroundMusicVolumeSlider.SetValueWithoutNotify(volume);

        mixer.GetFloat(_sfxVolume, out volume);
        sfxVolumeSlider.SetValueWithoutNotify(volume);

        mixer.GetFloat(_voiceVolume, out volume);
        voiceVolumeSlider.SetValueWithoutNotify(volume);
    }

    public void PlaySFX(AudioClip gateOpeningSound)
    {
        sfxAudioSource.PlayOneShot(gateOpeningSound);
    }

    private void OnDisable()
    {
        backgroundMusicVolumeSlider.onValueChanged.RemoveListener(OnBackgroundMusicVolumeChanged);
    }

    private void OnBackgroundMusicVolumeChanged(float value)
    {
        mixer.SetFloat(_backgrounMusicVolume, value);
    }
    private void OnSFXVolumeChanged(float value)
    {
        mixer.SetFloat(_sfxVolume, value);
    }
    private void OnVoiceVolumeChanged(float value)
    {
        mixer.SetFloat(_voiceVolume, value);
    }
    private void OnMasterVolumeChanged(float value)
    {
        mixer.SetFloat(_masterVolume, value);
    }

    public void PlayVoiceClip(AudioClip voiceClip)
    {
        StopNarration();
        voiceNarrationAudioSource.PlayOneShot(voiceClip);
    }
    public void StopNarration()
    {
        voiceNarrationAudioSource.Stop();
    }

    public void SetVolume(float percent)
    {
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = percent;
        }
    }
}
