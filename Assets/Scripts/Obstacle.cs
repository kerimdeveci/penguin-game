using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Player player;
    GameObject brokenRock;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        brokenRock = GameObject.FindGameObjectWithTag("Broken");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerWeapon")
        {
            if (player.Weapon.ID == 0)
            {
                Break();
            }
        }
    }

    void Break()
    {
        Debug.Log("Break");
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
        Instantiate(brokenRock, transform.position, transform.rotation);
    }
}
