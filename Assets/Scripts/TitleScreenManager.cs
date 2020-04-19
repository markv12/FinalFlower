using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour {

    public Button startButton;
    public Button levelSelectButton;

    // Start is called before the first frame update
    void Awake() {
        startButton.onClick.AddListener(delegate { StartGame(); });
        levelSelectButton.onClick.AddListener(delegate { GoToLevelSelect(); });
    }

    private void StartGame() {
        LoadingScreen.LoadScene(Scenes.LEVEL_1);
    }
    private void GoToLevelSelect() {
        LoadingScreen.LoadScene(Scenes.LEVEL_SELECT);
    }
}
