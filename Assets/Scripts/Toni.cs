using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toni : Interactable
{
    void Start()
    {
        base.Start();

        dialogue = new List<Page>();
        dialogue.Add(new Page("is that you"));
        dialogue.Add(new Page("", Tuple.Create((Interactable)this, "UpdateProgress1")));

    }

    void Update()
    {
        base.Update();
                
        if (player.Progress == 3)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("you went to monster? break ice with shanice"));
            dialogue.Add(new Page("", Tuple.Create((Interactable)this, "UpdateProgress4")));
        }

        if (player.Progress == 6)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("you went to rock? break rock with shanice"));
            dialogue.Add(new Page("", Tuple.Create((Interactable)this, "UpdateProgress7")));
        }

        if (player.Progress == 8)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("do us proud, man"));
        }

        if (player.Progress == 1)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("talk to shanice for wooden club"));
        }

        if (player.Progress == 4)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("talk to shanice for spiked club"));
        }

        if (player.Progress == 7)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("talk to shanice for metal club"));
        }
    }

    public void UpdateProgress1()
    {
        if (player.Progress == 0)
        {
            player.Progress = 1;
        }
    }

    public void UpdateProgress4()
    {
        if (player.Progress == 3)
        {
            player.Progress = 4;
        }
    }

    public void UpdateProgress7()
    {
        if (player.Progress == 6)
        {
            player.Progress = 7;
        }
    }
}
