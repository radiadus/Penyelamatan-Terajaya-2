using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Stats mage, warrior, assassin;
    public Equipment pedang, keris, tongkat;
    public QuestionReader reader;
    public AudioMixer mixer;
    public GameObject loadingCanvas;
    public Image loadingBackground;
    public TextMeshProUGUI loadingText;
    public Dictionary<string, Vector3> sceneGameOverSpawn = new Dictionary<string, Vector3>();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(loadingCanvas);
    }

    private void Start()
    {
        sceneGameOverSpawn.Add("Forest Overworld", new Vector3());
        sceneGameOverSpawn.Add("Mountain Overworld", new Vector3());
        sceneGameOverSpawn.Add("Terajaya Destroyed", new Vector3());
        SceneManager.LoadScene("Main Menu");
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
        Time.timeScale = 0f;
        loadingCanvas.SetActive(true);
        int buildIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
        Debug.Log(buildIndex);
        StartCoroutine(ChangeScene(sceneName, done =>
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = spawnPosition;
            player.GetComponent<CharacterController>().enabled = true;
            SaveGame(player);
            Time.timeScale = 1f;
            loadingCanvas.SetActive(false);
        }));
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
        ResetProgress();
        SceneManager.LoadScene("Forest Overworld");
        Time.timeScale = 1f;
    }
    private void ResetProgress()
    {
        inventory.emptyInventory();
        mage.Reset();
        warrior.Reset();
        assassin.Reset();
        pedang.Reset();
        keris.Reset();
        tongkat.Reset();
        float master = PlayerPrefs.GetFloat("Master", 1f);
        float sfx = PlayerPrefs.GetFloat("SFX", 1f);
        float music = PlayerPrefs.GetFloat("Music", 1f);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("Master", master);
        PlayerPrefs.SetFloat("SFX", sfx);
        PlayerPrefs.SetFloat("Music", music);
    }

    public void Continue()
    {
        if (!PlayerPrefs.HasKey("posX")) return;
        PlayerPrefs.SetInt("warrior2", 1);
        mage.InitializeSkills();
        warrior.InitializeSkills();
        assassin.InitializeSkills();
        Time.timeScale = 1f;
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

    public void ExitToMainMenu()
    {
        StartCoroutine(ChangeScene("Main Menu", done =>
        {

        }));
    }

    IEnumerator ChangeScene(string sceneName, Action<bool> done)
    {
        float a = 0;
        Color colorBG = loadingBackground.color;
        Color colorText = loadingText.color;
        while (a < 1)
        {
            a += 0.1f;
            colorBG.a = a;
            colorText.a = a;
            loadingBackground.color = colorBG;
            loadingText.color = colorText;
            yield return null;
        }
        colorBG.a = 1;
        colorText.a = 1;
        loadingBackground.color = colorBG;
        loadingText.color = colorText;
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        while (!load.isDone)
        {
            yield return null;
        }
        a = 1;
        while (a > 0)
        {
            a -= 0.1f;
            colorBG.a = a;
            colorText.a = a;
            loadingBackground.color = colorBG;
            loadingText.color = colorText;
            yield return null;
        }
        colorBG.a = 0;
        colorText.a = 0;
        loadingBackground.color = colorBG;
        loadingText.color = colorText;
        done(true);
    }

    IEnumerator LoadScene(string sceneName)
    {
        float a = 1;
        Color colorBG = loadingBackground.color;
        Color colorText = loadingText.color;
        while (a > 0)
        {
            a -= 0.1f;
            colorBG.a = a;
            colorText.a = a;
            loadingBackground.color = colorBG;
            loadingText.color = colorText;
            yield return null;
        }
        colorBG.a = 0;
        colorText.a = 0;
        loadingBackground.color = colorBG;
        loadingText.color = colorText;
    }
}
