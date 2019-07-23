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
    float timeToNextWaypoint;

    private void Awake()
    {
        random = new Random();
        float delay = Random.Range(1f, 2f);
        timeLastIdleAnimation = delay;
    }

    void Start()
    {
        base.Start();
        initialX = transform.position.x;
        initialZ = transform.position.z;
        target = null;
        Vector3 next = NextWaypoint();
        navMeshAgent.SetDestination(next);
        weapon = transform.Find("EnemyWeapon").gameObject;
        weapon.SetActive(false);
        transform.rotation = Quaternion.LookRotation(new Vector3(0, Random.Range(0f, 180f), 0));
    }

    Vector3 NextWaypoint()
    {
        return new Vector3(initialX + Random.Range(-4f, 4f), 0, initialZ + Random.Range(-4f, 4f));
    }

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

        if (target && Time.timeSinceLevelLoad - timeSetTarget > 1.5f)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > 1.8f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, 2f * Time.deltaTime);
            }
            else
            {
                if (Time.timeSinceLevelLoad - timeStartAttack > 1.8f)
                {
                    Attack();
                }
            }
            iTween.LookUpdate(gameObject, target.position, 0f);
            if (player.IsDead())
            {
                target = null;
                navMeshAgent.isStopped = false;
            }
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
                if (Time.timeSinceLevelLoad - timeLastWaypoint > timeToNextWaypoint)
                {
                    timeLastWaypoint = Time.timeSinceLevelLoad;
                    timeToNextWaypoint = Random.Range(5f, 12f);
                    Vector3 next = NextWaypoint();
                    navMeshAgent.SetDestination(next);
                }
            }
            if (Time.timeSinceLevelLoad - timeLastWaypoint > 8f)
            {
                timeLastWaypoint = Time.timeSinceLevelLoad;
                timeToNextWaypoint = Random.Range(5f, 12f);
                navMeshAgent.SetDestination(new Vector3(initialX, 0, initialZ));
            }
        }
    }

    void UpdateIdle()
    {
        if (Time.timeSinceLevelLoad - timeLastIdleAnimation > 1f && IsState(State.Idle) && Time.timeSinceLevelLoad - timeStartAttack > 2f)
        {
            iTween.PunchPosition(model, iTween.Hash("y", -0.4, "time", 0.7f));
            iTween.PunchScale(model, iTween.Hash("x", 0.1, "time", 0.9f));
            timeLastIdleAnimation = Time.timeSinceLevelLoad;
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
            timeSetTarget = Time.timeSinceLevelLoad;
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
        timeStartAttack = Time.timeSinceLevelLoad;

        //iTween.Stop(model);
        //transform.localScale = new Vector3(1, 1, 1);
        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        iTween.PunchPosition(model, iTween.Hash("z", 1, "time", 1f));
        iTween.PunchScale(model, iTween.Hash("y", 0.5, "time", 1.5f));

        SetState(State.Idle);
    }

    public override void Die()
    {
        base.Die();
        Debug.Log("Enemy - Die");
        DropLoot();
    }

    public override void Respawn()
    {
        base.Respawn();
        Debug.Log("Enemy - Respawn");
        iTween.MoveTo(gameObject, new Vector3(initialX, 0, initialZ), 0);
        target = null;
        navMeshAgent.isStopped = false;
        timeLastIdleAnimation = Time.timeSinceLevelLoad;
    }

    void DropLoot()
    {
        Debug.Log("DropLoot");
        GameObject coinObject = items.Find("Coin").gameObject;
        Instantiate(coinObject, transform.position, coinObject.transform.rotation);
    }
}
