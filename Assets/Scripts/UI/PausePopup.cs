using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePopup : MonoBehaviour
{
    [SerializeField] private Slider m_musicVolumeSlider;
    [SerializeField] private Slider m_SFXVolumeSlider;


    private void Start()
    {
        m_musicVolumeSlider.onValueChanged.AddListener(OnVolumeChange);
        m_SFXVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChange);
    }

    private void OnVolumeChange(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnSFXVolumeChange(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    private void OnEnable()
    {
        Time.timeScale = 1f;

        m_musicVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
        m_SFXVolumeSlider.SetValueWithoutNotify(AudioManager.Instance.GetSFXVolume());
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }


    public void QuitToMenuClicked()
    {
        SceneController.Instance.LoadScene(Scenes.Menu);
    }

    public void QuitToWindowsClicked()
    {
        Application.Quit();
    }
}
