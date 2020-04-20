using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{

	public Button backButton;

	void Awake()
	{
		backButton.onClick.AddListener(delegate { LoadGameScene(Scenes.TITLE_SCREEN); });
	}

	private void LoadGameScene(Scenes scene)
	{
		LoadingScreen.LoadScene(scene);
	}
}
