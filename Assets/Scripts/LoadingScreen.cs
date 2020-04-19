using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public Image mainImage;

    public static LoadingScreen instance;

    private const string LOADING_SCREEN_PATH = "LoadingScreen";
    public static void LoadScene(Scenes scene, System.Action onComplete = null)
    {
        LoadScene(scene.Name(), onComplete);
    }

    public static void LoadScene(string sceneName, System.Action onComplete = null) {
        if (instance == null) {
            GameObject loadingScreenObject = (GameObject)Resources.Load(LOADING_SCREEN_PATH);
            GameObject instantiated = Instantiate(loadingScreenObject);
            DontDestroyOnLoad(instantiated);
            instance = instantiated.GetComponent<LoadingScreen>();
        }
        instance.FadeLoadScene(sceneName, onComplete);
    }

    private Coroutine loadRoutine = null;
    private void FadeLoadScene(string sceneName, System.Action onComplete)
    {
        if (loadRoutine != null)
        {
            //Debug.LogError("Tried loading while load already in progress");
        }
        else {
            loadRoutine = StartCoroutine(_FadeLoadScene(sceneName, onComplete));
        }
    }

    private const float FADE_TIME = 0.222f;
    private IEnumerator _FadeLoadScene(string sceneName, System.Action onComplete)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        mainImage.enabled = true;
        Color currentColor = Color.white;
        Color startColor = currentColor.SetA(mainImage.color.a);
        Color endColor = currentColor;
        float progress = 0;
        float elapsedTime = 0;
        while (progress <= 1)
        {
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / FADE_TIME;
            mainImage.color = Color.Lerp(startColor, endColor, Easing.easeInOutSine(0, 1, progress));
            yield return null;
        }
        mainImage.color = endColor;
        yield return null;
        async.allowSceneActivation = true;
        yield return null;

        onComplete?.Invoke();

        startColor = mainImage.color;
        endColor = currentColor.SetA(0);
        progress = 0;
        elapsedTime = 0;
        while (progress <= 1)
        {
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / FADE_TIME;
            mainImage.color = Color.Lerp(startColor, endColor, Easing.easeInOutSine(0, 1, progress));
            yield return null;
        }
        mainImage.color = endColor;
        mainImage.enabled = false;
        loadRoutine = null;
    }
}
