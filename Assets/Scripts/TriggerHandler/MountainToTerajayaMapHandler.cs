using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainToTerajayaMapHandler : ChangeMapTriggerHandler
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("batuPusaka", 0) == 1)
        {
            targetSceneName = "Terajaya Overworld";
            spawnPosition = new Vector3(-2.06f, -0.05f, -16.59f);
        }
    }

}
