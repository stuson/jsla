﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueSystem : MonoBehaviour, IPointerClickHandler {
    private TextMeshProUGUI text;
    private Image cg;
    private Queue<Message> messages;
    private DialogueInstance nextDialogue;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentText;
    [SerializeField] private DialogueInstance initialDialogue;

    void Start() {
        text = GameObject.FindGameObjectWithTag("TextBackground").GetComponentInChildren<TextMeshProUGUI>();
        cg = GameObject.FindGameObjectWithTag("CG").GetComponent<Image>();
        messages = new Queue<Message>();
        initialDialogue.StartDialogue();
    }

    void Update() {
        if (Input.GetButtonDown("Proceed")) {
            ProceedMessages();
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        cg.sprite = dialogue.background;

        messages.Clear();
        foreach(Message message in dialogue.messages) {
            messages.Enqueue(message);
        }

        nextDialogue = dialogue.nextDialogue;
        ProceedMessages();
    }

    public void ProceedMessages() {
        if (isTyping) {
            StopCoroutine(typingCoroutine);
            text.text = currentText;
            isTyping = false;
            return;
        }

        if (messages.Count == 0) {
            EndDialogue();
            return;
        }

        Message message = messages.Dequeue();
        
        string newMessage = "";

        if (message.clearLast) {
            text.text = "";
        } else {
            newMessage = "\n\n";
        }

        newMessage += message.content;
        currentText = text.text + newMessage;
        text.font = message.textSettings.font;
        text.color = message.textSettings.color;
        typingCoroutine = StartCoroutine("ShowText", newMessage);
    }

    private void EndDialogue() {
        if (nextDialogue != null) {
            nextDialogue.StartDialogue();
        } else {
            SceneManager.LoadScene("Main");
        }
    }

    public void OnPointerClick(PointerEventData pointer) {
        ProceedMessages();
    }

    private IEnumerator ShowText(string newMessage) {
        isTyping = true;
        foreach (char character in newMessage) {
            text.text += character;
            yield return new WaitForSeconds(0.03f);
        }
        isTyping = false;
    }
}