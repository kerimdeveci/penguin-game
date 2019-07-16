using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using System;

public class DialogueBox : MonoBehaviour
{
    Player player;
    GameObject canvas;
    InteractPrompt interactPrompt;
    GameObject dialogueBox;
    Text textBox;
    
    bool playerInRange = false;
    bool talking = false;
    bool waitingForInput = false;

    List<Page> dialogue;
    int dialogueCursor = 0;
    float lastAction = 0;
    float lastActionCooldown = 0.1f;

    public bool scrolling;
    float lastPrint;
    float scrollSpeed;
    int scrollingCursor;
    string currentLine;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        dialogueBox = canvas.transform.Find("DialogueBox").gameObject;
        textBox = canvas.transform.Find("DialogueBox/DialogueText").GetComponent<Text>();
        dialogueBox.SetActive(false);
        Debug.Log(textBox);

        scrolling = false;
        scrollSpeed = 0.005f;
        scrollingCursor = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDialogue();
        CheckDialogueInput();
    }

    void UpdateDialogue()
    {
        if (talking && !waitingForInput)
        {
            if (scrolling && Time.time - lastPrint >= scrollSpeed)
            {
                textBox.text += currentLine.ToCharArray()[scrollingCursor].ToString();
                ++scrollingCursor;
                if (scrollingCursor == currentLine.Length)
                {
                    scrolling = false;
                    waitingForInput = true;
                }
                lastPrint = Time.time;
            }
        }
    }

    void CheckDialogueInput()
    {
        //Debug.Log(talking);
        //Debug.Log(waitingForInput);
        //Debug.Log(Time.time - lastAction > lastActionCooldown);
        if (talking && waitingForInput && Time.time - lastAction > lastActionCooldown)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                lastAction = Time.time;
                NextPage();
            }
        }
    }

    void NextPage()
    {
        Debug.Log("NextPage");
        if (dialogueCursor >= dialogue.Count)
        {
            EndConversation();
            return;
        }
        Debug.Log(dialogue[dialogueCursor].Type);
        if (dialogue[dialogueCursor].Type == Page.PageType.Function)
        {
            Debug.Log(dialogue[dialogueCursor].Callback.Item1);
            Debug.Log(dialogue[dialogueCursor].Callback.Item2);
            Type thisType = dialogue[dialogueCursor].Callback.Item1.GetType();
            Debug.Log(thisType);
            MethodInfo theMethod = thisType.GetMethod(dialogue[dialogueCursor].Callback.Item2);
            Debug.Log(theMethod);
            theMethod.Invoke(dialogue[dialogueCursor].Callback.Item1, null);
            EndConversation();
            return;
        }
        textBox.text = "";
        currentLine = dialogue[dialogueCursor].Message;
        dialogueCursor++;
        scrollingCursor = 0;
        scrolling = true;
        waitingForInput = false;
    }

    public void EndConversation()
    {
        Debug.Log("End Conversation");
        dialogueBox.SetActive(false);
        textBox.text = "";
        dialogueCursor = 0;
        talking = false;
        player.SetListening(false);
    }

    public void StartConversation(List<Page> dialogue)
    {
        Debug.Log("Start Conversation");
        dialogueBox.SetActive(true);
        this.dialogue = dialogue;
        talking = true;
        player.SetListening(true);
        NextPage();
    }
}
