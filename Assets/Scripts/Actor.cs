using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour
{
    public Transform items;
    public Player player;
    public enum State { Idle, Attacking, Walking, Following };
    UI ui;

    State state;

    public int maxHealth = 3;
    public int health;

    bool colorUpdated = false;

    public Rigidbody rigidbody;
    public GameObject model;

    public float timeDied;
    public float timeStartAttack;
    public float timeLastDamaged;

    // Start is called before the first frame update
    public void Start()
    {
        ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UI>();
        health = maxHealth;
        items = GameObject.FindGameObjectWithTag("Items").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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

    public virtual void Respawn()
    {
        GetComponent<BoxCollider>().enabled = true;
        timeDied = 0.0f;
        health = maxHealth;
        iTween.ScaleTo(model, iTween.Hash("x", 0.4, "y", 0.4, "z", 0.4, "time", 1));
    }

    public virtual void Die()
    {
        Debug.Log("Actor - Die");

        timeDied = Time.time;

        GetComponent<BoxCollider>().enabled = false;

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
            //gameObject.SetActive(false);
            //Destroy(gameObject);
        }
        if (Time.time - timeLastDamaged > 0.1f && colorUpdated)
        {
            iTween.ColorTo(model, iTween.Hash("r", 0, "b", 0, "g", 0, "time", 0));
            colorUpdated = false;
        }

        if (IsDead() && Time.time - timeDied > 5f)
        {
            if (gameObject.name != "Boss")
            {
                Respawn();
            }
        }

        if (IsDead() && Time.time - timeDied > 2f)
        {
            if (gameObject.name == "Boss" && !ui.IsFadingOut())
            {
                ui.DoGameComplete();
                ui.DoFade();
                if (Time.time - timeDied > 3f)
                {
                    ui.GoLeaderboards();
                }
            }
        }
    }

    public void TakeDamage()
    {
        Debug.Log("Enemy - TakeDamage");
        if (IsDead())
        {
            return;
        }
        iTween.PunchPosition(model, iTween.Hash("y", 0.5, "time", 1));
        iTween.ColorTo(model, iTween.Hash("r", 0.3, "b", 0.3, "g", 0.3, "time", 0));
        colorUpdated = true;
        timeLastDamaged = Time.time;

        Debug.Log(player.Weapon.Attack);

        GameObject damageTextObject = items.Find("DamageNumber").gameObject;
        damageTextObject = Instantiate(damageTextObject, transform.position, Quaternion.identity);
        damageTextObject.GetComponent<DamageNumber>().Target = gameObject;
        damageTextObject.GetComponent<TextMesh>().text = player.Weapon.Attack.ToString();

        health--;

        if (health <= 0)
        {
            Die();
        }
    }
}
