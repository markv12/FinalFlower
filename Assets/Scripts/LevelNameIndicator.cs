using UnityEngine;
using TMPro;
using System.Collections;

public class LevelNameIndicator : MonoBehaviour
{
	public static LevelNameIndicator instance;

	public CanvasGroup mainCanvasGroup;
	public TMP_Text levelText;

	private const string LEVEL_NAME_INDICATOR_PATH = "LevelNameIndicator";
	private static Coroutine levelNameIndicatorRoutine = null;
	private static Coroutine levelNameIndicatorSubRoutine = null;
	public static void ShowLevelName(string levelName)
	{
		if (instance == null)
		{
			GameObject showLevelObject = (GameObject)Resources.Load(LEVEL_NAME_INDICATOR_PATH);
			GameObject instantiated = Instantiate(showLevelObject);
			DontDestroyOnLoad(instantiated);
			instance = instantiated.GetComponent<LevelNameIndicator>();
		}
		instance._ShowLevelName(levelName);
	}

	private void _ShowLevelName(string levelName)
	{
		levelText.text = levelName;
		mainCanvasGroup.alpha = 1;
		gameObject.SetActive(true);
		this.EnsureCoroutineStopped(ref levelNameIndicatorRoutine);
		this.EnsureCoroutineStopped(ref levelNameIndicatorSubRoutine);
		levelNameIndicatorRoutine = StartCoroutine(LevelNameAnimation());
	}

	private static readonly WaitForSeconds nameWait = new WaitForSeconds(1.5f);
	private IEnumerator LevelNameAnimation() {
		yield return nameWait;
		yield return levelNameIndicatorSubRoutine = this.CreateAnimationRoutine(
			0.666f,
			delegate (float progress) {
				mainCanvasGroup.alpha = Easing.easeInOutSine(1, 0, progress);
			},
			delegate {
				gameObject.SetActive(false);
			}
		);
	}
}
