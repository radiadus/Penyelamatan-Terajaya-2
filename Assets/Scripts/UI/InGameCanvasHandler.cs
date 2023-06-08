using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameCanvasHandler : MonoBehaviour
{
    public GameObject player, playerCanvas;
    private bool paused;
    [SerializeField] private Button pauseButton, inventoryButton, settingsButton, exitButton, closeInventory, closeSettings;
    [SerializeField] private GameObject backdrop, inventoryPanel, settingsPanel, characterStats;
    [SerializeField] private Stats[] stats;
    [SerializeField] private TextMeshProUGUI[] level, HP, MP, attack, defense;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        pauseButton.onClick.AddListener(delegate { TogglePause(); });
        inventoryButton.onClick.AddListener(delegate { OpenInventory(); });
        settingsButton.onClick.AddListener(delegate { OpenSettings(); });
        exitButton.onClick.AddListener(delegate { Exit(); });
        closeInventory.onClick.AddListener(delegate { CloseInventory(); });
        closeSettings.onClick.AddListener(delegate { CloseSettings(); });
        for (int i = 0; i < stats.Length; i++)
        {
            level[i].text = stats[i].level.ToString();
            HP[i].text = stats[i].HP + "/" + stats[i].maxHP;
            MP[i].text = stats[i].MP + "/" + stats[i].maxMP;
            attack[i].text = stats[i].attack.ToString();
            defense[i].text = stats[i].defense.ToString();
        }
    }

    public void TogglePause()
    {
        if (paused) Unpause();
        else Pause();
        paused = !paused;
    }

    public void Pause()
    {
        backdrop.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        characterStats.gameObject.SetActive(true);
        playerCanvas.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Unpause()
    {
        backdrop.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        characterStats.gameObject.SetActive(false);
        if (GameManager.Instance.gameState == GameManager.State.DEFAULT)
            playerCanvas.SetActive(true);
        Time.timeScale = 1.0f;
    }

    void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        characterStats.gameObject.SetActive(false);
    }

    void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        characterStats.gameObject.SetActive(true);
    }

    void OpenSettings()
    {
        settingsPanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        characterStats.gameObject.SetActive(false);
    }

    void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        characterStats.gameObject.SetActive(true);
    }

    void Exit()
    {
        GameManager.Instance.SaveGame(player);
        Application.Quit();
        Debug.Log("quitted");
    }
}
