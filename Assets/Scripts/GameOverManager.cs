using UnityEngine.SceneManagement;

public static class GameOverManager {

    public static void GameOver() {
        LoadingScreen.LoadScene(SceneManager.GetActiveScene().name);
    }
}
