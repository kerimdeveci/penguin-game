﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    int waypointIndex;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[waypointIndex].position);
        }
    }
}
