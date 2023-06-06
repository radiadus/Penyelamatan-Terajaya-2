using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statue : Interactible
{
    public int id;
    private bool solved;
    private int answer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        answer = -1;
        textBoxButton = textBox.GetComponent<Button>();
    }

    protected override int CheckUsedText()
    {
        solved = PlayerPrefs.GetInt("statue_" + id, 0) == 1;
        return solved ? 1 : 0;
    }

    public override void Interact()
    {
        base.Interact();
        this.Solve();
    }

    private void Solve()
    {
        answer = -1;
        textBox.SetActive(true);
        //textPanel.text = question;
        //answerButton1234.SetActive(true);
        //answerButton1234.onClick...
        StartCoroutine(Answer());
        //if (correct){
            solved = true;
            PlayerPrefs.SetInt("statue_" + id, 1);
        //}
        textBox.SetActive(false);
    }

    IEnumerator Answer()
    {
        while (answer < 0)
        {
            yield return null;
        }
    }
}
