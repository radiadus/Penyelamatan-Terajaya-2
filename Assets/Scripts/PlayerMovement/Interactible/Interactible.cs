using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Interactible : MonoBehaviour
{
    public List<string> text;
    public new string name;
    public string dialogueCode;
    public GameObject textBox;
    public TextMeshProUGUI textPanel;
    public TextMeshProUGUI namePanel;
    protected Button textBoxButton;
    protected int usedText;
    protected int currentPage;
    public int defaultTextId;

    protected virtual void Start()
    {
        //this.usedText = defaultTextId == 0 ? CheckUsedText() : defaultTextId;
        this.textBoxButton = textBox.GetComponent<Button>();
    }

    protected virtual int CheckUsedText()
    {
        return 1;
    }

    public virtual void Interact()
    {
        Time.timeScale = 0f;
        this.usedText = defaultTextId == 0 ? CheckUsedText() : defaultTextId;
        this.text = DialogueReader.Instance.GetDialoguesByCodeAndId(dialogueCode, usedText);
        textBox.SetActive(true);
        namePanel.text = name;
        currentPage = 0;
        StartCoroutine(ShowText());
        return;
    }

    public void ShowPage(int maxPage)
    {
        if (currentPage == maxPage)
        {
            return;
        }
        textPanel.text = text[currentPage];
        currentPage++;
    }

    public virtual void ClosePanel()
    {
        textBox.SetActive(false);
        GameManager.Instance.gameState = GameManager.State.DEFAULT;
        Time.timeScale = 1f;
    }

    protected virtual IEnumerator ShowText()
    {
        int maxPage = text.Count;
        ShowPage(maxPage);
        while(currentPage < maxPage)
        {
            textBoxButton.onClick.RemoveAllListeners();
            textBoxButton.onClick.AddListener(delegate { ShowPage(maxPage); });
            yield return null;
        }
        textBoxButton.onClick.RemoveAllListeners();
        textBoxButton.onClick.AddListener(delegate { ClosePanel(); });
    }
}
