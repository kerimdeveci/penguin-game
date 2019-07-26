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
        dialogue.Add(new Page("Ayo, is that you? Ain't seen you in forever!"));
        dialogue.Add(new Page("", Tuple.Create((Interactable)this, "UpdateProgress1")));

    }

    void Update()
    {
        base.Update();

        if (player.Progress == 0)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("Toni: Ayo, " + player.Name + ", is that you? Ain't seen you in forever!"));
            dialogue.Add(new Page("You: Hey waddup."));
            dialogue.Add(new Page("Toni: Haven't forgotten about your old man Toni have you? We missed you 'round here, man!"));
            dialogue.Add(new Page("Toni: Tell you what though, things haven't been the same these streets. Them damn seals are back again - even brought Biggie-S with'em this time."));
            dialogue.Add(new Page("You: No way man, he's here? Right now? We gotta do something."));
            dialogue.Add(new Page("Toni: Y'tellin' me. We thinkin' that we get rid of Biggie-S and the rest'll get off our turf. Help us out would ya?"));
            dialogue.Add(new Page("You: Sure thing my man, what you need?"));
            dialogue.Add(new Page("Toni: Our girl Shanice over there has some clubs. Real nice, do some real damage to them. They should do the job."));
            dialogue.Add(new Page("Toni: If you got any problems, just holler at me."));

            dialogue.Add(new Page("", Tuple.Create((Interactable)this, "UpdateProgress1")));
        }
                
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
            dialogue.Add(new Page("Toni: You got yourself a club from Shanice yet?"));
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
