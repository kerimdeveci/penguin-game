﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public enum State { Idle, Attacking, Walking, Following };

    State state;

    int health = 3;

    bool colorUpdated = false;

    public Rigidbody rigidbody;
    public GameObject model;

    public float timeDied;
    public float timeStartAttack;
    public float timeLastDamaged;

    // Start is called before the first frame update
    public void Start()
    {
        state = State.Idle;
        model = transform.Find("Model").gameObject;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateModel();
    }


    public void SetState(State state)
    {
        this.state = state;
    }

    public bool IsState(State state)
    {
        return this.state == state;
    }

    void Die()
    {
        Debug.Log("Enemy - Die");

        timeDied = Time.time;

        iTween.RotateBy(model, iTween.Hash("y", 2, "time", 8));
        iTween.ScaleTo(model, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 2));
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public bool IsAttacking()
    {
        return Time.time - timeStartAttack < 0.2f;
    }

    void UpdateModel()
    {
        if (IsDead() && Time.time - timeDied > 1f)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        if (Time.time - timeLastDamaged > 0.1f && colorUpdated)
        {
            iTween.ColorTo(model, iTween.Hash("r", 0, "b", 0, "g", 0, "time", 0));
            colorUpdated = false;
        }
    }

    public void TakeDamage()
    {
        Debug.Log("Enemy - TakeDamage");

        iTween.PunchPosition(model, iTween.Hash("y", 0.5, "time", 1));
        iTween.ColorTo(model, iTween.Hash("r", 0.3, "b", 0.3, "g", 0.3, "time", 0));
        colorUpdated = true;
        timeLastDamaged = Time.time;

        health--;

        if (health <= 0)
        {
            Die();
        }
    }
}