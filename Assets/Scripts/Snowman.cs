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
        dialogue.Add(new Page("UIEnhancement", "UIEnhancement"));
    }

    void Update()
    {
        base.Update();
    }
}
