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
    private int revealed;
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

        if (message.clearLast) {
            revealed = 0;
            currentText = "";
        } else {
            currentText += "\n\n";
        }

        currentText += message.content;
        text.font = message.character.font;
        text.color = message.character.color;
        typingCoroutine = StartCoroutine("ShowText", message.character.voice);
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

    private IEnumerator ShowText(AudioSource voice) {        
        string cTag = "<color=#00000000>";
        isTyping = true;

        for (int i=revealed; i < currentText.Length; ++i) {
            text.text = currentText.Insert(i + 1, cTag) + "</color>";
            if (!char.IsWhiteSpace(currentText[i])) {
                voice.pitch = Random.Range(0.95f, 1f);
                voice.PlayOneShot(voice.clip, Random.Range(0.9f, 1f));
            }

            revealed = i;
            yield return new WaitForSeconds(0.02f);
        }
        isTyping = false;
    }
}
