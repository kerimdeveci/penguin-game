using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    Player player;
    GameObject canvas;
    InteractPrompt interactPrompt;
    DialogueBox dialogueBox;
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

        dialogue = new List<Page>();
        dialogue.Add(new Page("NPC does not have dialogue set", null));
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
        if (playerInRange && !talking)
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
    private string message;
    private OrderedDictionary options;

    public Page(string message)
    {
        this.message = message;
        this.options = null;
    }

    public Page(string message, OrderedDictionary options)
    {
        this.message = message;
        this.options = options;
    }

    public bool HasOptions()
    {
        return this.options != null;
    }

    public string GetMessage()
    {
        return message;
    }

    public OrderedDictionary GetOptions()
    {
        return options;
    }
}