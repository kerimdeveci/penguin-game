using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : Interactable
{
    void Start()
    {
        base.Start();

        dialogue = new List<Page>();
        dialogue.Add(new Page("Hello", null));
        dialogue.Add(new Page("Tasdg", null));
        dialogue.Add(new Page("Pack my box with five dozen liquor jugs", null));
        dialogue.Add(new Page("The five asdasdasdasd wizards jump quickly", null));
    }

    void Update()
    {
        base.Update();
    }
}
