using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public Image[] images;
    public string textCode, targetScene;
    public List<string> text;
    public Button button;
    private enum CutsceneState
    {
        WAITING,
        PRESSED
    }
    private CutsceneState state;
    private void Start()
    {
        state = CutsceneState.WAITING;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { StartCoroutine(Continue()); });
        text = DialogueReader.Instance.GetDialoguesByCodeAndId(textCode, 1);
        StartCoroutine(CutsceneControl());
    }

    IEnumerator Continue()
    {
        Debug.Log("kepencet");
        state = CutsceneState.PRESSED;
        yield return new WaitForEndOfFrame();
        state = CutsceneState.WAITING;
    }

    IEnumerator CutsceneControl()
    {
        for (int i = 0; i < images.Length; i++)
        {
            Image image = images[i];
            image.gameObject.SetActive(true);
            textUI.text = text[i];
            float a = 0;
            Color colorBG = image.color;
            Color colorText = textUI.color;
            while (a < 1)
            {
                a += 0.01f;
                colorBG.a = a;
                colorText.a = a;
                image.color = colorBG;
                textUI.color = colorText;
                yield return new WaitForEndOfFrame();
            }
            colorBG.a = 1;
            colorText.a = 1;
            image.color = colorBG;
            textUI.color = colorText;
            while (state == CutsceneState.WAITING) yield return null;
            textUI.text = "";
            a = 1;
            while (a > 0)
            {
                a -= 0.01f;
                colorBG.a = a;
                colorText.a = a;
                image.color = colorBG;
                textUI.color = colorText;
                yield return new WaitForEndOfFrame();
            }
            colorBG.a = 0;
            colorText.a = 0;
            image.color = colorBG;
            textUI.color = colorText;
            image.gameObject.SetActive(false);
            yield return null;
        }
        GameManager.Instance.gameState = GameManager.State.DEFAULT;
        AsyncOperation load = SceneManager.LoadSceneAsync(targetScene);
        load.completed += (asyncOperation) =>
        {
            GameManager.Instance.GetPlayer();
        };
    }
}
