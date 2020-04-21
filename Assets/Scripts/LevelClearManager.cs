using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelClearManager : MonoBehaviour
{

	//Shared with multiple scripts, make sure UIs don't open within quick succession of each other
	public static int lastActivateFrame = 0;

	public static LevelClearManager instance;

	public CanvasGroup mainCanvasGroup;
	public CanvasGroup gameOverCanvasGroup;

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
		this.gameObject.SetActive(true);
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
		if (controlsEnabled == true)
		{
			Debug.Log("Back to main menu button from LevelClearManager pushed");
			LoadingScreen.LoadScene(Scenes.TITLE_SCREEN, delegate {
				TurnUIOff();
			});
			controlsEnabled = false;
		}
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
					TurnUIOff();
				});
				controlsEnabled = false;
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				Debug.Log("Retry button from LevelClearManager pushed");
				LoadingScreen.LoadScene(SceneManager.GetActiveScene().name, delegate
				{
					TurnUIOff();
				});
				controlsEnabled = false;
			}
		}
	}
	private void TurnUIOff() {
		Time.timeScale = 1;
		mainCanvasGroup.alpha = 0;
		gameOverCanvasGroup.alpha = 0;
		this.gameObject.SetActive(false);
	}
}
