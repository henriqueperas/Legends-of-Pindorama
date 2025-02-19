using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Serialized
    [SerializeField] private GameObject configCanvas;
    [SerializeField] private Button voltarBtn;
    [SerializeField] private Button salvarBtn;
    [SerializeField] private Button settingsBtn;


    [SerializeField] private Slider sliderVolume;
    [SerializeField] private Slider sliderSoundEffects;
    [SerializeField] private Slider sliderMusic;


    [SerializeField] private AudioManager audioManager;
    #endregion

    void Start()
    {
        configCanvas.SetActive(false);

        settingsBtn.onClick.AddListener(ShowConfig);
        voltarBtn.onClick.AddListener(HideConfig);
        // salvarBtn.onClick.AddListener(SaveGame);


        sliderVolume.onValueChanged.AddListener(SetVolume);
        sliderSoundEffects.onValueChanged.AddListener(SetSoundEffectsVolume);
        sliderMusic.onValueChanged.AddListener(SetMusicVolume);


        sliderVolume.value = audioManager.GetVolume();
        sliderSoundEffects.value = audioManager.GetSoundEffectsVolume();
        sliderMusic.value = audioManager.GetMusicVolume();
    }

    private void SetVolume(float volume)
    {
        audioManager.SetVolume(volume);
    }

    private void SetSoundEffectsVolume(float volume)
    {
        audioManager.SetSoundEffectsVolume(volume);
    }

    private void SetMusicVolume(float volume)
    {
        audioManager.SetMusicVolume(volume);
    }

    public void ShowConfig()
    {
        Time.timeScale = 0;
        configCanvas.SetActive(true);
    }

    public void HideConfig()
    {
        Time.timeScale = 1;
        configCanvas.SetActive(false);
    }
}