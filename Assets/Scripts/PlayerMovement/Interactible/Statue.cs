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
    }

    protected override int CheckUsedText()
    {
        solved = PlayerPrefs.GetInt(statueCode, 0) == 1;
        return solved ? 1 : 0;
    }

    public override void Interact()
    {
        base.Interact();
        if (!solved)
        {
            GameManager.Instance.gameState = GameManager.State.INTERACT;
            this.Solve();
        }
    }

    private void Solve()
    {
        questionCanvas.SetActive(true);
        questionText.text = question.question;
        for (int i = 0; i < question.answerCount; i++)
        {
            int index = i;
            answerButton[index].gameObject.SetActive(true);
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
            StartCoroutine(ShowResult(true));
            GameManager.Instance.UpdateSkills();
            return;
        }
        StartCoroutine(ShowResult(false));

    }

    IEnumerator ShowResult(bool correct)
    {
        if (correct) correctImage.SetActive(true);
        else incorrectImage.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        questionCanvas.SetActive(false);
        GameManager.Instance.gameState = GameManager.State.DEFAULT;
    }
}
