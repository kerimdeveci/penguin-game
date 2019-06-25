using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float turnSpeed = 20f;
    int health = 10;
    float speed = 2f;
    Animator animator;
    Rigidbody rigidbody;
    Vector3 movement;
    Quaternion rotation = Quaternion.identity;
    GameObject weapon;
    GameObject model;
    float lastDamage = -10f;

    float attackStart = -10f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        model = transform.Find("Model").gameObject;
        weapon = GameObject.FindGameObjectWithTag("PlayerWeapon");
        weapon.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateWeapon();
        UpdateColor();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement.Set(horizontal, 0f, vertical);
        movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        bool isWalking = hasHorizontalInput || hasVerticalInput;
        animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
        rotation = Quaternion.LookRotation(desiredForward);

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Attack");
        attackStart = Time.time;
    }

    private bool IsAttacking()
    {
        return Time.time - attackStart < 0.2f;
    }

    private void UpdateWeapon()
    {
        weapon.SetActive(IsAttacking());
    }

    private void OnAnimatorMove()
    {
        rigidbody.MovePosition(rigidbody.position + movement * animator.deltaPosition.magnitude * speed);
        rigidbody.MoveRotation(rotation);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EnemyWeapon")
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        Debug.Log("Enemy - TakeDamage");
        iTween.PunchPosition(model, iTween.Hash("y", 0.5, "time", 1));
        lastDamage = Time.time;

        health--;

        if (health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log("Player - Die");
    }



    private void UpdateColor()
    {
        if (Time.time - lastDamage > 0.1f)
        {
        }
    }
}
