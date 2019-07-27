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
            dialogue.Add(new Page("Toni: Tell you what though, things haven't been the same these streets. Them d*mn seals are back again - even brought Biggie-S with'em this time."));
            dialogue.Add(new Page("You: No way man, he's here? Right now? We gotta do something the seals."));
            dialogue.Add(new Page("Toni: Y'tellin' me. We thinkin' that we get rid of Biggie-S and the rest'll get off our turf. Help us out would ya?"));
            dialogue.Add(new Page("You: Sure thing my man, what you need?"));
            dialogue.Add(new Page("Toni: Our girl Shanice over there has some clubs. Real nice, do some real damage to them. They should do the job."));
            dialogue.Add(new Page("Toni: If you got any problems, just holler at me."));

            dialogue.Add(new Page("", Tuple.Create((Interactable)this, "UpdateProgress1")));
        }
                
        if (player.Progress == 3)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("You: Yo the seals are out there for sure, but there's some ice blocking the way to Biggie-S - you got anything for that?"));
            dialogue.Add(new Page("Toni: Ah h*ck, that club of yours ain't gonna do it on those rocks huh - Shanice probably got some stuff that can break through, but she been real tight on cash these days so she ain't helping us with much without some payment."));
            dialogue.Add(new Page("Toni: Go 'round to her and see if there's anything she can do."));
            dialogue.Add(new Page("", Tuple.Create((Interactable)this, "UpdateProgress4")));
        }

        if (player.Progress == 6)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("You: It's some big a*s rock this time man, still can't get through."));
            dialogue.Add(new Page("Toni: No matter man, we'll make it through. You been doing a real great job so far. We try another club with Shanice again."));
            dialogue.Add(new Page("Toni: By the way, you tried talking to that Snowbird? Weird dude but if you got the cash he can upgrade them clubs."));
            dialogue.Add(new Page("", Tuple.Create((Interactable)this, "UpdateProgress7")));
        }

        if (player.Progress == 8 || player.Progress == 9)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("Toni: Do us proud, man."));
        }

        if (player.Progress == 1)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("Toni: You got yourself a club from Shanice yet? We gotta find where Biggie-S at and whoop his a*s."));
        }

        if (player.Progress == 4)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("Toni: Shanice got you a club to get through the ice yet? We gotta be getting close to Biggie-S."));
        }

        if (player.Progress == 7)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("Toni: Shanice got you a club to get through the huge rock yet?"));
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
