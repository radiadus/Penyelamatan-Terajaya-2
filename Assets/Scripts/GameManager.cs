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
        ENCOUNTER,
        MAIN_MENU,
        CUTSCENE
    }

    public State gameState;
    public Inventory inventory;
    public Stats mage, warrior, assassin;
    public Equipment pedang, keris, tongkat;
    public AudioMixer mixer;
    public GameObject loadingCanvas;
    public GameObject player;
    public Image loadingBackground;
    public TextMeshProUGUI loadingText;
    public Dictionary<string, Vector3> sceneGameOverSpawn = new Dictionary<string, Vector3>();
    public float autoSaveTimer;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(loadingCanvas);
    }

    private void Start()
    {
        sceneGameOverSpawn.Add("Forest Overworld", new Vector3(7.255f, 0.1f, -14.47f));
        sceneGameOverSpawn.Add("Mountain Overworld", new Vector3(-26.33f, 0.1f, -11.46f));
        sceneGameOverSpawn.Add("Terajaya Destroyed", new Vector3(-1.05f, 0.1f, -17.04f));
        autoSaveTimer = 0;
        gameState = State.MAIN_MENU;
        SceneManager.LoadScene("Main Menu");
        StartCoroutine(AutoSave());
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
        StartCoroutine(ChangeScene(sceneName, done =>
        {
            Time.timeScale = 1f;
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = spawnPosition;
            player.GetComponent<CharacterController>().enabled = true;
            SaveGame(player);
            loadingCanvas.SetActive(false);
        }));
    }

    public void SaveGame(GameObject player)
    {
        PlayerPrefs.SetInt("sceneId", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetFloat("posX", player.transform.position.x);
        PlayerPrefs.SetFloat("posY", player.transform.position.y);
        PlayerPrefs.SetFloat("posZ", player.transform.position.z);
        PlayerPrefs.SetInt("mage", mage.level);
        PlayerPrefs.SetInt("mageHP", mage.HP);
        PlayerPrefs.SetInt("mageMP", mage.MP);
        PlayerPrefs.SetInt("mageExp", mage.exp);
        PlayerPrefs.SetInt("warrior", warrior.level);
        PlayerPrefs.SetInt("warriorHP", warrior.HP);
        PlayerPrefs.SetInt("warriorMP", warrior.MP);
        PlayerPrefs.SetInt("warriorExp", warrior.exp);
        PlayerPrefs.SetInt("assassin", assassin.level);
        PlayerPrefs.SetInt("assassinHP", assassin.HP);
        PlayerPrefs.SetInt("assassinMP", assassin.MP);
        PlayerPrefs.SetInt("assassinExp", assassin.exp);
        PlayerPrefs.SetInt("money", inventory.money);
        PlayerPrefs.SetInt("pedang", pedang.enhanceLevel);
        PlayerPrefs.SetInt("keris", keris.enhanceLevel);
        PlayerPrefs.SetInt("tongkat", tongkat.enhanceLevel);
        ItemInstance hpPotion = inventory.FindItemInstance(typeof(Jamu));
        ItemInstance mpPotion = inventory.FindItemInstance(typeof(JamuEnergi));
        PlayerPrefs.SetInt("hpPotion", hpPotion == null ? 0 : hpPotion.quantity);
        PlayerPrefs.SetInt("mpPotion", mpPotion == null ? 0 : mpPotion.quantity);
    }

    public void NewGame()
    {
        ResetProgress();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Cutscene Awal");
        //AsyncOperation load = SceneManager.LoadSceneAsync("Forest Overworld");
        //load.completed += (asyncOperation) =>
        //{
        //    player = GameObject.FindGameObjectWithTag("Player");
        //    gameState = State.DEFAULT;
        //};
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
        if (!PlayerPrefs.HasKey("sceneId")) return;
        mage.InitializeSkills();
        warrior.InitializeSkills();
        assassin.InitializeSkills();
        inventory.money = PlayerPrefs.GetInt("money");
        inventory.addItem(Resources.Load<Item>("Items/Item_1"), PlayerPrefs.GetInt("hpPotion"));
        inventory.addItem(Resources.Load<Item>("Items/Item_2"), PlayerPrefs.GetInt("mpPotion"));
        Time.timeScale = 1f;
        AsyncOperation load = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("sceneId"));
        load.completed += (asyncOperation) =>
        {
            player = GameObject.FindGameObjectWithTag("Player");
            Vector3 position = new Vector3(PlayerPrefs.GetFloat("posX"), PlayerPrefs.GetFloat("posY"), PlayerPrefs.GetFloat("posZ"));
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = position;
            player.GetComponent<CharacterController>().enabled = true;
            if (Physics.Raycast(player.transform.position+new Vector3(0,1.8f,0), Vector3.up, out RaycastHit hit, 3, 1 << 6))
            {
                GameObject[] roofObjects = GameObject.FindGameObjectsWithTag("Roof");
                foreach (GameObject roofObject in roofObjects)
                {
                    roofObject.SetActive(false);
                }
            }
            gameState = State.DEFAULT;
        };
    }

    public void GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void UpdateSkills()
    {
        mage.InitializeSkills();
        assassin.InitializeSkills();
        warrior.InitializeSkills();
    }

    public void ExitToMainMenu()
    {
        gameState = State.MAIN_MENU;
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

    IEnumerator AutoSave()
    {
        while (true)
        {
            while (GameManager.Instance.gameState != GameManager.State.DEFAULT) yield return null;
            autoSaveTimer += Time.deltaTime;
            if (autoSaveTimer > 10)
            {
                SaveGame(player);
                autoSaveTimer = 0;
            }
            yield return null;
        }
    }
}
