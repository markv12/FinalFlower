using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelClearManager : MonoBehaviour {

    public static LevelClearManager instance;

    public CanvasGroup mainCanvasGroup;

    private const string  LEVEL_CLEAR_SCREEN_PATH = "LevelClearScreen";
    private static Coroutine levelClearFadeRoutine = null;
    public static void LevelClear() {
        if (instance == null) {
            GameObject gameOverScreenObject = (GameObject)Resources.Load(LEVEL_CLEAR_SCREEN_PATH);
            GameObject instantiated = Instantiate(gameOverScreenObject);
            DontDestroyOnLoad(instantiated);
            instance = instantiated.GetComponent<LevelClearManager>();
        }
        instance.ShowLevelClearScreen();
    }

    private void ShowLevelClearScreen() {
        Time.timeScale = 0;
        this.EnsureCoroutineStopped(ref levelClearFadeRoutine);
        this.CreateAnimationRoutine(
            0.3f,
            delegate (float progress) {
                mainCanvasGroup.alpha = Easing.easeInOutSine(0, 1, progress);
            },
            delegate {
                controlsEnabled = true;
            }
        );
    }

    private bool controlsEnabled = false;
    private void Update() {
        if (controlsEnabled) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                LoadingScreen.LoadScene(SceneManager.GetActiveScene().name, delegate {
                    Time.timeScale = 1;
                    mainCanvasGroup.alpha = 0;
                });
                controlsEnabled = false;
            }
        }
    }
}
