using System.Collections;
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
        text.font = message.character.font;
        text.color = message.character.color;
        object[] args = new object[2]{ newMessage, message.character.voice };
        typingCoroutine = StartCoroutine("ShowText", args);
    }

    private void EndDialogue() {
        if (nextDialogue != null) {
            nextDialogue.StartDialogue();
        } else {
            SceneManager.LoadScene("TitleScreen");
        }
    }

    public void OnPointerClick(PointerEventData pointer) {
        ProceedMessages();
    }

    private IEnumerator ShowText(object[] args) {
        string newMessage = (string)args[0];
        AudioSource voice = (AudioSource)args[1];
        
        isTyping = true;
        foreach (char character in newMessage) {
            text.text += character;
            if (!char.IsWhiteSpace(character)) {
                voice.PlayOneShot(voice.clip);
            }
            yield return new WaitForSeconds(0.06f);
        }
        isTyping = false;
    }
}
