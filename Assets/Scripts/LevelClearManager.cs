using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelClearManager : MonoBehaviour
{

	public static LevelClearManager instance;

	public CanvasGroup mainCanvasGroup;

	private const string LEVEL_CLEAR_SCREEN_PATH = "LevelClearScreen";
	private static Coroutine levelClearFadeRoutine = null;
	public static void LevelClear(float timeInSeconds)
	{
		if (instance == null)
		{
			GameObject gameOverScreenObject = (GameObject)Resources.Load(LEVEL_CLEAR_SCREEN_PATH);
			GameObject instantiated = Instantiate(gameOverScreenObject);
			DontDestroyOnLoad(instantiated);
			instance = instantiated.GetComponent<LevelClearManager>();
		}
		instance.ShowLevelClearScreen(timeInSeconds);
	}

	private void ShowLevelClearScreen(float timeInSeconds)
	{
		gameObject.GetComponent<HighScoreManager>().setup(timeInSeconds);
		Time.timeScale = 0;
		this.EnsureCoroutineStopped(ref levelClearFadeRoutine);
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
			Debug.Log("Back to main menu button from LevelClearManager pushed");
			LoadingScreen.LoadScene(Scenes.TITLE_SCREEN, delegate
			{
				Time.timeScale = 1;
				mainCanvasGroup.alpha = 0;
			});
		}
		controlsEnabled = false;
	}

	private bool controlsEnabled = false;

	public void ToggleControlsEnabled()
	{
		controlsEnabled = !controlsEnabled;
	}

	private void Update()
	{
		if (controlsEnabled)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Debug.Log("Next level button from LevelClearManager pushed");
				LoadingScreen.LoadScene(SceneLoader.GetNextSceneName(SceneManager.GetActiveScene().name), delegate
				{
					Time.timeScale = 1;
					mainCanvasGroup.alpha = 0;
				});
				controlsEnabled = false;
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				Debug.Log("Retry button from LevelClearManager pushed");
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
