using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    int health = 10;
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    Rigidbody rigidbody;
    int waypointIndex;
    GameObject model;
    float lastDamage = -10f;
    Transform target;
    Quaternion rotation = Quaternion.identity;
    float targetTime;
    bool turningToPlayer = false;
    float idleTimer;

    // Start is called before the first frame update
    void Start()
    {
        idleTimer = Time.time;
        target = null;
        model = GameObject.Find("Enemy/Model");
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateColor();
        UpdateIdle();

        if (target && Time.time - targetTime > 1.5f)
        {
            Debug.Log(target.position.ToString());

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > 1.8f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, 2f * Time.deltaTime);
            }
            iTween.LookUpdate(gameObject, target.position, 0f);
        }
        else if (target)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            TurnToPlayer();
        }
        else
        {
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                navMeshAgent.SetDestination(waypoints[waypointIndex].position);
            }
        }
    }

    void UpdateIdle()
    {
        if (Time.time - idleTimer > 1f)
        {
            iTween.PunchPosition(model, iTween.Hash("y", -0.4, "time", 1f));
            iTween.PunchScale(model, iTween.Hash("x", 0.1, "time", 1f));
            idleTimer = Time.time;
        }
    }

    void TurnToPlayer()
    {
        if (!turningToPlayer)
        {
            iTween.LookTo(gameObject, target.position, 1.5f);
            turningToPlayer = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerWeapon")
        {
            TakeDamage();
            target = other.gameObject.GetComponentInParent<Transform>();
            targetTime = Time.time;
        }
    }

    private void TakeDamage()
    {
        Debug.Log("TakeDamage");
        iTween.PunchPosition(model, iTween.Hash("y", 0.5, "time", 1));
        iTween.ColorTo(model, iTween.Hash("r", 1, "b", 1, "g", 1, "time", 0.02));
        lastDamage = Time.time;

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    private void UpdateColor()
    {
        if (Time.time - lastDamage > 0.1f)
        {
            iTween.ColorTo(model, iTween.Hash("r", 0, "b", 0, "g", 0, "time", 0.02));
        }
    }

    private void Die()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        iTween.RotateBy(model, iTween.Hash("y", 2, "time", 8));
        iTween.ScaleTo(model, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 2));
    }
}
