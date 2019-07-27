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
            dialogue.Add(new Page("Shanice: Hey sweetie, long time no see."));
        }
    }

    void Update()
    {
        base.Update();

        if (player.Progress == 2 || player.Progress == 5 || player.Progress == 8 || player.Progress == 9 || player.Progress == 3 || player.Progress == 6)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("Shanice: You show those seals not to mess with us again, " + player.Name + "."));
        }

        if (player.Progress == 1)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("Shanice: Hey sweetie, long time no see. You back to help us with Biggie-S? Got just the right thing hun -"));
            dialogue.Add(new Page("Shanice: Your old man used to use this back when we had to take out T.U.N.A. He woulda be proud to see you now, " + player.Name + "."));
            dialogue.Add(new Page("", Tuple.Create((Interactable)this, "GiveWoodenClub")));
        }

        if (player.Progress == 4)
        {
            if (player.Coins >= 10)
            {
                dialogue = new List<Page>();
                dialogue.Add(new Page("Shanice: Ice rocks? I might have the thing - but your girl here's gotta survive too, so I'll be needing 10 gold for that."));
                dialogue.Add(new Page("", Tuple.Create((Interactable)this, "GiveSpikedClub")));
            }
            else
            {
                dialogue = new List<Page>();
                dialogue.Add(new Page("Shanice: Ice rocks? I might have the thing - but your girl here's gotta survive too, so I'll be needing 10 gold for that."));
            }
        }

        if (player.Progress == 7)
        {
            if (player.Coins >= 20)
            {
                dialogue = new List<Page>();
                dialogue.Add(new Page("Shanice: Oh babe, I thought you might come across that so I prepared this for you, if you got 20 gold - this should get you through."));
                dialogue.Add(new Page("", Tuple.Create((Interactable)this, "GiveMetalClub")));
            }
            else
            {
                dialogue = new List<Page>();
                dialogue.Add(new Page("Shanice: Oh babe, I thought you might come across that so I prepared something better for you - I'll get it to you for 20 gold."));
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
        dialogue.Add(new Page("Shanice: Good luck hun!"));
    }

    public void GiveSpikedClub()
    {
        player.Progress = 5;

        player.UpdateCoins(-10);

        player.SetWeapon(1);
        player.Weapon = new Weapon(1, "Spiked Club", 20, 0.3f, "Bashing");

        dialogue = new List<Page>();
        dialogue.Add(new Page("Shanice: That should break that rock."));
    }

    public void GiveWoodenClub()
    {
        player.Progress = 2;

        player.SetWeapon(0);
        player.Weapon = new Weapon(0, "Wooden Club", 10, 0.3f, "Thwarting");

        dialogue = new List<Page>();
        dialogue.Add(new Page("Shanice: That should break those ice rocks."));
    }
}
