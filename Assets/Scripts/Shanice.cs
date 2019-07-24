using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shanice : Interactable
{
    void Start()
    {
        base.Start();

        dialogue = new List<Page>();
        dialogue.Add(new Page("Hello, this is Shanice"));
        dialogue.Add(new Page("UIEnhancement", Tuple.Create((Interactable)this, "SwitchWeapon")));
    }

    void Update()
    {
        base.Update();
    }

    public void SwitchWeapon()
    {
        player.SetWeapon(2);
        player.Weapon = new Weapon(2, "Hakapik", 10, 0.3f, "Critical");
    }
}
