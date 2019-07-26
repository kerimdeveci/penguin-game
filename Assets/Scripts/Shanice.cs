using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shanice : Interactable
{
    void Start()
    {
        base.Start();

        if (player.Progress == 0)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("asdf? That you sweetie?"));
        }
    }

    void Update()
    {
        base.Update();

        if (player.Progress == 2 || player.Progress == 5 || player.Progress == 8 || player.Progress == 3 || player.Progress == 6)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("how you doing sweetie?"));
        }

        if (player.Progress == 1)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("Hello, this is Shanice"));
            dialogue.Add(new Page("", Tuple.Create((Interactable)this, "GiveWoodenClub")));
        }

        if (player.Progress == 4)
        {
            if (player.Coins >= 10)
            {
                dialogue = new List<Page>();
                dialogue.Add(new Page("you want ice? i take money"));
                dialogue.Add(new Page("", Tuple.Create((Interactable)this, "GiveSpikedClub")));
            }
            else
            {
                dialogue = new List<Page>();
                dialogue.Add(new Page("you want ice? you don't have the money"));
            }
        }

        if (player.Progress == 7)
        {
            if (player.Coins >= 20)
            {
                dialogue = new List<Page>();
                dialogue.Add(new Page("you want rock? i take money"));
                dialogue.Add(new Page("", Tuple.Create((Interactable)this, "GiveMetalClub")));
            }
            else
            {
                dialogue = new List<Page>();
                dialogue.Add(new Page("you want rock? you don't have the money"));
            }
        }
    }

    public void GiveMetalClub()
    {
        player.Progress = 8;

        player.UpdateCoins(-20);

        player.SetWeapon(2);
        player.Weapon = new Weapon(2, "Hakapik", 30, 0.3f, "Bludgeoning");

        dialogue = new List<Page>();
        dialogue.Add(new Page("break that rock"));
    }

    public void GiveSpikedClub()
    {
        player.Progress = 5;

        player.UpdateCoins(-10);

        player.SetWeapon(1);
        player.Weapon = new Weapon(1, "Spiked Club", 20, 0.3f, "Bashing");

        dialogue = new List<Page>();
        dialogue.Add(new Page("break that ice"));
    }

    public void GiveWoodenClub()
    {
        player.Progress = 2;

        player.SetWeapon(0);
        player.Weapon = new Weapon(0, "Wooden Club", 10, 0.3f, "Thwarting");

        dialogue = new List<Page>();
        dialogue.Add(new Page("good luck beb"));
    }
}
