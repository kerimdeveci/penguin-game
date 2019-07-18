using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Player player;
    int value;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        value = 3;

        iTween.RotateBy(gameObject, new Vector3(0,0,5), 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            player.UpdateCoins(value);
        }
    }
}
