using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
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

        muteToggle.onValueChanged.AddListener(delegate { ToggleMute(); });
        master.onValueChanged.AddListener(delegate { ChangeVolume(master.value, "Master"); });
        effects.onValueChanged.AddListener(delegate { ChangeVolume(effects.value, "SFX"); });
        music.onValueChanged.AddListener(delegate { ChangeVolume(music.value, "Music"); });
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
}
