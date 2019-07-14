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
    }

    void Update()
    {
        base.Update();
    }
}
