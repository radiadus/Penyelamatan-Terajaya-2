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
        GameManager.Instance.gameState = GameManager.State.ENCOUNTER;
        Time.timeScale = 0f;
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneId);
        load.completed += (asyncOperation) =>
        {
            Time.timeScale = 1f;
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
            GameManager.Instance.player = player;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = lastPosition;
            player.GetComponent<CharacterController>().enabled = true;
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
            CheckRoof(player);
        };
    }

    public void LoseEncounter()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(lastSceneId);
        load.completed += (asyncOperation) =>
        {
            GameManager.Instance.gameState = GameManager.State.DEFAULT;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameManager.Instance.player = player;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = GameManager.Instance.sceneGameOverSpawn[SceneManager.GetActiveScene().name];
            player.GetComponent<CharacterController>().enabled = true;
            defeatedEnemyIds.Clear();
            GameManager.Instance.SaveGame(player);
        };
    }

    public void FleeEncounter()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(lastSceneId);
        load.completed += (asyncOperation) =>
        {
            GameManager.Instance.gameState = GameManager.State.DEFAULT;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameManager.Instance.player = player;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = lastPosition;
            player.GetComponent<CharacterController>().enabled = true;
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
            CheckRoof(player);
            currentEnemyId = 0;
        };
    }

    private void CheckRoof(GameObject player)
    {
        if (Physics.Raycast(player.transform.position, Vector3.up, out RaycastHit hit, 3, 1 << 6))
        {
            GameObject[] roofObjects = GameObject.FindGameObjectsWithTag("Roof");
            foreach (GameObject roofObject in roofObjects)
            {
                roofObject.SetActive(false);
            }
        }
    }
}
