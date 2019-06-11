using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject player;
    public GameObject interactPrompt;
    public CanvasGroup interactPromptCanvasGroup;
    bool playerInRange = false;
    bool talking = false;
    bool waitingForInput = false;

    List<Page> dialogue;
    int dialogueCursor = 0;
    float lastAction;
    float lastActionCooldown = 0.5f;

    void Awake()
    {
        interactPromptCanvasGroup = interactPrompt.GetComponent<CanvasGroup>();
        interactPromptCanvasGroup.alpha = 0;lastAction = Time.time;
        lastAction = Time.time;

        dialogue = new List<Page>();
        dialogue.Add(new Page("Hello", null));
        dialogue.Add(new Page("The quick brown fox jumps over the lazy dog", null));
        dialogue.Add(new Page("Pack my box with five dozen liquor jugs", null));
        dialogue.Add(new Page("The five boxing wizards jump quickly", null));
    }
    
    void Update()
    {
        UpdatePrompt();
        CheckPromptInput();
        UpdateDialogue();
        CheckDialogueInput();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Player entered interactive area");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        if (other.gameObject == player)
        {
            Debug.Log("Player left interactive area");
        }
    }

    void UpdateDialogue()
    {
        if (talking && !waitingForInput)
        {
            NextPage();
        }
        
        if (talking && !playerInRange)
        {
            Debug.Log("End Conversation");
            dialogueCursor = 0;
            talking = false;
        }
    }

    void CheckDialogueInput()
    {
        //Debug.Log(talking && waitingForInput && Time.time - lastAction > lastActionCooldown);
        if (talking && waitingForInput && Time.time - lastAction > lastActionCooldown)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                lastAction = Time.time;
                waitingForInput = false;
            }
        }
    }

    void NextPage()
    {
        if (dialogueCursor >= dialogue.Count)
        {
            Debug.Log("End Conversation");
            dialogueCursor = 0;
            talking = false;
            return;
        }
        Debug.Log("Turn to page " + dialogueCursor);
        Debug.Log(dialogue[dialogueCursor].GetMessage());
        dialogueCursor++;
        waitingForInput = true;
    }

    void CheckPromptInput()
    {
        if (playerInRange && !talking)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                lastAction = Time.time;
                Debug.Log("Start conversation");
                talking = true;
            }
        }
    }

    void UpdatePrompt()
    {
        if (playerInRange && !talking)
        {
            interactPromptCanvasGroup.alpha += 0.2f;
        }
        else
        {
            interactPromptCanvasGroup.alpha -= 0.2f;
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