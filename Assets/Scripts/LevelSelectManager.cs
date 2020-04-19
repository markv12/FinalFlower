using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour {

    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    void Awake() {
        level1Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_1); });
        level2Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_2); });
        level3Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_3); });
    }

    private void LoadGameScene(Scenes scene) {
        AudioManager.Instance.PlayGameMusic();
        LoadingScreen.LoadScene(scene);
    }
}
