using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMapTriggerHandler : MonoBehaviour
{
    public string targetSceneName;
    public Vector3 spawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.Log(SceneManager.GetSceneByName(targetSceneName).buildIndex);
            GameManager.Instance.ChangeMap(targetSceneName, spawnPosition);
        }
    }
}
