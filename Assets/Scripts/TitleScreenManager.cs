using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour {

    public Button startButton;
    public Button levelSelectButton;

    void Awake() {
        startButton.onClick.AddListener(delegate { StartGame(); });
        levelSelectButton.onClick.AddListener(delegate { GoToLevelSelect(); });
        AudioManager.Instance.PlayMenuMusic();
    }

    private void StartGame() {
        AudioManager.Instance.PlayGameMusic();
        LoadingScreen.LoadScene(Scenes.LEVEL_1);
    }
    private void GoToLevelSelect() {
        LoadingScreen.LoadScene(Scenes.LEVEL_SELECT);
    }
}
