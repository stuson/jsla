using UnityEngine;

public class ExitButton : MenuOption {
    public override void Trigger() {
        Application.Quit();
    }
}