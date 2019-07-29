using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform canvas;
    float turnSpeed = 20f;
    int health = 5;
    float speed = 2f;
    Animator animator;
    Rigidbody rigidbody;
    Vector3 movement;
    Quaternion rotation = Quaternion.identity;
    GameObject weaponObject;
    GameObject model;
    GameObject weaponArm;
    GameObject weaponsObject;
    Slider healthSlider;
    List<Weapon> weapons;
    List<Vector3> weaponsPositions;
    List<Quaternion> weaponsRotations;
    UI ui;
    public float TimeStart { get; set; }
    public Weapon Weapon { get; set; }
    bool colorUpdated = false;
    float lastDamage = -10f;
    bool isWalking;
    float timeWalkStep;
    bool stepLeftFoot = true;
    public bool listening;
    bool inInteractRange;
    public int Coins { get; set; }
    public string Name { get; set; }

    //0   Enter home
    //1   Talked to Toni
    //2   Got wooden club
    //3   Enter monster map
    //4   Talked to toni
    //5   Got spiked club
    //6   Enter monster map
    //7   Talked to Toni
    //8   Got metal club
    public int Progress { get; set; }

    float attackStart = -10f;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        model = transform.Find("Model").gameObject;
        weaponArm = transform.Find("Model/ArmedArm").gameObject;
        healthSlider = canvas.Find("HealthSlider").GetComponent<Slider>();
        weaponObject = GameObject.FindGameObjectWithTag("PlayerWeapon");
        weaponsObject = GameObject.FindGameObjectWithTag("Weapons");
        ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UI>();
        weaponObject.SetActive(false);
        listening = false;
        UpdateCoins(0);

        healthSlider.maxValue = health;

        LoadWeapons();
        Attack();

        SetWeapon(3);
        Weapon = weapons[3];
        Progress = 0;
        TimeStart = Time.time;

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Home" || sceneName == "Monster" || sceneName == "Boss")
        {
            ui.Load();
            SetWeapon(Weapon.ID);
            Weapon.Name = weapons[Weapon.ID].Name;
        }
        else if (sceneName == "Ranking" || sceneName == "RankingAfterGame")
        {
            ui.DisplayRanking();
        }


        if (sceneName == "Monster" && Progress == 2)
        {
            Progress = 3;
        }

        if (sceneName == "Monster" && Progress == 5)
        {
            Progress = 6;
        }

        //if (sceneName == "Boss")
        //{
        //    Progress = 9;
        //    SetWeapon(2);
        //    Name = "hehe";
        //    Weapon = new Weapon(2, "Hakapik", 43, 0.3f, "Bludgeoning");
        //}

        //if (sceneName == "Home")
        //{
        //    SetWeapon(0);
        //    Progress = 6;
        //    Coins = 22;
        //    Weapon = new Weapon(0, "Wooden Club", 10, 0.1f, "Thwarting");
        //    //Weapon = new Weapon(1, "Spiked Club", 20, 0.1f, "Bashing");
        //    //Weapon = new Weapon(2, "Hakapik", 30, 0.1f, "Bludgeoning");
        //}
    }

    void LoadWeapons()
    {
        weapons = new List<Weapon>();
        weapons.Add(new Weapon(0, "Wooden Club", 10, 0.1f, "Thwarting"));
        weapons.Add(new Weapon(1, "Spiked Club", 20, 0.1f, "Bashing"));
        weapons.Add(new Weapon(2, "Hakapik", 30, 0.1f, "Bludgeoning"));
        weapons.Add(new Weapon(3, "Nothing", 0, 0.1f, "Dissapointment"));
        weaponsObject.SetActive(false);
    }

    public void SetWeapon(int id)
    {
        SetWeaponModel(id);
    }

    public void SetWeaponModel(int id)
    {
        transform.rotation = Quaternion.identity;
        if (Weapon != null)
        {
            GameObject currentWeapon = transform.Find("Model/ArmedArm/" + Weapon.Name + "(Clone)").gameObject;
            Destroy(currentWeapon);
        }
        Transform targetWeapon = weaponsObject.transform.Find(weapons[id].Name);
        Transform arm = transform.Find("Model/ArmedArm");
        Vector3 position = new Vector3(arm.position.x + targetWeapon.position.x, arm.position.y + targetWeapon.position.y, arm.position.z + 0.5f);
        
        Instantiate(targetWeapon, position, new Quaternion(targetWeapon.rotation.x, targetWeapon.rotation.y + transform.rotation.y, targetWeapon.rotation.z, targetWeapon.rotation.w), arm);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateWeapon();
        UpdateColor();
        if (IsDead())
        {
            return;
        }
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
            rigidbody.angularVelocity = Vector3.zero;
        }

        string sceneName = SceneManager.GetActiveScene().name;
        if (!IsListening() && !IsInInteractRange())
        {
            if (sceneName == "Home" || sceneName == "Monster" || sceneName == "Boss")
            {
                if (Input.GetButtonDown("Fire1") && Time.time - attackStart > 0.8f)
                {
                    Attack();
                }
            }
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
        if (!IsListening() && !IsDead())
        {
            rigidbody.MovePosition(rigidbody.position + movement * animator.deltaPosition.magnitude * speed);
            rigidbody.MoveRotation(rotation);
        }
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
        if (other.gameObject.name == "BossDialogueTrigger")
        {
            if (Progress == 8)
            {
                List<Page> dialogue = new List<Page>();
                dialogue.Add(new Page("You: Biggie-S!"));
                dialogue.Add(new Page("Biggie-S: ... ... ... ?"));
                dialogue.Add(new Page("You: You been messin' on our turf for too long - let's settle this right here, right now!"));
                dialogue.Add(new Page("Biggie-S: ... ... owo ... ..."));
                dialogue.Add(new Page("You: That's it - you're going down, m*therfucker!"));
                DialogueBox dialogueBox = canvas.transform.Find("DialogueBox").GetComponent<DialogueBox>();
                dialogueBox.StartConversation(dialogue);
                other.gameObject.SetActive(false);
                Progress = 9;
                ui.Save();
            }
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
        if (IsDead())
        {
            return;
        }
        Debug.Log("Player - TakeDamage");
        iTween.PunchPosition(model, iTween.Hash("y", 0.5, "time", 1));
        iTween.ColorTo(model, iTween.Hash("r", 0.3, "b", 0.3, "g", 0.3, "time", 0));
        colorUpdated = true;
        lastDamage = Time.time;

        health--;
        healthSlider.value = health;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player - Die");

        iTween.RotateBy(model, iTween.Hash("y", 2, "time", 8));
        iTween.ScaleTo(model, iTween.Hash("x", 0, "y", 0, "z", 0, "time", 1.8f, "oncomplete", "CallbackGameOver", "oncompletetarget", this.gameObject));
    }

    void CallbackGameOver()
    {
        ui.DoGameOver();
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    private void UpdateColor()
    {
        if (Time.time - lastDamage > 0.1f && colorUpdated)
        {
            iTween.ColorTo(model, iTween.Hash("r", 0, "b", 0, "g", 0, "time", 0));
            colorUpdated = false;
        }
    }

    public int UpdateCoins(int add)
    {
        Debug.Log("UpdateCoins");
        Text coinsText = canvas.Find("Coins/Text").GetComponent<Text>();
        Coins = Coins + add;
        coinsText.text = Coins.ToString();
        return Coins;
    }
}

public class Weapon
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int Attack { get; set; }
    public float ModifierChance { get; set; }
    public string Modifier { get; set; }

    // public enum WeaponModifier { Critical, Poison, Burn, Shrink, Freeze };

    public Weapon (int id, string name, int attack, float modifierChance, string modifier)
    {
        ID = id;
        Name = name;
        Attack = attack;
        ModifierChance = modifierChance;
        Modifier = modifier;
    }
}