using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Actor
{
    public NavMeshAgent navMeshAgent;
    private float initialX;
    private float initialZ;

    Transform target;
    GameObject weapon;
    Quaternion rotation = Quaternion.identity;
    Random random;

    bool turningToPlayer = false;

    float timeSetTarget;
    float timeLastIdleAnimation;
    float timeLastWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        random = new Random();
        timeLastIdleAnimation = Time.time;
        target = null;
        navMeshAgent.SetDestination(NextWaypoint());
        weapon = transform.Find("EnemyWeapon").gameObject;
        weapon.SetActive(false);
    }

    Vector3 NextWaypoint()
    {
        return new Vector3(initialX + Random.Range(-4f, 4f), 0, initialZ + Random.Range(-4f, 4f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateWeapon();

        if (IsDead())
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            return;
        }

        UpdateIdle();

        if (target && Time.time - timeSetTarget > 1.5f)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > 1.8f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, 2f * Time.deltaTime);
            }
            else
            {
                if (Time.time - timeStartAttack > 1.8f)
                {
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
                if (Time.time - timeLastWaypoint > 7f)
                {
                    timeLastWaypoint = Time.time;
                    navMeshAgent.SetDestination(NextWaypoint());
                }
            }
            if (Time.time - timeLastWaypoint > 15f)
            {
                timeLastWaypoint = Time.time;
                navMeshAgent.SetDestination(NextWaypoint());
            }
        }
    }

    void UpdateIdle()
    {
        if (Time.time - timeLastIdleAnimation > 1f && IsState(State.Idle) && Time.time - timeStartAttack > 2f)
        {
            iTween.PunchPosition(model, iTween.Hash("y", -0.4, "time", 0.7f));
            iTween.PunchScale(model, iTween.Hash("x", 0.1, "time", 0.9f));
            timeLastIdleAnimation = Time.time;
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
            timeSetTarget = Time.time;
        }
    }


    private void UpdateWeapon()
    {
        weapon.SetActive(IsAttacking());
    }

    private void Attack()
    {
        Debug.Log("Enemy - Attack");

        SetState(State.Attacking);
        timeStartAttack = Time.time;

        //iTween.Stop(model);
        //transform.localScale = new Vector3(1, 1, 1);
        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        iTween.PunchPosition(model, iTween.Hash("z", 1, "time", 1f));
        iTween.PunchScale(model, iTween.Hash("y", 0.5, "time", 1.5f));

        SetState(State.Idle);
    }
}
