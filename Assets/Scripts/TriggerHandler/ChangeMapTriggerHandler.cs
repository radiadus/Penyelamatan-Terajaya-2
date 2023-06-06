using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMapTriggerHandler : MonoBehaviour
{
    public string targetSceneName;
    public Vector3 spawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameManager.Instance.ChangeMap(targetSceneName, spawnPosition);
        }
    }
}
