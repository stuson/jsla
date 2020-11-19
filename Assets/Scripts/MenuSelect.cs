using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelect : MonoBehaviour {
    [SerializeField] private MenuOption[] menuOptions;
    private int optionIndex = 0;

    void Update() {
        if (Input.GetKeyDown("up")) {
            NextOption(-1);
        } else if (Input.GetKeyDown("down")) {
            NextOption(1);
        } else if (Input.GetButtonDown("Proceed")) {
            TriggerOption();
        }
    }

    private void NextOption(int direction) {
        DeselectAllOptions();
        optionIndex = (optionIndex + direction) % menuOptions.Length;
        menuOptions[optionIndex].Select();
    }

    private void TriggerOption() {
        menuOptions[optionIndex].Trigger();
    }

    private void DeselectAllOptions() {
        foreach (MenuOption option in menuOptions) {
            option.Deselect();
        }
    }
}