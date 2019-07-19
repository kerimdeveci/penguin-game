using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Player player;
    int value;
    bool collected = false;

    void Awake()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        iTween.Init(this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        value = 3;
    }

    private void Start()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 1f));
        iTween.RotateBy(gameObject, iTween.Hash("y", 1, "time", 2f, "looptype", "loop", "easetype", "linear"));
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!collected && other.gameObject.name == "Player")
        {
            collected = true;
            Debug.Log(other.gameObject.name);
            player.UpdateCoins(value);
            iTween.ScaleTo(gameObject, iTween.Hash("x", 0.01, "y", 0.01, "z", 0.01, "time", 0.3f, "easetype", "easeInCubic", "oncomplete", "Disappear", "oncompletetarget", this.gameObject));
        }
    }

    void Disappear()
    {
        Destroy(gameObject);
    }
}
