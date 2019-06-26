﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    enum State { Idle, Attacking, Walking, Following };
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
    float attackStart = -10f;
    GameObject weapon;
    bool colorUpdated = false;
    State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        idleTimer = Time.time;
        target = null;
        model = transform.Find("Model").gameObject;
        rigidbody = GetComponent<Rigidbody>();
        navMeshAgent.SetDestination(waypoints[0].position);
        weapon = transform.Find("EnemyWeapon").gameObject;
        weapon.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateColor();
        UpdateIdle();
        UpdateWeapon();

        if (target && Time.time - targetTime > 1.5f)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > 1.8f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, 2f * Time.deltaTime);
            }
            else
            {
                if (Time.time - attackStart > 1.8f)
                {
                    state = State.Attacking;
                    Attack();
                }
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
        if (Time.time - idleTimer > 1f && state == State.Idle && Time.time - attackStart > 2f)
        {
            iTween.PunchPosition(model, iTween.Hash("y", -0.4, "time", 0.7f));
            iTween.PunchScale(model, iTween.Hash("x", 0.1, "time", 0.9f));
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
        Debug.Log("Enemy - TakeDamage");
        iTween.PunchPosition(model, iTween.Hash("y", 0.5, "time", 1));
        iTween.ColorTo(model, iTween.Hash("r", 0.3, "b", 0.3, "g", 0.3, "time", 0));
        colorUpdated = true;
        lastDamage = Time.time;

        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    private void UpdateColor()
    {
        if (Time.time - lastDamage > 0.1f && colorUpdated)
        {
            iTween.ColorTo(model, iTween.Hash("r", 0, "b", 0, "g", 0, "time", 0));
            colorUpdated = false;
        }
    }

    private void Die()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        iTween.RotateBy(model, iTween.Hash("y", 2, "time", 8));
        iTween.ScaleTo(model, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 2));
    }

    private bool IsAttacking()
    {
        return Time.time - attackStart < 0.2f;
    }

    private void UpdateWeapon()
    {
        weapon.SetActive(IsAttacking());
    }

    private void Attack()
    {
        Debug.Log("Enemy - Attack");

        //iTween.Stop(model);
        //transform.localScale = new Vector3(1, 1, 1);
        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        iTween.PunchPosition(model, iTween.Hash("z", 1, "time", 1f));
        iTween.PunchScale(model, iTween.Hash("y", 0.5, "time", 1.5f));
        attackStart = Time.time;
        state = State.Idle;
    }
}
