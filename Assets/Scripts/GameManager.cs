using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum State
    {
        DEFAULT,
        INTERACT,
        SHOPPING,
        ENCOUNTER
    }

    public State gameState;
    public Inventory inventory;
    //public Mage mage;
    //public Swordsman swordsman;
    //public Assassin assassin;
    public Stats mage, swordsman, assassin;
    public Equipment pedang, keris, tongkat;
    public QuestionReader reader;
    public AudioMixer mixer;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume(float master, float sfx, float music)
    {
        ChangeVolume(master, "Master");
        ChangeVolume(sfx, "SFX");
        ChangeVolume(music, "Music");
    }

    public void ChangeVolume(float volume, string soundType)
    {
        if (PlayerPrefs.GetInt("mute", 0) != 1) mixer.SetFloat(soundType, MathF.Log10(volume) * 20);
    }

    public void ToggleMute(bool mute)
    {
        PlayerPrefs.SetInt("mute", mute ? 1 : 0);
        switch (mute)
        {
            case true:
                Mute();
                break;
            case false:
                float master = PlayerPrefs.GetFloat("Master", 1);
                float sfx = PlayerPrefs.GetFloat("SFX", 1);
                float music = PlayerPrefs.GetFloat("Music", 1);
                SetVolume(master, sfx, music);
                break;
        }
    }

    public void Mute()
    {
        mixer.SetFloat("Master", MathF.Log10(0.001f) * 20);
        mixer.SetFloat("SFX", MathF.Log10(0.001f) * 20);
        mixer.SetFloat("Music", MathF.Log10(0.001f) * 20);
    }

    public void ChangeMap(string sceneName, Vector3 spawnPosition)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        load.completed += (asyncOperation) =>
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = spawnPosition;
            player.GetComponent<CharacterController>().enabled = true;
            SaveGame(player);
        };
    }

    public void SaveGame(GameObject player)
    {
        PlayerPrefs.SetInt("sceneId", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetFloat("posX", player.transform.position.x);
        PlayerPrefs.SetFloat("posY", player.transform.position.y);
        PlayerPrefs.SetFloat("posZ", player.transform.position.z);
        Debug.Log(PlayerPrefs.GetFloat("posX") + " " + PlayerPrefs.GetFloat("posY") + " " + PlayerPrefs.GetFloat("posZ"));
    }

    public void NewGame()
    {
        inventory.emptyInventory();
        mage.Reset();
        swordsman.Reset();
        assassin.Reset();
        pedang.Reset();
        keris.Reset();
        tongkat.Reset();
        PlayerPrefs.SetInt("end", 0);
        SceneManager.LoadScene("Forest Overworld");
    }

    public void Continue()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("sceneId"));
        load.completed += (asyncOperation) =>
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 position = new Vector3(PlayerPrefs.GetFloat("posX"), PlayerPrefs.GetFloat("posY"), PlayerPrefs.GetFloat("posZ"));
            Debug.Log(PlayerPrefs.GetFloat("posX") + " " + PlayerPrefs.GetFloat("posY") + " " + PlayerPrefs.GetFloat("posZ"));
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = position;
            player.GetComponent<CharacterController>().enabled = true;
        };
    }

}
