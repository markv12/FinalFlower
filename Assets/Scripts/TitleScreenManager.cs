using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{

	public Button startButton;
	public Button levelSelectButton;
	public Button rulesButton;
	public Button quitButton;

	void Awake()
	{
		startButton.onClick.AddListener(delegate { StartGame(); });
		levelSelectButton.onClick.AddListener(delegate { GoToLevelSelect(); });
		rulesButton.onClick.AddListener(delegate { GoToRulesScreen(); });
		quitButton.onClick.AddListener(delegate { Quit(); });
		AudioManager.Instance.PlayMenuMusic();
	}

	private void StartGame()
	{
		AudioManager.Instance.PlayGameMusic();
		LoadingScreen.LoadScene(Scenes.LEVEL_1);
	}
	private void GoToLevelSelect()
	{
		LoadingScreen.LoadScene(Scenes.LEVEL_SELECT);
	}
	private void GoToRulesScreen()
	{
		LoadingScreen.LoadScene(Scenes.RULE_SCREEN);
	}
	private void Quit()
	{
		Debug.Log("Quitting");
		Application.Quit();
	}
}
