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
    protected WalkState walkState;
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
    protected GameObject player;
    protected float chaseTimer;

    protected int enemySize;
    public GameObject[] enemyPrefabs;
    protected GameObject[] enemyList;
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
        transform.rotation = rotation;
        idleState = IdleState.MOVING;
    }

    private void Update()
    {
        if (Distance2D(transform.position, player.transform.position) < 4f && transform.position.y - player.transform.position.y < 0.3f)
        {
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("Player"))
                    chaseState = ChaseState.CHASING;
            }
            else chaseState = ChaseState.DEFAULT;
        }
        else chaseState = ChaseState.DEFAULT;
        if (chaseTimer >= 4f) chaseState = ChaseState.DEFAULT;
        if (chaseState == ChaseState.CHASING)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, walkSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
            chaseTimer += Time.deltaTime;
            animator.SetBool("isMoving", true);
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
        animator.SetBool("isMoving", true);
        float step = walkSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTargetLocation, step);
        Vector3 rot = currentTargetLocation - transform.position;
        if (rot != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(rot, Vector3.up);
    }

    IEnumerator Idle()
    {
        animator.SetBool("isMoving", false);
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

    private double Distance2D(Vector3 from, Vector3 to)
    {
        return Math.Sqrt(Math.Pow((from.x - to.x), 2) + Math.Pow((from.z - to.z), 2));
    }
}
