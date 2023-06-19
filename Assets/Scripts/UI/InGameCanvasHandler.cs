using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameCanvasHandler : MonoBehaviour
{
    public GameObject player, playerCanvas, itemUsePanel;
    public GameObject[] itemUserIndicator;
    private bool paused;
    public Text hpPotionCountText, mpPotionCountText;
    public Button hpPotionButton, mpPotionButton, cancelUseItem, confirmUseItem;
    public Button[] chooseItemUser;
    [SerializeField] private Button pauseButton, inventoryButton, settingsButton, exitButton, closeInventory, closeSettings;
    [SerializeField] private GameObject backdrop, inventoryPanel, settingsPanel, characterStats;
    [SerializeField] private Stats[] stats;
    [SerializeField] private TextMeshProUGUI[] level, HP, MP, attack, defense;
    private int hpPotionCount, mpPotionCount;

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
        cancelUseItem.onClick.AddListener(delegate { ResetItemUser(); itemUsePanel.SetActive(false); });
        InitializeStats();
    }

    public void InitializeStats()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            level[i].text = stats[i].level.ToString();
            HP[i].text = stats[i].HP + "/" + stats[i].maxHP;
            MP[i].text = stats[i].MP + "/" + stats[i].maxMP;
            attack[i].text = (stats[i].attack + stats[i].equipment.attackStat).ToString();
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
        InitializeStats();
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
        InitializeInventory();
    }

    void InitializeInventory()
    {
        ItemInstance hpPotion = GameManager.Instance.inventory.FindItemInstance(typeof(Jamu));
        ItemInstance mpPotion = GameManager.Instance.inventory.FindItemInstance(typeof(JamuEnergi));
        hpPotionCount = hpPotion != null ? hpPotion.quantity : 0;
        mpPotionCount = mpPotion != null ? mpPotion.quantity : 0;
        hpPotionCountText.text = hpPotionCount.ToString();
        mpPotionCountText.text = mpPotionCount.ToString();
        hpPotionButton.onClick.RemoveAllListeners();
        hpPotionButton.onClick.AddListener(delegate
        {
            if (hpPotionCount > 0)
            {
                OpenItemUsePanel(hpPotion.item);
            }
        });
        mpPotionButton.onClick.RemoveAllListeners();
        mpPotionButton.onClick.AddListener(delegate
        {
            if (mpPotionCount > 0)
            {
                OpenItemUsePanel(mpPotion.item);
            }
        });
    }

    void OpenItemUsePanel(Item item)
    {
        itemUsePanel.SetActive(true);
        confirmUseItem.onClick.RemoveAllListeners();
        for (int i = 0; i < chooseItemUser.Length; i++)
        {
            int index = i;
            chooseItemUser[index].onClick.RemoveAllListeners();
            chooseItemUser[index].onClick.AddListener(delegate
            {
                ResetItemUser();
                itemUserIndicator[index].SetActive(true);
            });
        }
        confirmUseItem.onClick.AddListener(delegate
        {
            ((Consumable)item).UseInventory(GetItemUser());
            ResetItemUser();
            itemUsePanel.SetActive(false);
            InitializeInventory();
            InitializeStats();
        });
    }

    Stats GetItemUser()
    {
        List<Stats> targets = new List<Stats>();
        for (int i = 0; i < itemUserIndicator.Length; i++)
        {
            Debug.Log(itemUserIndicator[i].activeSelf);
            if (itemUserIndicator[i].activeSelf)
            {
                targets.Add(stats[i]);
            }
        }
        return targets[0];
    }

    void ResetItemUser()
    {
        foreach(GameObject arrow in itemUserIndicator)
        {
            arrow.SetActive(false);
        }
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
        GameManager.Instance.ExitToMainMenu();
        Debug.Log("quitted");
    }
}
