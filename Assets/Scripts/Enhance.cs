using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Text weaponName = ui.transform.Find("Outer/WeaponOuter/WeaponNameText").GetComponent<Text>();
        Text weaponModifier = ui.transform.Find("Outer/WeaponOuter/WeaponModifierText").GetComponent<Text>();
        Text weaponDescription = ui.transform.Find("Outer/WeaponOuter/WeaponDescriptionText").GetComponent<Text>();
        Text weaponAttack = ui.transform.Find("Outer/WeaponOuter/WeaponAttackText").GetComponent<Text>();

        weaponName.text = player.Weapon.Name;
        weaponModifier.text = "of " + player.Weapon.Modifier;
        weaponDescription.text = (player.Weapon.ModifierChance * 100).ToString() + "% chance to " + GetModifierDescription(player.Weapon.Modifier);
        weaponAttack.text = player.Weapon.Attack.ToString() + " Attack";
    }

    string GetModifierDescription(string modifier)
    {
        switch (modifier)
        {
            case "Critical":
                return "land a critical strike";
            default:
                return "do something";
        }
    }

    void EnhanceRoll()
    {
        Debug.Log("EnhanceRoll");
        Weapon weapon = player.Weapon;
        weapon.Attack = Random.Range(10, 40);
        player.Weapon = weapon;
    }

    public void Open()
    {
        LoadWeapon();
        ui.GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
    }

    void Close()
    {
        ui.GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
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
