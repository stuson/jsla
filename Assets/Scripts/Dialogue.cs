using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Dialogue {
    public Message[] messages;
    public Sprite background;
    public DialogueInstance nextDialogue;
}

[System.Serializable]
public class Message {
    [TextArea(3, 10)] public string content;
    public bool clearLast;
    public TextSettings textSettings;
    
}
