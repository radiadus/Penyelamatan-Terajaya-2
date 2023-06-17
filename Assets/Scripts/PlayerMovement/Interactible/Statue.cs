using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statue : Interactible
{
    public int id;
    public string statueCode;
    private bool solved;
    private bool textBoxDone;
    private Question question;
    public GameObject questionCanvas, correctImage, incorrectImage;
    public Button[] answerButton;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] answerText;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        question = QuestionReader.Instance.GetStatueQuestion(id);
        textBoxDone = false;
    }

    protected override int CheckUsedText()
    {
        solved = PlayerPrefs.GetInt(statueCode, 0) == 1;
        return solved ? 2 : 1;
    }

    public override void Interact()
    {
        base.Interact();
    }

    private void Solve()
    {
        questionCanvas.SetActive(true);
        questionText.text = question.question;
        for (int i = 0; i < question.answerCount; i++)
        {
            int index = i;
            answerButton[index].gameObject.SetActive(true);
            answerText[index].text = question.answer[index];
            answerButton[index].onClick.RemoveAllListeners();
            answerButton[index].onClick.AddListener(delegate { AnswerQuestion(index); });
        }
        textBox.SetActive(false);
    }

    private void AnswerQuestion(int answer)
    {
        bool correct = answer == question.key - 'A';
        if (correct)
        {
            PlayerPrefs.SetInt(statueCode, 1);
            solved = true;
            usedText = 2;
            StartCoroutine(ShowResult(true));
            GameManager.Instance.UpdateSkills();
            return;
        }
        StartCoroutine(ShowResult(false));
    }
    public override void ClosePanel()
    {
        textBox.SetActive(false);
        textBoxDone = true;
    }

    protected override IEnumerator ShowText()
    {
        yield return base.ShowText();
        while (!textBoxDone) yield return null;
        if (!solved)
        {
            Solve();
        }
        else
        {
            GameManager.Instance.gameState = GameManager.State.DEFAULT;
            Time.timeScale = 1f;
        }
        textBoxDone = false;

    }

    IEnumerator ShowResult(bool correct)
    {
        if (correct) correctImage.SetActive(true);
        else incorrectImage.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        correctImage.SetActive(false);
        incorrectImage.SetActive(false);
        questionCanvas.SetActive(false);
        GameManager.Instance.gameState = GameManager.State.DEFAULT;
        Time.timeScale = 1f;
    }
}
