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
        dialogue.Add(new Page("Hello"));
        dialogue.Add(new Page("UIEnhancement", Tuple.Create((Interactable) this, "OpenEnhancements")));
    }

    void Update()
    {
        base.Update();
    }

    public void OpenEnhancements()
    {
        GameObject window = canvas.transform.Find("UIEnhancement").gameObject;
        window.GetComponent<CanvasGroup>().alpha = 1;
        player.SetListening(true);
    }
}
