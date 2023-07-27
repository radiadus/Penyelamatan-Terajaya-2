using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuHandler : MonoBehaviour
{
    public Button newGame, continueGame, settingsButton, tutorialButton, exitButton, alertYes, alertNo, settingsExit, confirmationNo, confirmationYes;
    public GameObject exitAlert, settingsPanel, tutorialCanvas, newGameConfirmation;
    public GameObject[] tutorialImages;
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
        confirmationNo.onClick.AddListener(delegate { newGameConfirmation.SetActive(false); });
        confirmationYes.onClick.AddListener(delegate { GameManager.Instance.NewGame(); });
        continueGame.onClick.AddListener(delegate { Continue(); });

        settingsButton.onClick.AddListener(delegate { OpenSettings(); });
        muteToggle.onValueChanged.AddListener(delegate { ToggleMute(); });
        master.onValueChanged.AddListener(delegate { ChangeVolume(master.value, "Master"); });
        effects.onValueChanged.AddListener(delegate { ChangeVolume(effects.value, "SFX"); });
        music.onValueChanged.AddListener(delegate { ChangeVolume(music.value, "Music"); });
        tutorialButton.onClick.AddListener(delegate { Tutorial(); });
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

    void Tutorial()
    {
        foreach (GameObject tutorial in tutorialImages)
        {
            tutorial.SetActive(true);
        }
        tutorialCanvas.SetActive(true);
        StartCoroutine(TutorialUpdate());
    }
    public IEnumerator TutorialUpdate()
    {
        for (int i = 0; i < tutorialImages.Length; i++)
        {
            yield return null;
            while (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Began) yield return null;
            tutorialImages[i].SetActive(false);
            yield return null;
        }
        tutorialCanvas.SetActive(false);
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
        Debug.Log(PlayerPrefs.HasKey("sceneId"));
        if (PlayerPrefs.GetInt("sceneId", 0) != 0)
        {
            newGameConfirmation.SetActive(true);
            return;
        }
        GameManager.Instance.NewGame();
    }

    void Continue()
    {
        GameManager.Instance.Continue();
    }
}
