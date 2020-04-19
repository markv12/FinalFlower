using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour {

    public Text playerScoreTextObject;
    public Text highScoreTextObject;
    public Text playerNameTextObject;
    public GameObject inputZone;
    public int scoreCount;
    public string leaderboardName = "ludumdare46";

    string[] names = { };
    string[] scores = { };
    float playerScore = 0;

    void Start () {
        setPlayerScore (3);
        StartCoroutine (getHighScores ());
    }

    public void saveScore () {
        string name = playerNameTextObject.text;
        if (name == "") return;
        Debug.Log ("Adding high score " + playerScore + " for player " + name);
        StartCoroutine (addHighScore (name, playerScore));
        inputZone.SetActive (false);
    }

    public void setPlayerScore (float score) {
        playerScore = score;
        playerScoreTextObject.text = "Your Score: " + score.ToString ();
    }

    public void restart () {
        SceneManager.LoadScene ("MainScene");
    }

    void updateHighScoreLabel () {
        bool hasShownPlayer = false;
        string scoreLabel = "";
        string playerName = playerNameTextObject.text;
        if (playerName == "") {
            playerName = "You";
        }
        for (int i = 0; i < names.Length; i++) {
            if (!hasShownPlayer && playerScore <= float.Parse (scores[i])) {
                scoreLabel += playerName + "   " + playerScore + '\n';
                hasShownPlayer = true;
            }
            scoreLabel += names[i] + "   " + scores[i] + '\n';
        }
        if (scoreLabel == "") {
            scoreLabel = playerName + "   " + playerScore + '\n';
        }
        highScoreTextObject.text = scoreLabel;
    }

    IEnumerator getHighScores () {
        Debug.Log ("Updating high scores");
        string url = "https://agile-citadel-44322.herokuapp.com/" + leaderboardName + "/bottom/" + scoreCount.ToString () + '/';
        using (UnityWebRequest webRequest = UnityWebRequest.Get (url)) {
            yield return webRequest.SendWebRequest ();

            if (webRequest.isNetworkError) {
                Debug.Log ("Network Error: " + webRequest.error);
            } else if (webRequest.downloadHandler.text == "Forbidden" || webRequest.downloadHandler.text == "Internal Server Error") {
                Debug.Log ("Network Error: " + webRequest.downloadHandler.text);
            } else {
                Debug.Log ("Received high scores: " + webRequest.downloadHandler.text);
                string[] highScoreStrings;
                string[] stringSplitter = new string[] { "\\n" };
                string highScoreString = webRequest.downloadHandler.text.Substring (1, webRequest.downloadHandler.text.Length - 2);
                if (webRequest.downloadHandler.text.IndexOf ("\\n") == -1) {
                    Debug.Log ("Only one score: " + highScoreString);
                    highScoreStrings = new string[] { highScoreString };
                } else {
                    highScoreStrings = highScoreString.Split (stringSplitter, System.StringSplitOptions.None);
                }

                int scoreCountToDisplay = scoreCount;
                if (highScoreStrings.Length < scoreCountToDisplay) {
                    scoreCountToDisplay = highScoreStrings.Length;
                }

                names = new string[scoreCountToDisplay];
                scores = new string[scoreCountToDisplay];

                for (int s = 0; s < highScoreStrings.Length; s++) {
                    string[] split = highScoreStrings[s].Split (',');
                    names[s] = split[0];
                    scores[s] = split[1];
                    if (playerScore <= float.Parse (split[1])) {
                        Debug.Log ("High Score!");
                        inputZone.SetActive (true);
                    }
                }
            }
            updateHighScoreLabel ();
        }
    }

    IEnumerator addHighScore (string name, float score) {
        updateHighScoreLabel ();
        string url = "https://agile-citadel-44322.herokuapp.com/" + leaderboardName + "/add/" + name + '/' + score.ToString () + '/';
        using (UnityWebRequest webRequest = UnityWebRequest.Get (url)) {
            yield return webRequest.SendWebRequest ();

            if (webRequest.isNetworkError) {
                Debug.Log ("Network Error: " + webRequest.error);
            } else if (webRequest.downloadHandler.text == "Forbidden" || webRequest.downloadHandler.text == "Internal Server Error") {
                Debug.Log ("Network Error: " + webRequest.downloadHandler.text);
            } else {
                Debug.Log ("Successfully added new score");
            }
        }
    }
}