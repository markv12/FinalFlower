using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{

	public static GameOverManager instance;

	public CanvasGroup mainCanvasGroup;

	private const string GAME_OVER_SCREEN_PATH = "GameOverScreen";
	private static Coroutine gameOverFadeRoutine = null;
	public static void GameOver()
	{
		if (instance == null)
		{
			GameObject gameOverScreenObject = (GameObject)Resources.Load(GAME_OVER_SCREEN_PATH);
			GameObject instantiated = Instantiate(gameOverScreenObject);
			DontDestroyOnLoad(instantiated);
			instance = instantiated.GetComponent<GameOverManager>();
		}
		instance.ShowGameOverScreen();
	}

	private void ShowGameOverScreen()
	{
		AudioManager.Instance.EnableWarningMusic(false);
		Time.timeScale = 0;
		this.EnsureCoroutineStopped(ref gameOverFadeRoutine);
		this.CreateAnimationRoutine(
				0.3f,
				delegate (float progress)
				{
					mainCanvasGroup.alpha = Easing.easeInOutSine(0, 1, progress);
				},
				delegate
				{
					controlsEnabled = true;
				}
		);
	}

	public void BackToMainMenu()
	{
		if (controlsEnabled)
		{
			LoadingScreen.LoadScene(Scenes.TITLE_SCREEN, delegate
			{
				Time.timeScale = 1;
				mainCanvasGroup.alpha = 0;
			});
		}
		controlsEnabled = false;
	}

	private bool controlsEnabled = false;
	private void Update()
	{
		if (controlsEnabled)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				LoadingScreen.LoadScene(SceneManager.GetActiveScene().name, delegate
				{
					Time.timeScale = 1;
					mainCanvasGroup.alpha = 0;
				});
				controlsEnabled = false;
			}
		}
	}
}
