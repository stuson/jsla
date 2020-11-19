using UnityEngine.SceneManagement;

public class StartButton : MenuOption {
    public override void Trigger() {
        SceneManager.LoadScene("Main");
    }
}