using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject[] tutorialImages;
    public string targetScene;
    private int i;

    private void Start()
    {
        i = 0;

    }
    private void Update()
    {
        if (Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            i++;
            if (i < tutorialImages.Length)
                tutorialImages[i-1].SetActive(false);
        }
        if (i == tutorialImages.Length)
        {
            i++;
            AsyncOperation load = SceneManager.LoadSceneAsync(targetScene);
            load.completed += (asyncOperation) =>
            {
                GameManager.Instance.GetPlayer();
            };
        }
    }


}
