#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine.SceneManagement;

public static class SceneLoader {
    public static void LoadScene(Scenes scene) {
        SceneManager.LoadScene(scene.Name());
    }

#if UNITY_EDITOR
    [MenuItem("Scenes/Load Title Screen")]
    public static void LoadMainMenu() { OpenScene(Scenes.TITLE_SCREEN.Name()); }

    private const string PATH = "Assets/scenes/";
    private const string SCENE_SUFFIX = ".unity";

    private static void OpenScene(string sceneName) {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(GetScenePath(sceneName), OpenSceneMode.Single);
    }

    private static string GetScenePath(string sceneName) {
        return PATH + sceneName + SCENE_SUFFIX;
    }
#endif

    private static Dictionary<string, Scenes> nameToScene;
    private static Dictionary<string, Scenes> NameToScene {
        get {
            if(nameToScene == null) {
                nameToScene = new Dictionary<string, Scenes>();
                IEnumerable<Scenes> allScenes = System.Enum.GetValues(typeof(Scenes)).Cast<Scenes>();
                foreach(Scenes s in allScenes) {
                    nameToScene[s.Name()] = s;
                }
            }
            return nameToScene;
        }
    }

    public static string GetNextSceneName(string sceneName) {
        Scenes s = NameToScene[sceneName];
        return (s + 1).Name();
    }
}

public enum Scenes {
    TITLE_SCREEN,
    LEVEL_SELECT,
    LEVEL_1,
    LEVEL_2,
}

public static class ScenesExtensions {
    public static string Name(this Scenes me) {
        switch (me) {
            case Scenes.TITLE_SCREEN:
                return "TitleScreen";
            case Scenes.LEVEL_SELECT:
                return "LevelSelect";
            case Scenes.LEVEL_1:
                return "Level 1";
            case Scenes.LEVEL_2:
                return "Level 2";
            default:
                return "Scene Not Found";
        }
    }
}
