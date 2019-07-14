using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toni : Interactable
{
    void Start()
    {
        base.Start();

        dialogue = new List<Page>();
        dialogue.Add(new Page("Hello"));
        dialogue.Add(new Page("The quick brown fox jumps over the lazy dog"));
        dialogue.Add(new Page("Pack my box with five dozen liquor jugs"));
        dialogue.Add(new Page("The five boxing wizards jump quickly"));
    }

    void Update()
    {
        base.Update();
    }
}
