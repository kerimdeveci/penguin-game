using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhance : MonoBehaviour
{
    Player player;
    GameObject canvas;
    GameObject ui;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        ui = canvas.transform.Find("UIEnhancement").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadWeapon()
    {

    }

    void EnhanceRoll()
    {
        Debug.Log("EnhanceRoll");
        Weapon weapon = player.Weapon;
        weapon.Attack = Random.Range(10, 40);
        player.Weapon = weapon;
    }

    void Open()
    {
        ui.GetComponent<CanvasGroup>().alpha = 1;
    }

    void Close()
    {
        ui.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void ButtonClickEnhance()
    {
        Debug.Log("ButtonClickEnhance");
        EnhanceRoll();
    }

    public void ButtonClickLeave()
    {
        Debug.Log("ButtonClickLeave");
        Close();
        player.SetListening(false);
    }
}
