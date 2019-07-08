using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public GameObject player;
    GameObject canvas;
    InteractPrompt interactPrompt;
    Text textBox;
    
    bool playerInRange = false;
    bool talking = false;
    bool waitingForInput = false;

    List<Page> dialogue;
    int dialogueCursor = 0;
    float lastAction = 0;
    float lastActionCooldown = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        textBox = canvas.transform.Find("DialogueBox/DialogueText").GetComponent<Text>();
        Debug.Log(textBox);
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
            NextPage();
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
                waitingForInput = false;
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
        Debug.Log("Turn to page " + dialogueCursor);
        Debug.Log(dialogue[dialogueCursor].GetMessage());
        textBox.text = dialogue[dialogueCursor].GetMessage();
        dialogueCursor++;
        waitingForInput = true;
    }

    public void EndConversation()
    {
        dialogueCursor = 0;
        talking = false;
        Debug.Log("End Conversation");
    }

    public void StartConversation(List<Page> dialogue)
    {
        Debug.Log("Start Conversation");
        this.dialogue = dialogue;
        talking = true;
        NextPage();
    }
}
