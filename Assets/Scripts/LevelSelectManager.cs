using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{

	public Button level1Button;
	public Button level2Button;
	public Button level3Button;
	public Button level4Button;
	public Button level5Button;
	public Button level6Button;
	public Button level7Button;
	public Button level8Button;
	public Button level9Button;
	public Button level10Button;

	void Awake()
	{
		level1Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_1); });
		level2Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_2); });
		level3Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_3); });
		level4Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_4); });
		level5Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_5); });
		level6Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_6); });
		level7Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_7); });
		level8Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_8); });
		level9Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_9); });
		level10Button.onClick.AddListener(delegate { LoadGameScene(Scenes.LEVEL_10); });
	}

	private void LoadGameScene(Scenes scene)
	{
		AudioManager.Instance.PlayGameMusic();
		LoadingScreen.LoadScene(scene);
	}
}
