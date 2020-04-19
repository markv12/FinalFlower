using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{

	public Button startButton;
	public Button levelSelectButton;
	public Button rulesButton;

	void Awake()
	{
		startButton.onClick.AddListener(delegate { StartGame(); });
		levelSelectButton.onClick.AddListener(delegate { GoToLevelSelect(); });
		rulesButton.onClick.AddListener(delegate { GoToRulesScreen(); });
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
}
