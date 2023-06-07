using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuHandler : MonoBehaviour
{
    public Button newGame, continueGame, settingsButton, exitButton, alertYes, alertNo, settingsExit;
    public GameObject exitAlert, settingsPanel;
    public Toggle muteToggle;
    public Slider master, effects, music;

    // Start is called before the first frame update
    void Start()
    {
        master.value = PlayerPrefs.GetFloat("Master", 1f);
        effects.value = PlayerPrefs.GetFloat("SFX", 1f);
        music.value = PlayerPrefs.GetFloat("Music", 1f);
        bool mute = PlayerPrefs.GetInt("mute", 0) == 1 ? true : false;
        muteToggle.isOn = mute;

        newGame.onClick.AddListener(delegate { NewGame(); });
        continueGame.onClick.AddListener(delegate { Continue(); });

        settingsButton.onClick.AddListener(delegate { OpenSettings(); });
        muteToggle.onValueChanged.AddListener(delegate { ToggleMute(); });
        master.onValueChanged.AddListener(delegate { ChangeVolume(master.value, "Master"); });
        effects.onValueChanged.AddListener(delegate { ChangeVolume(effects.value, "SFX"); });
        music.onValueChanged.AddListener(delegate { ChangeVolume(music.value, "Music"); });
        settingsExit.onClick.AddListener(delegate { ExitSettings(); });

        exitButton.onClick.AddListener(delegate { OpenAlert(); });
        alertYes.onClick.AddListener(delegate { Exit(); });
        alertNo.onClick.AddListener(delegate { CloseAlert(); });

        GameManager.Instance.ToggleMute(mute);
    }

    void ToggleMute()
    {
        GameManager.Instance.ToggleMute(muteToggle.isOn);
    }
    void ChangeVolume(float volume, string soundType)
    {
        Debug.Log(soundType + ": " + volume);
        PlayerPrefs.SetFloat(soundType, volume);
        GameManager.Instance.ChangeVolume(volume, soundType);
    }

    void OpenSettings()
    {
        newGame.gameObject.SetActive(false);
        continueGame.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        settingsPanel.SetActive(true);
    }

    void ExitSettings()
    {
        newGame.gameObject.SetActive(true);
        continueGame.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        settingsPanel.SetActive(false);
    }

    void OpenAlert()
    {
        exitAlert.SetActive(true);
    }

    void Exit()
    {
        Application.Quit();
    }

    void CloseAlert()
    {
        exitAlert.SetActive(false);
    }

    void NewGame()
    {
        GameManager.Instance.NewGame();
    }

    void Continue()
    {
        GameManager.Instance.Continue();
    }
}
