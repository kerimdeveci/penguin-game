using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Actor
{
    private float initialX;
    private float initialZ;

    Transform target;
    GameObject weapon;
    Quaternion rotation = Quaternion.identity;
    Random random;

    bool turningToPlayer = false;

    float timeSetTarget;
    float timeLastIdleAnimation;

    void Start()
    {
        base.Start();
        initialX = transform.position.x;
        initialZ = transform.position.z;
        random = new Random();
        timeLastIdleAnimation = Random.Range(1f, 2f);
        target = null;
        weapon = transform.Find("EnemyWeapon").gameObject;
        weapon.SetActive(false);
        transform.rotation = Quaternion.LookRotation(new Vector3(0, Random.Range(0f, 180f), 0));
    }

    void FixedUpdate()
    {
        UpdateWeapon();

        if (IsDead())
        {
            return;
        }

        UpdateIdle();

        if (target && Time.time - timeSetTarget > 1.5f)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 4f)
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
            TurnToPlayer();
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
