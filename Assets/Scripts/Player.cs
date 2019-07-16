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
    GameObject weaponObject;
    GameObject model;
    GameObject weaponArm;
    GameObject weaponsObject;
    List<Weapon> weapons;
    List<Vector3> weaponsPositions;
    List<Quaternion> weaponsRotations;
    public Weapon weapon { get; set; }
    bool colorUpdated = false;
    float lastDamage = -10f;
    bool isWalking;
    float timeWalkStep;
    bool stepLeftFoot = true;
    public bool listening;
    bool inInteractRange;

    float attackStart = -10f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        model = transform.Find("Model").gameObject;
        weaponArm = transform.Find("Model/ArmedArm").gameObject;
        weaponObject = GameObject.FindGameObjectWithTag("PlayerWeapon");
        weaponsObject = GameObject.FindGameObjectWithTag("Weapons");
        weaponObject.SetActive(false);
        listening = false;

        LoadWeapons();
        SetWeapon(0);
        Attack();
    }

    void LoadWeapons()
    {
        weapons = new List<Weapon>();
        weapons.Add(new Weapon(0, "Wooden Club", 10, 0.3f, Weapon.WeaponModifier.Critical));
        weapons.Add(new Weapon(1, "Spiked Club", 10, 0.3f, Weapon.WeaponModifier.Critical));
        weapons.Add(new Weapon(2, "Metal Club", 10, 0.3f, Weapon.WeaponModifier.Critical));
        weaponsObject.SetActive(false);
    }

    public void SetWeapon(int id)
    {

        transform.rotation = Quaternion.identity;
        weapon = weapons[id];
        Transform currentWeapon = transform.Find("Model/ArmedArm/Club");
        Transform targetWeapon = weaponsObject.transform.Find(weapon.Name);
        Transform arm = transform.Find("Model/ArmedArm");
        Vector3 position = new Vector3(arm.position.x + targetWeapon.position.x, arm.position.y + targetWeapon.position.y, arm.position.z + 0.5f);
        
        Instantiate(targetWeapon, position, new Quaternion(targetWeapon.rotation.x, targetWeapon.rotation.y + transform.rotation.y, targetWeapon.rotation.z, targetWeapon.rotation.w), arm);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateWeapon();
        UpdateColor();
        UpdateWalk();

        if (!IsListening())
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            movement.Set(horizontal, 0f, vertical);
            movement.Normalize();

            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

            isWalking = hasHorizontalInput || hasVerticalInput;
            animator.SetBool("IsWalking", isWalking);

            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
            rotation = Quaternion.LookRotation(desiredForward);

        }
        else
        {
            rigidbody.velocity = Vector3.zero;
        }

        if (Input.GetButtonDown("Fire1") && Time.time - attackStart > 0.8f && !IsListening() && !IsInInteractRange())
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Attack");
        iTween.PunchPosition(model, iTween.Hash("z", 1, "time", 0.6f, "delay", 0.1f));
        iTween.PunchScale(model, iTween.Hash("y", 0.5, "time", 0.5f, "delay", 0.1f));

        iTween.RotateBy(weaponArm, iTween.Hash("x", -0.5, "time", 0.1f, "oncomplete", "AnimationArmDown", "oncompletetarget", this.gameObject));

        attackStart = Time.time;

        //Tornado Club animation
        //iTween.RotateBy(weaponArm, iTween.Hash("x", 80, "time", 0.7f));
    }

    public void AnimationArmDown()
    {
        iTween.RotateBy(weaponArm, iTween.Hash("x", 0.8, "time", 0.2f, "oncomplete", "AnimationArmReturn", "oncompletetarget", this.gameObject));
    }

    public void AnimationArmReturn()
    {
        iTween.RotateBy(weaponArm, iTween.Hash("x", -0.3, "time", 0.2f));
    }

    private bool IsAttacking()
    {
        return Time.time - attackStart < 0.2f;
    }

    private void UpdateWeapon()
    {
        weaponObject.SetActive(IsAttacking());
    }

    private void OnAnimatorMove()
    {
        rigidbody.MovePosition(rigidbody.position + movement * animator.deltaPosition.magnitude * speed);
        rigidbody.MoveRotation(rotation);
    }

    public bool IsListening()
    {
        return listening;
    }

    public void SetListening(bool listening)
    {
        this.listening = listening;
    }

    public bool IsInInteractRange()
    {
        return inInteractRange;
    }

    public void SetInInteractRange(bool inInteractRange)
    {
        this.inInteractRange = inInteractRange;
    }

    void UpdateWalk()
    {
        //if (isWalking && Time.time - timeWalkStep > 0.2f)
        //{
        //    if (stepLeftFoot)
        //    {
        //        iTween.RotateAdd(model, iTween.Hash("x", transform.rotation.x, "y", transform.rotation.y, "z", transform.rotation.z + 40, "easetype", "easeOutBounce", "time", 0.1f));

        //    } 
        //    else
        //    {
        //        iTween.RotateAdd(model, iTween.Hash("x", transform.rotation.x, "y", transform.rotation.y, "z", transform.rotation.z - 40, "easetype", "easeOutBounce", "time", 0.1f));
        //    }
        //    stepLeftFoot = !stepLeftFoot;
        //    timeWalkStep = Time.time;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EnemyWeapon")
        {
            TakeDamage();
        }
        if (other.gameObject.name == "Interactable")
        {
            SetInInteractRange(true);
        }
        if (other.gameObject.name == "Shard")
        {
            Physics.IgnoreCollision(other, GetComponent<CapsuleCollider>());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Shard")
        {
            Physics.IgnoreCollision(other, GetComponent<CapsuleCollider>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Interactable")
        {
            SetInInteractRange(false);
        }
    }

    private void TakeDamage()
    {
        Debug.Log("Player - TakeDamage");
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

    private void Die()
    {
        Debug.Log("Player - Die");
    }

    private void UpdateColor()
    {
        if (Time.time - lastDamage > 0.1f && colorUpdated)
        {
            iTween.ColorTo(model, iTween.Hash("r", 0, "b", 0, "g", 0, "time", 0));
            colorUpdated = false;
        }
    }
}

public class Weapon
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Attack { get; set; }
    public float ModifierChance { get; set; }
    public WeaponModifier Modifier { get; set; }

    public enum WeaponModifier { Critical, Poison, Burn, Shrink, Freeze };

    public Weapon (int id, string name, int attack, float modifierChance, WeaponModifier modifier)
    {
        ID = id;
        Name = name;
        Attack = attack;
        ModifierChance = modifierChance;
        Modifier = modifier;
    }
}