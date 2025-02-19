using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource; // Fonte de áudio da música
    [SerializeField] AudioSource soundEffectsSource; // Fonte de áudio dos efeitos sonoros

    private float volume = 1f;
    private float soundEffectsVolume = 1f;
    private float musicVolume = 1f;

    public void SetVolume(float volume)
    {
        this.volume = volume;
        UpdateAudioVolumes();
    }

    public void SetSoundEffectsVolume(float volume)
    {
        this.soundEffectsVolume = volume;
        UpdateAudioVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        this.musicVolume = volume;
        UpdateAudioVolumes();
    }

    public float GetVolume() { return volume; }
    public float GetSoundEffectsVolume() { return soundEffectsVolume; }
    public float GetMusicVolume() { return musicVolume; }

    private void UpdateAudioVolumes()
    {
        musicSource.volume = musicVolume * volume; // Ajusta a música de acordo com o volume
        soundEffectsSource.volume = soundEffectsVolume * volume; // Ajusta os efeitos sonoros
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null)
        {
            soundEffectsSource.PlayOneShot(clip, soundEffectsVolume * volume);
        }
    }
}