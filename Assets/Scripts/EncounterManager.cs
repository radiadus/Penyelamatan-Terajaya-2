using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager Instance { get; private set; }
    public Vector3 lastPosition;
    public List<int> defeatedEnemyIds = new List<int>();
    public int lastSceneId;
    public int currentEnemyId;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartEncounter(int sceneId, int roamingEnemyId, GameObject[] enemies)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        lastPosition = player.transform.position;
        lastSceneId = SceneManager.GetActiveScene().buildIndex;
        currentEnemyId = roamingEnemyId;
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneId);
        load.completed += (asyncOperation) =>
        {
            GameManager.Instance.gameState = GameManager.State.ENCOUNTER;
            Encounter encounter = GameObject.FindObjectOfType<Encounter>();
            encounter.enemyPrefabs = enemies;
        };
    }

    public void WinEncounter()
    {
        defeatedEnemyIds.Add(currentEnemyId);
        currentEnemyId = 0;
        AsyncOperation load = SceneManager.LoadSceneAsync(lastSceneId);
        load.completed += (asyncOperation) =>
        {
            GameManager.Instance.gameState = GameManager.State.DEFAULT;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = lastPosition;
            GameManager.Instance.SaveGame(player);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                RoamingEnemy roamingEnemy = enemy.GetComponent<RoamingEnemy>();
                if (defeatedEnemyIds.Contains(roamingEnemy.id))
                {
                    enemy.SetActive(false);
                }
            }
        };
    }

    public void LoseEncounter()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(lastSceneId);
        load.completed += (asyncOperation) =>
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = GameManager.Instance.sceneGameOverSpawn[SceneManager.GetActiveScene().name];
            defeatedEnemyIds.Clear();
        };
    }

    public void FleeEncounter()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(lastSceneId);
        load.completed += (asyncOperation) =>
        {
            GameManager.Instance.gameState = GameManager.State.DEFAULT;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = lastPosition;
            GameManager.Instance.SaveGame(player);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                RoamingEnemy roamingEnemy = enemy.GetComponent<RoamingEnemy>();
                if (defeatedEnemyIds.Contains(roamingEnemy.id) || roamingEnemy.id == currentEnemyId)
                {
                    enemy.SetActive(false);
                }
            }
            currentEnemyId = 0;
        };
    }
}
