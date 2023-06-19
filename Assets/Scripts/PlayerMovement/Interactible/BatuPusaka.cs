using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BatuPusaka : Interactible
{
    public override void Interact()
    {
        PlayerPrefs.SetInt("batuPusaka", 1);
        SceneManager.LoadScene("Cutscene Ending");
    }
}
