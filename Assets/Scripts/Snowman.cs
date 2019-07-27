using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : Interactable
{
    void Start()
    {
        base.Start();

        dialogue = new List<Page>();
    }

    void Update()
    {
        base.Update();

        if (player.Progress < 3)
        {
            dialogue = new List<Page>();
            dialogue.Add(new Page("Snowbird: I ain't got time for your broke a*s, come back when you're ready to spill some real blood."));
        }
        else
        {
            dialogue.Add(new Page("Snowbird: Hey lil' man, you got the coin? We'll see what I can do with that club'o yours."));
            dialogue.Add(new Page("", Tuple.Create((Interactable) this, "OpenEnhancements")));
        }
    }

    public void OpenEnhancements()
    {
        Enhance enhance = canvas.transform.Find("UIEnhancement").GetComponent<Enhance>();
        enhance.Open();
        player.SetListening(true);
    }
}
