using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInstance : MonoBehaviour {
    public Dialogue dialogue;

    public void StartDialogue() {
        GameObject.FindGameObjectWithTag("CG").GetComponent<DialogueSystem>().StartDialogue(dialogue);
    }
}