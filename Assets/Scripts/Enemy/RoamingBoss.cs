using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoamingEnemy;

public class RoamingBoss : RoamingEnemy
{
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
        enemySize = enemyPrefabs.Length;
        enemyList = enemyPrefabs;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
