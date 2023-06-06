using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RoamingEnemy : MonoBehaviour
{
    public int id;
    public Waypoint currentWaypoint;
    public Vector3 currentTargetLocation;
    public float walkSpeed;
    public int encounterSceneIndex;
    public enum WalkState
    {
        FORWARD,
        BACKWARD
    }
    private WalkState walkState;
    public enum IdleState
    {
        MOVING,
        IDLE
    }
    public IdleState idleState;
    public enum ChaseState
    {
        CHASING,
        DEFAULT
    }
    public ChaseState chaseState;
    private GameObject player;
    private float chaseTimer;

    private int enemySize;
    public GameObject[] enemyPrefabs;
    private GameObject[] enemyList;

    // Start is called before the first frame update
    void Start()
    {
        walkState = WalkState.FORWARD;
        idleState = IdleState.MOVING;
        chaseState = ChaseState.DEFAULT;
        player = GameObject.FindGameObjectWithTag("Player");
        chaseTimer = 0f;
        currentTargetLocation = currentWaypoint.GetPosition();
        enemySize = Random.Range(1, 6);
        enemyList = new GameObject[enemySize];
        enemyList[0] = enemyPrefabs[0];
        int range = enemyPrefabs.Length;
        for (int i = 1; i < enemySize; i++)
        {
            enemyList[i] = enemyPrefabs[Random.Range(0, range)];
        }
    }

    void NextWaypoint()
    {
        switch (walkState)
        {
            case WalkState.FORWARD:
                if (currentWaypoint.nextWayPoint != null)
                {
                    currentWaypoint = currentWaypoint.nextWayPoint;
                    currentTargetLocation = currentWaypoint.GetPosition();
                }
                else
                {
                    walkState = WalkState.BACKWARD;
                    NextWaypoint();
                }
                break;
            case WalkState.BACKWARD:
                if (currentWaypoint.previousWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                    currentTargetLocation = currentWaypoint.GetPosition();
                }
                else
                {
                    walkState = WalkState.FORWARD;
                    NextWaypoint();
                }
                break;
            default:
                break;
        }
    }

    void FaceTargetLocation()
    {
        Vector3 vectToMove = currentTargetLocation - transform.position;
        Quaternion rotation = Quaternion.LookRotation(vectToMove, Vector3.up);
        StartCoroutine(Rotate(rotation));
    }

    IEnumerator Rotate(Quaternion rotation)
    {
        float step = 2f * Time.deltaTime;
        while (Quaternion.Angle(transform.rotation, rotation) > 1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, step);
            yield return null;
        }
        idleState = IdleState.MOVING;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 4f) chaseState = ChaseState.CHASING;
        else chaseState = ChaseState.DEFAULT;
        if (chaseTimer >= 4f) chaseState = ChaseState.DEFAULT;
        if (chaseState == ChaseState.CHASING)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, walkSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
            chaseTimer += Time.deltaTime;
            return;
        }
        if (Vector3.Distance(transform.position, currentTargetLocation) < 0.001f)
        {
            idleState = IdleState.IDLE;
            chaseTimer = 0f;
            StartCoroutine(Idle());
            NextWaypoint();
            return;
        }
        if (idleState == IdleState.IDLE) return;
        float step = walkSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTargetLocation, step);
        transform.rotation = Quaternion.LookRotation(currentTargetLocation - transform.position, Vector3.up);
    }

    IEnumerator Idle()
    {
        int frames = 0;
        while (frames < 30)
        {
            frames++;
            yield return null;
        }
        FaceTargetLocation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EncounterManager.Instance.StartEncounter(encounterSceneIndex, id, enemyList);
        }
    }
}
