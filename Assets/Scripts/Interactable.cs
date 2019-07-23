using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Player player;
    public GameObject canvas;
    InteractPrompt interactPrompt;
    DialogueBox dialogueBox;
    UI ui;
    bool playerInRange = false;
    bool talking = false;
    bool waitingForInput = false;

    public List<Page> dialogue;
    int dialogueCursor = 0;
    float lastAction;
    float lastActionCooldown = 0.5f;


    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        interactPrompt = canvas.transform.Find("InteractPrompt").GetComponent<InteractPrompt>();
        dialogueBox = canvas.transform.Find("DialogueBox").GetComponent<DialogueBox>();
        ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UI>();

        dialogue = new List<Page>();
        dialogue.Add(new Page("NPC does not have dialogue set"));
    }
    
    public void Update()
    {
        CheckPromptInput();
        CheckPlayerListening();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Player entered interactive area");
            playerInRange = true;
            interactPrompt.FadeIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Player left interactive area");
            interactPrompt.FadeOut();
            dialogueBox.EndConversation();
            talking = false;
        }
    }

    void CheckPromptInput()
    {
        if (playerInRange && !talking && !ui.IsPaused())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                lastAction = Time.time;
                dialogueBox.StartConversation(dialogue);
                talking = true;
            }
            if (!interactPrompt.IsVisible())
            {
                interactPrompt.FadeIn();
            }
        }
    }

    void CheckPlayerListening()
    {
        if (talking)
        {
            if (!player.IsListening())
            {
                talking = false;
            }
        }
    }
}

public class Page
{
    public enum PageType { Line, Option, Function };

    public string Message { get; set; }
    public Tuple<Interactable, string> Callback { get; set; }
    public OrderedDictionary Options { get; set; }
    public PageType Type { get; set; }

    public Page(string message)
    {
        this.Message = message;
        this.Options = null;
        this.Callback = null;
        this.Type = PageType.Line;
    }

    public Page(string message, Tuple<Interactable, string> callback)
    {
        this.Message = message;
        this.Callback = callback;
        this.Options = null;
        this.Type = PageType.Function;
    }

    public Page(string message, OrderedDictionary options)
    {
        this.Message = message;
        this.Options = options;
        this.Callback = null;
        this.Type = PageType.Option;
    }
}