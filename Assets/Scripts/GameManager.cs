using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeVolume(float volume, string soundType)
    {
        PlayerPrefs.SetFloat(soundType, volume);
        AudioListener.volume = volume;
    }

    public void ToggleMute(bool mute)
    {
        PlayerPrefs.SetInt("mute", mute ? 1 : 0);
    }

    public void ChangeMap(string sceneName, Vector3 spawnPosition)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        load.completed += (asyncOperation) =>
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPosition;
            SaveGame(player);
        };
    }

    public void SaveGame(GameObject player)
    {
        PlayerPrefs.SetInt("sceneId", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetFloat("posX", player.transform.position.x);
        //easter egg
        PlayerPrefs.SetFloat("posY", player.transform.position.y);
        PlayerPrefs.SetFloat("posZ", player.transform.position.z);
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
            player.transform.position = position;
        };
    }

}
