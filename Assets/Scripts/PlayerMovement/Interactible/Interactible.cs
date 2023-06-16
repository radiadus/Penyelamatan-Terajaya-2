using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactible : MonoBehaviour
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

    protected virtual void Start()
    {
        this.usedText = CheckUsedText();
        this.textBoxButton = textBox.GetComponent<Button>();
        this.text = DialogueReader.Instance.GetDialoguesByCodeAndId(dialogueCode, usedText);
        
    }

    protected virtual int CheckUsedText()
    {
        return 1;
    }

    public virtual void Interact()
    {
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

    public void ClosePanel()
    {
        GameManager.Instance.gameState = GameManager.State.DEFAULT;
        textBox.SetActive(false);
    }

    IEnumerator ShowText()
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
        while (GameManager.Instance.gameState == GameManager.State.INTERACT)
        {
            yield return null;
        }
    }
}
